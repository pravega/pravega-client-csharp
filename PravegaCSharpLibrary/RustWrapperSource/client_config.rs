#![allow(
    non_snake_case,
    unused_imports
)]
///
/// File: client_config.rs
/// File Creator: John Sbur
/// Purpose: Contains methods transferred from the PravegaConfig area. Not all methods are transferred, only necessary ones.
///     Provides definitions on the Rust side.
///

// Global Imports
use crate::LIBRARY_CLIENT_FACTORY;

// Crate Imports
use crate::utility::{CustomRustString};

// Library Imports
use interoptopus::{Inventory, InventoryBuilder};
use pravega_client_config::{ClientConfig, ClientConfigBuilder};
use pravega_client_retry::{retry_policy::{RetryWithBackoff}, retry_result::RetryError};
use pravega_client_shared::{PravegaNodeUri};

// Default Constructor for Client Config 
//  -Creates client config with default settings
#[no_mangle]
extern "C" fn CreateClientConfig() -> *const ClientConfig{

    // Create default ClientConfig
    let default_client_config: ClientConfig = ClientConfigBuilder::default()
        .controller_uri("localhost:8000")
        .build()
        .expect("create config");

    // Box and return
    let client_config_box: Box<ClientConfig> = Box::new(default_client_config);
    let box_pointer: *const ClientConfig = Box::into_raw(client_config_box);
    return box_pointer;
}

// Getters and Setters for ClientConfig

// ClientConfig.max_connections_in_pool
#[no_mangle]
extern "C" fn GetClientConfigMaxConnectionsInPool(source_config: &mut ClientConfig) -> u32{

    // Get Max Connections and return
    return source_config.max_connections_in_pool();
}
#[no_mangle]
extern "C" fn SetClientConfigMaxConnectionsInPool(source_config: &mut ClientConfig, new_value: u32) -> (){

    // Set Max Connections
    source_config.max_connections_in_pool = new_value;

    // Return
    return;
}

// ClientConfig.max_controller_connections
#[no_mangle]
extern "C" fn GetClientConfigMaxControllerConnections(source_config: &mut ClientConfig) -> u32{

    // Get Max Connections and return
    return source_config.max_controller_connections();
}
#[no_mangle]
extern "C" fn SetClientConfigMaxControllerConnections(source_config: &mut ClientConfig, new_value: u32) -> (){

    // Set Max Connections
    source_config.max_controller_connections = new_value;

    // Return
    return;
}

// ClientConfig.retry_policy
#[no_mangle]
extern "C" fn GetClientConfigRetryPolicy(source_config: &mut ClientConfig) -> *const RetryWithBackoff{

    // Get Retry Policy, box and return
    let config_retry_policy: RetryWithBackoff = source_config.retry_policy();

    let retry_box: Box<RetryWithBackoff> = Box::new(config_retry_policy);
    let box_pointer: *const RetryWithBackoff = Box::into_raw(retry_box);
    return box_pointer;
}
#[no_mangle]
extern "C" fn SetClientConfigRetryPolicy(source_config: &mut ClientConfig, new_policy_pointer: *const RetryWithBackoff) -> (){

    // Read retry policy, set, and return when done
    unsafe{

        let new_policy: RetryWithBackoff = std::ptr::read(new_policy_pointer);
        source_config.retry_policy = new_policy;
        return;
    }
}

// ClientConfig.controller_uri
#[no_mangle]
extern "C" fn GetClientConfigControllerUri(source_config: &mut ClientConfig) -> *const PravegaNodeUri{

    // Get clone of PravegaNodeUti, box and return
    let config_uri: PravegaNodeUri = source_config.controller_uri.clone();
    let config_uri_box: Box<PravegaNodeUri> = Box::new(config_uri);
    let box_pointer: *const PravegaNodeUri = Box::into_raw(config_uri_box);
    return box_pointer;
}
#[no_mangle]
extern "C" fn SetClientConfigControllerUri(source_config: &mut ClientConfig, new_uri_pointer: *const PravegaNodeUri) -> (){

    // read praveganodeuri, set, and return when done
    unsafe{

        let new_uri: PravegaNodeUri = std::ptr::read(new_uri_pointer);
        source_config.controller_uri = new_uri;
        return;
    }
    
}

// ClientConfig.transaction_timeout_time
#[no_mangle]
extern "C" fn GetClientConfigTransactionTimeoutTime(source_config: &mut ClientConfig) -> u64{

    // Get Transaction timeout time and return
    return source_config.transaction_timeout_time();
}
#[no_mangle]
extern "C" fn SetClientConfigTransactionTimeoutTime(source_config: &mut ClientConfig, new_value: u64) -> (){

    // Set Max Connections
    source_config.transaction_timeout_time = new_value;

    // Return
    return;
}

// ClientConfig.mock
#[no_mangle]
extern "C" fn GetClientConfigMock(source_config: &mut ClientConfig) -> u32{

    // Get bool and return 1 if true, 0 if false
    if source_config.mock() == true
    {
        return 1;
    }
    else
    {
        return 0;
    }
}
#[no_mangle]
extern "C" fn SetClientConfigMock(source_config: &mut ClientConfig, new_value: u32) -> (){

    // Set mock based on input, false if 0, true otherwise
    if new_value == 0
    {
        source_config.mock = false;
    }
    else
    {
        source_config.mock = true;
    }

    // Return
    return;
}

// ClientConfig.is_tls_enabled
#[no_mangle]
extern "C" fn GetClientConfigIsTlsEnabled(source_config: &mut ClientConfig) -> u32{

    // Get bool and return 1 if true, 0 if false
    if source_config.is_tls_enabled() == true
    {
        return 1;
    }
    else
    {
        return 0;
    }
}
#[no_mangle]
extern "C" fn SetClientConfigIsTlsEnabled(source_config: &mut ClientConfig, new_value: u32) -> (){

    // Set mock based on input, false if 0, true otherwise
    if new_value == 0
    {
        source_config.is_tls_enabled = false;
    }
    else
    {
        source_config.is_tls_enabled = true;
    }

    // Return
    return;
}

// ClientConfig.extract_trust_certs
#[no_mangle]
extern "C" fn GetClientExtractTrustCert(source_config: &mut ClientConfig, index: u32) -> CustomRustString{

    // Get Vec<str> trust certs from client config. clone
    let cloned_trust_certs: &Vec<String> = &source_config.trustcerts;

    // If index exceeds length, return "null string" sentinel inside of CustomRustString
    if index >= cloned_trust_certs.len() as u32{
        let returnString: CustomRustString = CustomRustString::from_string(String::from("null string"));
        return returnString;
    }
    else{
        let returnString: CustomRustString = CustomRustString::from_string(cloned_trust_certs[index as usize].clone());
        return returnString;
    }
}


// Used for interoptopus wrapping
pub fn my_inventory() -> Inventory {
    {
        InventoryBuilder::new()
        .inventory()
    }
}


