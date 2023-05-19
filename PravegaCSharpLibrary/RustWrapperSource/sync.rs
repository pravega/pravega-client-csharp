#![allow(
    non_snake_case,
    unused_imports
)]
///
/// File: lib.rs
/// File Creator: John Sbur
/// Purpose: Contains methods transferred from the ByteWriter area. Not all methods are transferred, only necessary ones.
///     Provides definitions on the Rust side.

// Global Imports
use crate::LIBRARY_CLIENT_FACTORY;

// Crate Imports

// Library Imports
use interoptopus::{Inventory, InventoryBuilder};
use pravega_client::{byte::ByteReader,byte::ByteWriter};
use pravega_client_shared::{ScopedStream};
use pravega_client::{client_factory::ClientFactory};
use tokio;
use futures::executor;
use std::time::Duration;
use tokio::runtime::Builder;
use tokio_timer::clock::Clock;



