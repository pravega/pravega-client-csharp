#![allow(
    non_snake_case,
    unused_imports
)]
///
/// File: byte.rs
/// File Creator: John Sbur
/// Purpose: Contains methods transferred from the ByteWriter area. Not all methods are transferred, only necessary ones.
///     Provides definitions on the Rust side.
/// 

// Global Imports 
use crate::LIBRARY_CLIENT_FACTORY;

// Crate Imports
use crate::utility::CustomRustString;
use crate::utility::U8Slice;

// Library Imports
use futures::executor;
use interoptopus::{Inventory, InventoryBuilder};
use std::{thread, time};
use std::io::{Error, ErrorKind, SeekFrom};
use std::time::Duration;
use once_cell::sync::OnceCell;
use pravega_client::{byte::ByteReader,byte::ByteWriter};
use pravega_client::client_factory::{ClientFactoryAsync, ClientFactory};
use pravega_client_config::{connection_type, ClientConfig, ClientConfigBuilder};
use pravega_client_shared::{ScopedStream, Scope, Stream, StreamConfiguration, Scaling, ScaleType, Retention, RetentionType};
use tokio::runtime::{Builder, Runtime, EnterGuard};
use tokio::task;
use tokio::task::JoinHandle;
use tracing_subscriber;
use tracing;



/////////////////////
// ByteReader Methods
/////////////////////

// Create Byte Reader
#[no_mangle]
pub extern "C" fn CreateByteReader(
    scope: CustomRustString,
    stream: CustomRustString,
    key: u64,
    callback: unsafe extern "C" fn(u64, *const ByteReader))
    {   
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

        // Get the reference to the main client factory from LIBRARY_CLIENT_FACTORY.
        let factory_LIBRARY_CLIENT_FACTORY = LIBRARY_CLIENT_FACTORY.get().unwrap();

        // Create the new bytewriter asynchronously with the inputted runtime and the
        factory_LIBRARY_CLIENT_FACTORY.runtime().block_on( async move {
            let result: ByteReader = factory_LIBRARY_CLIENT_FACTORY.create_byte_reader(ss).await;
            let result_box: Box<ByteReader> = Box::new(result);
            let result_ptr: *const ByteReader = Box::into_raw(result_box);
            unsafe { callback(key, result_ptr) };
        }) ;
        
}

// ByteReader.current_tail
#[no_mangle]
pub extern "C" fn ByteReaderCurrentTail(
    source_byte_reader: &mut ByteReader,
    key: u64,  
    callback: unsafe extern "C" fn(u64, u64)
) -> ()
{
    let client_factory_ptr: &ClientFactory = LIBRARY_CLIENT_FACTORY.get().unwrap();

    // Block on the client factory's runtime
    client_factory_ptr.runtime().block_on( async move {

        // Write data to server
        let result = source_byte_reader.current_tail().await.unwrap();
        unsafe { callback(key, result); }
    });
}

// ByteReader.seek
#[no_mangle]
pub extern "C" fn ByteReaderSeek(
    source_byte_reader: &mut ByteReader,
    mode: u64,
    number_of_bytes: i64,
    key: u64, 
    callback: unsafe extern "C" fn(u64, u64))
{

    // Initialize locals
    let reader_seek_from: SeekFrom;
    if mode == 0{
        reader_seek_from = SeekFrom::Start(number_of_bytes as u64);
    }
    else if mode == 1{
        reader_seek_from = SeekFrom::Current(number_of_bytes);
    }
    else if mode == 2{
        reader_seek_from = SeekFrom::End(number_of_bytes);
    }
    else{
        panic!("Invalid seek mode inputted")
    }

    let client_factory_ptr: &ClientFactory = LIBRARY_CLIENT_FACTORY.get().unwrap();

    // Seek from the reader seek from position
    client_factory_ptr.runtime().block_on( async move {
        let result: u64 = source_byte_reader.seek(reader_seek_from).await.unwrap();
        unsafe { callback(key, result) };
    }) ;
}

// ByteReader.current_head
#[no_mangle]
pub extern "C" fn ByteReaderCurrentHead(
    source_byte_reader: &mut ByteReader, 
    key: u64,
    callback: unsafe extern "C" fn(u64, u64)
) -> ()
{
    let client_factory_ptr: &ClientFactory = LIBRARY_CLIENT_FACTORY.get().unwrap();

    // Block on the client factory's runtime
    client_factory_ptr.runtime().block_on( async move {

        // Write data to server
        let result = source_byte_reader.current_head().await.unwrap();
        unsafe { callback(key, result); }
    });
}

// ByteReader.current_offset
#[no_mangle]
pub extern "C" fn ByteReaderCurrentOffset(source_byte_reader: &mut ByteReader) -> u64
{
    return source_byte_reader.current_offset();
}

// ByteReader.available
#[no_mangle]
pub extern "C" fn ByteReaderAvailable(source_byte_reader: &mut ByteReader) -> u64
{
    return source_byte_reader.available() as u64;
}

