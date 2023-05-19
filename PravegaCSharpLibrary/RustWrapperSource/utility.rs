#![allow(
    non_snake_case,
    unused_imports
)]
///
/// File: lib.rs
/// File Creator: John Sbur
/// Purpose: Contains helper structs and methods that can transfer varying sizes of values. Slices, tuple, rust values, strings, and more
///     are located in here. Provides definitions on the Rust side.
///

// Global Imports

// Crate Imports

// Library Imports     
use interoptopus::{Inventory, InventoryBuilder, ffi_type};
use std::{slice, char};
use tokio::{runtime::{Runtime, Handle, EnterGuard}, task::JoinHandle};


/////////////////////////////////////////
/// Utility Methods
///////////////////////////////////////// 
#[no_mangle]
extern "C" fn SpawnRuntime() -> *const Runtime
{
    let runtime = Runtime::new().unwrap();
    let runtime_box: Box<Runtime> = Box::new(runtime);
    let return_pointer: *const Runtime = Box::into_raw(runtime_box) as *const Runtime;
    return return_pointer;
}

#[no_mangle]
extern "C" fn KillRuntime(target_runtime: *const Runtime) -> ()
{   
    let runtime_box: Box<Runtime> = unsafe {Box::from_raw(target_runtime as *mut Runtime)};
    let _enter: EnterGuard = runtime_box.enter();
    drop(runtime_box);
}


/////////////////////////////////////////
/// Slice Structs
/////////////////////////////////////////
#[ffi_type]
#[repr(C)]
pub struct U8Slice{
    pub slice_pointer: *mut i32,
    pub length: u32,
}
impl U8Slice{

    // Constructs a U8 slice of length "newLength"
    pub fn new(new_length: &u32) -> U8Slice {
        let mut slice_vec: Vec<u8> = Vec::with_capacity(*new_length as usize);
        U8Slice {
            length: *new_length,
            slice_pointer: slice_vec.as_mut_slice().as_mut_ptr() as *mut i32,
        }
    }

    // Convert to standard rust u8 slice
    pub fn as_rust_u8_slice_mut(&self) -> &mut [u8] {
        unsafe{
            let self_slice: &mut [u8] = slice::from_raw_parts_mut(
                self.slice_pointer as *mut u8,
                self.length as usize
            );
            return self_slice;
        }           
    }

    // Takes a rust u8 slice and converts it into this custom slice
    pub fn from_rust_u8_slice_mut(
        source: &mut [u8],
        new_length: &usize)
    -> U8Slice{
        U8Slice{
            slice_pointer: source.as_mut_ptr() as *mut i32,
            length: *new_length as u32,
        }
    }
}
#[ffi_type]
#[repr(C)]
pub struct U16Slice{
    pub slice_pointer: *mut i32,
    pub length: u32,
}
impl U16Slice{

    // Constructs a U8 slice of length "newLength"
    pub fn new(new_length: &u32) -> U16Slice {
        let mut slice_vec: Vec<u8> = Vec::with_capacity(*new_length as usize);
        U16Slice {
            length: *new_length,
            slice_pointer: slice_vec.as_mut_slice().as_mut_ptr() as *mut i32,
        }
    }

    // Convert to standard rust u16 slice
    pub fn as_rust_u16_slice_mut(&self) -> &mut [u16] {
        unsafe{
            let self_slice: &mut [u16] = slice::from_raw_parts_mut(
                self.slice_pointer as *mut u16,
                self.length as usize
            );
            return self_slice;
        }           
    }

    // Takes a rust u16 slice and converts it into this custom slice
    pub fn from_rust_u16_slice_mut(
        source: &mut [u16],
        new_length: &usize) 
    -> U16Slice{
        U16Slice{
            slice_pointer: source.as_mut_ptr() as *mut i32,
            length: *new_length as u32,
        }
    }
}


/////////////////////////////////////////
/// String Structs
/////////////////////////////////////////
// Custom string used for transferring native Rust strings to and from C#
#[ffi_type]
#[repr(C)]
pub struct CustomRustString {
    pub capacity: u32,
    pub string_slice: U8Slice,
}
// Functions related to CustomRustString
impl CustomRustString{

    // Default constructor
    pub fn new(new_length: &u32) -> CustomRustString{
        CustomRustString{
            capacity: *new_length,
            string_slice: U8Slice::new(new_length)
        }
    }

    // Construct a string from a normal rust string.
    pub fn from_string(source_string: String) -> CustomRustString{
        let string_size: usize = source_string.len() as usize;
        let mut source_string_clone: String = source_string.clone();
        println!("capacity: {0}, string_slice{1}", string_size, source_string_clone);
        unsafe{
            return CustomRustString 
            { 
                capacity: string_size as u32,
                string_slice: U8Slice::from_rust_u8_slice_mut(source_string_clone.as_mut_vec().as_mut_slice(), &string_size),
            }
        }
    }

    // Convert the contents of the object into a normal Rust string
    pub fn as_string(&self) -> String{
        return String::from_utf8_lossy(self.string_slice.as_rust_u8_slice_mut()).to_string();
    }
}

// Used for interoptopus wrapping
pub fn my_inventory() -> Inventory {
    {
        InventoryBuilder::new()
        .inventory()
    }
}