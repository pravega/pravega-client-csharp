#![allow(
    non_snake_case,
    unused_imports
)]
//// File: index.rs
/// File Creator: John Sbur
/// Purpose: Contains methods transferred from the Index area. Not all methods are transferred, only necessary ones.
///     Provides definitions on the Rust side.
///

// Global Imports
use crate::LIBRARY_CLIENT_FACTORY;

// Crate Imports

// Library Imports
use interoptopus::{Inventory, InventoryBuilder};

// Used for interoptopus wrapping
pub fn my_inventory() -> Inventory {
    {
        InventoryBuilder::new()
        .inventory()
    }
}