// ByteReader.read
#[no_mangle]
pub extern "C" fn ByteReaderRead(
    source_byte_reader: &mut ByteReader, 
    bytes_requested: u32,
    key: u64,
    callback: unsafe extern "C" fn(u64, *mut i32, u32)
) -> ()
{
    // Block on and read
    LIBRARY_CLIENT_FACTORY.get().unwrap().runtime_handle().block_on(async move{

        // Create a buffer in Rust to use as storage for the read. Create at capacity
        let mut buffer: Vec<u8> = vec![0; bytes_requested as usize];

        // Read into buffer.
        let result: usize = source_byte_reader.read(&mut buffer).await.unwrap();
        
        // Box and then return.
        let buffer_slice = U8Slice::from_rust_u8_slice_mut(buffer.as_mut_slice(), &result);
        let buffer_slice_array_pointer = buffer_slice.slice_pointer;
        let _buffer_box: Box<U8Slice> = Box::new(buffer_slice);
        unsafe { callback(key, buffer_slice_array_pointer, result as u32); }
    })
}



/////////////////////
// ByteWriter Methods
/////////////////////

// Create Byte Writer
#[no_mangle]
pub extern "C" fn CreateByteWriter(
    scope: CustomRustString,
    stream: CustomRustString,
    key: u64,
    callback: unsafe extern "C" fn(u64, *const ByteWriter)){


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

        // Get the reference to the main client factory from LIBRARY_CLIENT_FACTORY.
        let factory_LIBRARY_CLIENT_FACTORY = LIBRARY_CLIENT_FACTORY.get().unwrap();

        // Create the new bytewriter asynchronously with the inputted runtime and the
        factory_LIBRARY_CLIENT_FACTORY.runtime().block_on( async move {
            let result: ByteWriter = factory_LIBRARY_CLIENT_FACTORY.create_byte_writer(ss).await;
            let result_box: Box<ByteWriter> = Box::new(result);
            let result_ptr: *const ByteWriter = Box::into_raw(result_box);
            unsafe { callback(key, result_ptr) };
        }) ;
        
}

// ByteWriter.current_offset
#[no_mangle]
pub extern "C" fn ByteWriterCurrentOffset(source_byte_writer: &mut ByteWriter) -> u64
{
    return source_byte_writer.current_offset();
}


// ByteWriter.seal
#[no_mangle]
pub extern "C" fn ByteWriterSeal(
    byte_writer_ptr: &mut ByteWriter,
    key: u64,
    callback: unsafe extern "C" fn(u64, u64)
)
{
    let client_factory_ptr = LIBRARY_CLIENT_FACTORY.get().unwrap();

    // Block on the client factory's runtime
    client_factory_ptr.runtime().block_on( async move {

        // Write data to server
        byte_writer_ptr.seal().await.unwrap();
        unsafe { callback(key, 1); }
    });
}


// ByteWriter.seek_to_tail
#[no_mangle]
pub extern "C" fn ByteWriterSeekToTail(
    byte_writer_ptr: &mut ByteWriter,
    key: u64,
    callback: unsafe extern "C" fn(u64, u64)
)
{
    let client_factory_ptr = LIBRARY_CLIENT_FACTORY.get().unwrap();

    // Block on the client factory's runtime
    client_factory_ptr.runtime().block_on( async move {

        // Write data to server
        byte_writer_ptr.seek_to_tail().await;
        unsafe { callback(key, 1); }
    });
}


// ByteWriter.write
#[no_mangle]
pub extern "C" fn ByteWriterWrite(
    byte_writer_ptr: &mut ByteWriter,
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
        let result: usize = byte_writer_ptr.write(buffer_array).await.unwrap();
        unsafe { callback(key, result as u64); }
    });
}

// ByteWriter.flush
#[no_mangle]
pub extern "C" fn ByteWriterFlush(
    byte_writer_ptr: &mut ByteWriter,
    key: u64,
    callback: unsafe extern "C" fn (u64, u64)
)
{
    // Block on the client factory's runtime
    LIBRARY_CLIENT_FACTORY.get().unwrap().runtime().block_on( async move {

        // Write data to server
        byte_writer_ptr.flush().await.unwrap();
        unsafe { callback(key, 1); }
    });
}

// ByteWriter.truncate_data_before
#[no_mangle]
pub extern "C" fn ByteWriterTruncateDataBefore(
    byte_writer_ptr: &mut ByteWriter,
    offset: i64,
    key: u64,
    callback: unsafe extern "C" fn (u64, u64)
)
{
    // Block on the client factory's runtime
    LIBRARY_CLIENT_FACTORY.get().unwrap().runtime().block_on( async move {

        // Write data to server
        byte_writer_ptr.truncate_data_before(offset).await.unwrap();
        unsafe { callback(key, 1); }
    });
}

// ByteWriter.reset
#[no_mangle]
pub extern "C" fn ByteWriterReset(
    byte_writer_ptr: &mut ByteWriter,
    key: u64,
    callback: unsafe extern "C" fn(u64, u64)
)
{
    // Block on the client factory's runtime
    LIBRARY_CLIENT_FACTORY.get().unwrap().runtime().block_on( async move {

        // Write data to server
        byte_writer_ptr.reset().await.unwrap();
        unsafe { callback(key, 1); }
    });
}