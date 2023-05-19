#![allow(
    non_snake_case,
    unused_imports
)]
/// File: controller_client.rs
/// File Creator: John Sbur
/// Purpose: Contains methods transferred from the ControllerClient area. Not all methods are transferred, only necessary ones.
///     Provides definitions on the Rust side.

// Global Imports
use crate::LIBRARY_CLIENT_FACTORY;

// Crate Imports
use crate::utility::CustomRustString;

// Library Imports
use interoptopus::{Inventory, InventoryBuilder};
use pravega_controller_client::{self, ControllerClientImpl, ControllerClient};
use tokio::{runtime::{Runtime, Handle, EnterGuard}, task::JoinHandle};
use pravega_client_config::{ClientConfig, ClientConfigBuilder};
use pravega_client_shared::{Scope, ScopedStream, Stream, Scaling, StreamConfiguration, ScaleType, RetentionType, Retention};
use std::{thread, time::{self, Duration}, mem::size_of, mem, env::join_paths, ptr::null};
use pravega_client::{client_factory::{ClientFactory, self}};
use pravega_client::client_factory::ClientFactoryAsync;
use num::FromPrimitive;

// ControllerClientImpl.create_scope()
#[no_mangle]
extern "C" fn ControllerClientImplCreateScope(
    source_controller_client_impl: &mut &dyn ControllerClient, 
    source_scope: CustomRustString,
    key: u64,
    callback: unsafe extern "C" fn(u64, u64))
    -> ()
    {
        unsafe { 

            let newScope: Scope = Scope::from(source_scope.as_string());
            
            // Create the new scope asynchronously 
            LIBRARY_CLIENT_FACTORY.get().unwrap().runtime().block_on( async move {

                //println!("test create scope begin");
                source_controller_client_impl
                    .create_scope(&newScope)
                    .await
                    .expect("create scope");
                //println!("test create scope end");
                callback(key, 1);
            });        
        }
}

// ControllerClientImpl.create_stream()
#[no_mangle]
extern "C" fn ControllerClientImplCreateStream(
    source_controller_client_impl: &mut &dyn ControllerClient, 
    newStream: CustomRustString,
    targetScope: CustomRustString,
    scaling_type: i32,
    scaling_target_rate: i32,
    scaling_factor: i32,
    scaling_min_num_segments: i32,
    retention_type: i32,
    retention_param: i32,
    key: u64,
    callback: unsafe extern "C" fn(u64, u64))
    -> ()
{

    unsafe
    {
        // Initialize locals
        // StreamConfig
        //  -ScaleType
        let st = ScaleType::from_i64(scaling_type as i64);
        let st_unwrapped: ScaleType;
        if st == None{
            println!("Invalid Scale Type inputted");
            callback(key, 0);
            return;
        }
        else{
            st_unwrapped = st.unwrap();
        }
        //  -RetentionType
        let rt = RetentionType::from_i64(retention_type as i64);
        let rt_unwrapped: RetentionType;
        if rt == None{
            println!("Invalid Retention Type inputted");
            callback(key, 0);
            return;
        }
        else{
            rt_unwrapped = rt.unwrap();
        }
        //  -StreamConfig
        let stream_config: StreamConfiguration = StreamConfiguration {
            scoped_stream: ScopedStream {
                scope: Scope::from(targetScope.as_string().to_owned()),
                stream: Stream::from(newStream.as_string().to_owned()),
            },
            scaling: Scaling {
                scale_type: st_unwrapped,
                target_rate: scaling_target_rate,
                scale_factor: scaling_factor,
                min_num_segments: scaling_min_num_segments,
            },
            retention: Retention {
                retention_type: rt_unwrapped,
                retention_param: retention_param as i64,
            },
            tags: None,
        };
        // ControllerClient
        let raw_pointer: usize = source_controller_client_impl as *const &dyn ControllerClient as usize;

        // Create the new stream asynchronously
        LIBRARY_CLIENT_FACTORY.get().unwrap().runtime().block_on( async move {

            // Move a controllerclient reference into memory and clone it so we don't worry about its lifetime.
            let controller_client: &dyn ControllerClient = std::ptr::read(raw_pointer as *const &dyn ControllerClient);

            controller_client
                .create_stream(&stream_config)
                .await
                .expect("create stream");
            callback(key, 1);
        });      
    }
}