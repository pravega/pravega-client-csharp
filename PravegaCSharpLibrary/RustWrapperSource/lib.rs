#![allow(
    non_snake_case,
    unused_imports,
    dead_code
)]
///
/// Contains the pub mod architecture needed for using all the categorized rust wrapper files under one name.
/// Also contains globals used throughout the Rust side of the Wrapper Library.
/// 

// Library imports
use once_cell::sync::OnceCell;
use pravega_client::client_factory::ClientFactory;

// File imports
pub mod auth;
pub mod byte;
pub mod client_factory;
pub mod client_config;
pub mod connection_pool;
pub mod controller_client;
pub mod event;
pub mod index;
pub mod retry;
pub mod segment;
pub mod shared;
pub mod sync;
pub mod utility;
pub mod wire_protocol;

// Globals
const TESTING_AMOUNT: i32 = 10;
static LIBRARY_CLIENT_FACTORY: OnceCell<ClientFactory> = OnceCell::new();

fn main() -> () { 
}

