#![allow(
    non_snake_case,
    unused_imports
)]
//// File: event.rs
/// File Creator: John Sbur
/// Purpose: Contains methods transferred from the Index area. Not all methods are transferred, only necessary ones.
///     Provides definitions on the Rust side.

// Global Imports
use crate::LIBRARY_CLIENT_FACTORY;

// Crate Imports
use crate::utility::{CustomRustString, U8Slice};

// Library Imports
use futures::executor;
use interoptopus::{Inventory, InventoryBuilder};
use once_cell::sync::OnceCell;
use pravega_client::{client_factory::ClientFactory};
use pravega_client::client_factory::ClientFactoryAsync;
use pravega_client::byte::{ByteReader,ByteWriter};
use pravega_client::event::{ReaderGroup,EventWriter};
use pravega_client_config::connection_type;
use pravega_client_shared::{ScopedStream, Scope, Stream};
use std::ops::Deref;
use std::{thread, time};
use std::io::{Error, ErrorKind, SeekFrom};
use std::time::Duration;
use tokio::runtime::{Builder, Runtime, EnterGuard};
use tokio::task;
use tokio::task::JoinHandle;


//use utility_wrapper::U8Slice;

// Used for interoptopus wrapping
pub fn my_inventory() -> Inventory {
    {
        InventoryBuilder::new()
        .inventory()
    }
}

// ReaderGroup default constructor


#[no_mangle]
pub extern "C" fn CreateReaderGroup(
    client_factory_ptr: &'static ClientFactory,
    scope: CustomRustString,
    stream: CustomRustString,
    key: u64,
    callback: unsafe extern "C" fn(u64, *const ReaderGroup)){


        // Construct scopedstream and clientfactoryasync from function inputs
        let scope_converted = Scope{
            name: scope.as_string()
        };
        let stream_converted = Stream{
            name: stream.as_string()
        };
        let ss: ScopedStream = ScopedStream{
            scope: scope_converted,
            stream: stream_converted  
        };
        println!("above block on");
        // Create the new bytewriter asynchronously with the inputted runtime and the
        client_factory_ptr.runtime().block_on( async move {
            println!("in block on");
            let result: ReaderGroup = client_factory_ptr.create_reader_group("test".to_string(),ss).await;
            println!("below create RG");
            let result_box: Box<ReaderGroup> = Box::new(result);
            let result_ptr: *const ReaderGroup = Box::into_raw(result_box);
            println!("From Rust");
            println!("{:?}",result_ptr);
            unsafe { callback(key,result_ptr) };
        }) ;
        
}

#[no_mangle]
pub extern "C" fn CreateEventWriter(
    scope: CustomRustString,
    stream: CustomRustString,
    key: u64,
    callback: unsafe extern "C" fn(u64, *const EventWriter)){


        // Construct scopedstream and clientfactoryasync from function inputs
        let scope_converted = Scope{
            name: scope.as_string()
        };
        let stream_converted = Stream{
            name: stream.as_string()
        };
        let ss: ScopedStream = ScopedStream{
            scope: scope_converted,
            stream: stream_converted  
        };
        println!("above block on");
        // Create the new bytewriter asynchronously with the inputted runtime and the
        LIBRARY_CLIENT_FACTORY.get().unwrap().runtime().block_on( async move {
            println!("in block on");
            let result: EventWriter = LIBRARY_CLIENT_FACTORY.get().unwrap().create_event_writer(ss);
            println!("below create EW");
            let result_box: Box<EventWriter> = Box::new(result);
            let result_ptr: *const EventWriter = Box::into_raw(result_box);
            println!("From Rust");
            println!("{:?}",result_ptr);
            unsafe { callback(key,result_ptr) };
        }) ;
        
}


#[no_mangle]
pub extern "C" fn EventWriterDrop(EW: &mut EventWriter)
{
    drop(EW);
}

#[no_mangle]
pub extern "C" fn EventWriterFlush(
event_writer_ptr: &mut EventWriter,
key: u64,
callback: unsafe extern "C" fn (u64, u64)
)
{
// Block on the client factory's runtime
LIBRARY_CLIENT_FACTORY.get().unwrap().runtime().block_on( async move {

    // Write data to server
    let result = event_writer_ptr.flush().await.unwrap();
    let mut returnVal:u64 = 1;
    if result==()
    {
        returnVal =0;
    }
    unsafe { callback(key, returnVal); }
});
}

#[no_mangle]
pub extern "C" fn EventWriterWrite(
    event_writer_ptr: &mut EventWriter,
    buffer: *mut u8,
    buffer_size: u32,
    key: u64, 
    callback: unsafe extern "C" fn(u64, u64)
)
{
    // Initialize the buffer from the inputs
    let buffer_slice: U8Slice = U8Slice { slice_pointer: buffer as *mut i32, length: buffer_size };
    let buffer_array: &mut [u8] = buffer_slice.as_rust_u8_slice_mut();

    // Block on the client factory's runtime
    LIBRARY_CLIENT_FACTORY.get().unwrap().runtime().block_on( async move {

        // Write data to server
        let mut result: tokio::sync::oneshot::Receiver<Result<(), pravega_client::error::Error>> = event_writer_ptr.write_event(buffer_array.to_vec()).await;
        let reviever_value: () = result.try_recv().unwrap().unwrap();
        let mut return_value:u64 =1;
        if reviever_value==()
        {
            return_value =0;
        }

        unsafe { callback(key, return_value as u64); }
    });
}

#[no_mangle]
pub extern "C" fn WriteEventByRoutingKey(
    event_writer_ptr: &mut EventWriter,
    routing_key: CustomRustString,
    event: *mut u8,
    event_size: u32,
    key: u64,
    callback: unsafe extern "C" fn(u64, u64)
){
    let rs = routing_key.as_string();

    let event_slice: U8Slice = U8Slice { slice_pointer: event as *mut i32, length: event_size };
    let event_array: &mut [u8] = event_slice.as_rust_u8_slice_mut();
    let event_vector = Vec::from(event_array);//In Future update

    LIBRARY_CLIENT_FACTORY.get().unwrap().runtime().block_on(async move {
        let receiver = event_writer_ptr.write_event_by_routing_key(rs, event_vector);
        let mut result = receiver.await;
        match result.try_recv(){
            Ok(x) => {
                println!("WriteEventByRoutingKey: {:?}", x);
                unsafe { callback(key, 1) }
            },
            Err(e) => {
                println!("WriteEventByRoutingKey: {:?}", e);
                unsafe { callback(key, 0) }
            }
        }
    })
}