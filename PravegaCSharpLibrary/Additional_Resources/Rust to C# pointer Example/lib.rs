use std::{ffi::CString, io::Read};

use interoptopus::{ffi_function, ffi_type, Inventory, InventoryBuilder, function, patterns::primitives::FFICChar};
use libc::c_char;
use pravega_client::client_factory::ClientFactory;
use pravega_client_config::ClientConfigBuilder;
use pravega_client_shared::{
    Retention, RetentionType, ScaleType, Scaling, Scope, ScopedStream, Stream, StreamConfiguration,
};

#[no_mangle]
pub extern "C" fn CreateClientFactoryHelper() -> *const ClientFactory{

    let testClientConfig = ClientConfigBuilder::default()
            .controller_uri("localhost:8000")
            .build()
            .expect("create config");

    // Send converted client config through "new" and grab result.
    let newClientFactory: Box<ClientFactory> = Box::new(ClientFactory::new(testClientConfig));
    let returnPointer: *const ClientFactory = Box::into_raw(newClientFactory);
    println!("This is from rust");
    println!("{:?}",returnPointer);
    return returnPointer;
}

pub struct TestTuple {
    
    pub y: i64,
    pub x: i64

}

#[no_mangle]
pub extern "C" fn TestStructHelper() -> *const TestTuple{
    let holder = Box::new(TestTuple{y: 5, x:6});
    let returnpointer: *const TestTuple = Box::into_raw(holder);
    return returnpointer;


}
#[no_mangle]
pub extern "C" fn ReadTestStruct(t: &mut TestTuple)->()
{
    t.x=45;
    println!("Y is : {} X is {}",t.y,t.x);
}