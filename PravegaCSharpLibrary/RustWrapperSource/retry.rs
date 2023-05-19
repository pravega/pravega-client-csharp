#![allow(
    non_snake_case,
    unused_imports
)]
/// File: retry.rs
/// File Creator: John Sbur
/// Purpose: Contains methods transferred from the Retry area. Not all methods are transferred, only necessary ones.
///     Provides definitions on the Rust side.


// Global Imports
use crate::LIBRARY_CLIENT_FACTORY;

// Crate Imports

// Library Imports
use interoptopus::{Inventory, InventoryBuilder};
use interoptopus::{ffi_type};
use pravega_client_retry::retry_policy::{RetryWithBackoff};

// Default constructor for RetryWithBackoff
#[no_mangle]
extern "C" fn CreateDefaultRetryWithBackOff() -> *const RetryWithBackoff{

    // Create default retrywithbackoff
    let new_retry_with_back_off: RetryWithBackoff = RetryWithBackoff::default();
    
    // Box and return
    let new_retry_box: Box<RetryWithBackoff> = Box::new(new_retry_with_back_off);
    let box_pointer: *const RetryWithBackoff = Box::into_raw(new_retry_box);
    return box_pointer;
}

// Used for interoptopus wrapping
pub fn my_inventory() -> Inventory {
    {
        InventoryBuilder::new()
        .inventory()
    }
}