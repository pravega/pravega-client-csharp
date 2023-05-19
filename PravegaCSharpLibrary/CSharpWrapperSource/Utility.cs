///
/// File: Utility.cs
/// File Creator: John Sbur
/// Purpose:  Contains methods and objects for helping communicate between Rust and C#.

#pragma warning disable 0105
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using Pravega;
#pragma warning restore 0105

namespace Pravega.Utility
{
    /// <summary>
    ///  Interop methods used in Utility
    /// </summary>
    public static partial class Interop
    {

        ////////
        /// Utility Methods
        ////////

        /// <summary>
        /// Create a runtime
        /// </summary>
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SpawnRuntime")]
        internal static extern IntPtr SpawnRuntime();

        /// <summary>
        /// Kill a runtime
        /// </summary>
        /// <param name="target_runtime">
        /// Pointer of Runtime to be dropped
        /// </param>
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "KillRuntime")]
        internal static extern void KillRuntime(IntPtr target_runtime);
    }

    /// <summary>
    ///    Abstract Class for most Pravega objects.
    ///    The C# library works in most part by communicating between the Pravega Rust code
    ///    and representing it in C#. A complex object can be created in Rust and stored in
    ///    unmanaged memory while keeping a reference to that object in C#. This is the 
    ///    basis of this class and with a reference to the object, we can perform typical
    ///    operations on it from C# by calling Pravega Rust code.
    /// </summary>
    public class RustStructWrapper{

        /// <summary>
        ///  Represents a reference to this object's Rust object counterpart.
        /// </summary>
        protected IntPtr _rustStructPointer;

        /// <summary>
        ///  Default constructor for RustStructWrapper. Initialized the internal pointer to be IntPtr.Zero.
        /// </summary>
        internal RustStructWrapper(){
            this._rustStructPointer = IntPtr.Zero;
        }

        /// <summary>
        ///  Allows internal code to get or set this object's internal pointer to a rust object.
        /// </summary>
        internal IntPtr RustStructPointer{
            get{return this._rustStructPointer;}
            set{this._rustStructPointer = value;}
        }

        /// <summary>
        ///  A method that determines whether this object is a null reference or not.
        ///  
        ///  Note: In rust, memory management is much different than C#. Unlike a 
        ///     garbage collector in C#, Rust manages memory by ownership. What this 
        ///     can lead to is portions of memory being deallocated after a function 
        ///     is called using an object in Rust. To represent that in C#, an object
        ///     may be set to null after being used in a function, showing that code
        ///     in Rust deleted the object that this class refers to. More information:
        ///     https://doc.rust-lang.org/book/ch04-00-understanding-ownership.html
        /// </summary>
        /// <returns>
        ///     -True if this object's reference is set to IntPtr.Zero, meaning it 
        ///         either was not initialized, or it was deallocated at some point.
        ///     -False if this object's reference is not set to IntPtr.Zero. This
        ///         likely implies that this object is still allocated in unmanaged
        ///         memory.
        /// </returns>
        public bool IsNull(){
            if(this._rustStructPointer == IntPtr.Zero) return true;
            else return false;
        }

        /// <summary>
        ///  This method marks this object as being deallocated and therefore no longer accessible.
        /// </summary>
        public void MarkAsNull(){
            this._rustStructPointer = IntPtr.Zero;
        }

        /// <summary>
        ///     This function compares this object and the inputted object, comparing their references.
        /// </summary>
        /// <param name="other">
        ///     The other Rust object you want to compare.
        /// </param>
        /// <returns>
        ///     -True if the inputted object is the same as this object.
        ///     -False if the inputted object is not the same as this object.
        /// </returns>
        public bool IsEqual(RustStructWrapper other)
        {
            if(other._rustStructPointer == this._rustStructPointer) return true; else return false;
        }
    }


    /// <summary>
    ///  This class represents a TokioRuntime object created through Rust code.
    ///     Runtime represents a thread that executions can occur on in the background
    ///     apart from C# execution. Helps utilize asynchronous operations on both the
    ///     C# side and Rust side of the code.
    ///  More information: https://tokio.rs/
    /// </summary>
    public class TokioRuntime : RustStructWrapper{

        /// <summary>
        ///  Default constructor. Creates a tokio runtime in unmanaged memory in rust.
        /// </summary>
        public TokioRuntime(){
            this._rustStructPointer = Interop.SpawnRuntime();
        }

        /// <summary>
        ///  Destructor. Kills the thread in rust this object corresponds to.
        /// </summary>
        ~TokioRuntime()
        {
            this.Drop();
        }

        /// <summary>
        ///  Kills the thread in rust this object corresponds to.
        /// </summary>
        public void Drop(){
            Interop.KillRuntime(this._rustStructPointer);
        }
    }

    /// <summary>
    ///  This class represents a TokioHandle object created through Rust code.
    ///  More information: https://tokio.rs/
    /// </summary>
    public class TokioHandle : RustStructWrapper{
    }
  
    /// <summary>
    ///  Represents a Unsigned 128bit integer.
    /// </summary>
    public class U128 : RustStructWrapper
    {
    }



    /////////////////////////////////////////
    /// Slice Structs
    /////////////////////////////////////////

    /// <summary>
    /// Used to hold a slice of Rust strings (Usually a vectory or array)
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal partial struct CustomRustStringSlice
    {
        public IntPtr slice_pointer;
        public uint length;
    }

    /// <summary>
    /// Used to hold a slice of Rust strings (Usually a vectory or array)
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal partial struct CustomCSharpStringSlice
    {
        public IntPtr slice_pointer;
        public uint length;
    }

    /// <summary>
    /// A representation of an array of data in unmanaged memory which can be modified. Array consists of bytes
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal partial struct U8Slice
    {
        public IntPtr slice_pointer;
        public uint length;
    }
    internal partial struct U8Slice : IEnumerable<byte>
    {
        public U8Slice(GCHandle handle, uint count)
        {
            this.slice_pointer = handle.AddrOfPinnedObject();
            this.length = count;
        }
        public U8Slice(IntPtr handle, uint count)
        {
            this.slice_pointer = handle;
            this.length = count;
        }
        public byte this[int i]
        {
            get
            {
                if (i >= Count) throw new IndexOutOfRangeException();
                var size = Marshal.SizeOf(typeof(byte));
                var ptr = new IntPtr(slice_pointer.ToInt64() + i * size);
                return Marshal.PtrToStructure<byte>(ptr);
            }
            set
            {
                if (i >= Count) throw new IndexOutOfRangeException();
                var size = Marshal.SizeOf(typeof(byte));
                var ptr = new IntPtr(slice_pointer.ToInt64() + i * size);
                Marshal.StructureToPtr<byte>(value, ptr, false);
            }
        }
        public byte[] Copied
        {
            get
            {
                var rval = new byte[length];
                for (var i = 0; i < (int) length; i++) {
                    rval[i] = this[i];
                }
                return rval;
            }
        }
        public int Count => (int) length;
        public IEnumerator<byte> GetEnumerator()
        {
            for (var i = 0; i < (int)length; ++i)
            {
                yield return this[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// A representation of an array of data in unmanaged memory which can be modified. Array consists of 16bit objects.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal partial struct U16Slice
    {
        public IntPtr slice_pointer;
        public uint length;
    }
    internal partial struct U16Slice : IEnumerable<ushort>
    {
        public U16Slice(GCHandle handle, uint count)
        {
            this.slice_pointer = handle.AddrOfPinnedObject();
            this.length = count;
        }
        public U16Slice(IntPtr handle, uint count)
        {
            this.slice_pointer = handle;
            this.length = count;
        }
        public ushort this[int i]
        {
            get
            {
                if (i >= Count) throw new IndexOutOfRangeException();
                var size = Marshal.SizeOf(typeof(ushort));
                var ptr = new IntPtr(slice_pointer.ToInt64() + i * size);
                return Marshal.PtrToStructure<byte>(ptr);
            }
            set
            {
                if (i >= Count) throw new IndexOutOfRangeException();
                var size = Marshal.SizeOf(typeof(ushort));
                var ptr = new IntPtr(slice_pointer.ToInt64() + i * size);
                Marshal.StructureToPtr<ushort>(value, ptr, false);
            }
        }
        public ushort[] Copied
        {
            get
            {
                var rval = new ushort[length];
                for (var i = 0; i < (int)length; i++)
                {
                    rval[i] = this[i];
                }
                return rval;
            }
        }
        public int Count => (int)length;
        public IEnumerator<ushort> GetEnumerator()
        {
            for (var i = 0; i < (int)length; ++i)
            {
                yield return this[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }


    /////////////////////////////////////////
    /// String Structs/Classes
    /////////////////////////////////////////

    /// <summary>
    ///     A class that represents an C# string in unmanaged memory.
    ///     Used for sending strings from C# into Pravega Rust code.
    ///     
    ///     Special Note: Can never store an empty string.
    /// </summary>
    public class CustomCSharpString
    {
        /// <summary>
        ///  Represents the max length of this string.
        /// </summary>
        protected ulong capacity;

        /// <summary>
        ///  Represents the reference to the unmanaged C# string.
        /// </summary>
        internal U16Slice string_slice;

        /// <summary>
        ///  Default constructor. Creates an unmanaged string containing 
        ///     only ' '. 
        /// </summary>
        public CustomCSharpString(){
            
            // Set up slice with length equal to source's length
            CustomCSharpString newCustomCSharpString = new CustomCSharpString(" ");
            this.string_slice = newCustomCSharpString.string_slice;
            this.capacity = newCustomCSharpString.capacity;
        }

        /// <summary>
        ///  Constructor. Creates an unmanaged string from an inputted C# string.
        /// </summary>
        /// <param name="source">
        ///  Managed string that this object will be created from.
        /// </param>
        public CustomCSharpString(string source){
            
            // Since this can't handle empty strings, always make sure there is a " " assigned at least at all times.
            if (source == string.Empty)
            {
                source = " ";
            }

            // Clone the source string so as to not rip the reference from managed memory.
            string sourceClone = (string)source.Clone();
            
            // Set up a slice for the CSharp string using Marshal.
            U16Slice source_slice;
            source_slice.slice_pointer = Marshal.StringToHGlobalUni(sourceClone);
            source_slice.length = (uint)sourceClone.Length;

            // Set this object's string slice to the source_slice and capacity to being the length
            this.string_slice = source_slice;
            this.capacity = source_slice.length;
        }

        /// <summary>
        ///  Constructor. Creates an unmanaged string from an inputted Rust string. 
        /// </summary>
        /// <param name="source">
        ///  Unmanaged Rust string that this object will be created from.
        /// </param>
        public CustomCSharpString(CustomRustString source){

            // Grab all the bytes of the source
            byte[] source_utf8_bytes = new byte[source.string_slice.length];
            int i = 0;
            foreach (byte utf8Char in source.string_slice){
                source_utf8_bytes[i] = utf8Char;
                i++;
            }

            // Convert utf8 byte array into utf16 byte array
            byte[] unicode_bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, source_utf8_bytes);

            // Convert utf16 byte array into a string
            string translated_utf16_string = Encoding.Unicode.GetString(unicode_bytes);

            // Convert this string into all the pieces necessary for a customCSharpString
            CustomCSharpString new_custom = new CustomCSharpString(translated_utf16_string);
            this.capacity = new_custom.capacity;
            this.string_slice = new_custom.string_slice;
        }

        /// <summary>
        ///  Constructor. Creates an unmanaged C# string cloned from an inputted unmanaged C# string.
        /// </summary>
        /// <param name="source">
        ///  Unmanaged C# string to be cloned from.
        /// </param>
        public CustomCSharpString(CustomCSharpString source){
            
            // Verify source isn't empty
            if (source.string_slice.length == 0){

                // Set up slice with length equal to source's length
                CustomCSharpString newCustomCSharpString = new CustomCSharpString(" ");
                this.string_slice = newCustomCSharpString.string_slice;
                this.capacity = newCustomCSharpString.capacity;
            }
            else{

                // Set up slice with length equal to source's length
                string copiedString = source.NativeString;
                CustomCSharpString newCustomCSharpString = new CustomCSharpString(copiedString);
                this.string_slice = newCustomCSharpString.string_slice;
                this.capacity = newCustomCSharpString.capacity;
            }
        }

        /// <summary>
        ///  Destructor. Frees the memory that this object points to.
        /// </summary>
        ~CustomCSharpString(){
            Marshal.FreeHGlobal(this.string_slice.slice_pointer);
            this.string_slice.slice_pointer = IntPtr.Zero;
            this.string_slice.length = 0;
            this.capacity = 0;
        } 
    
        /// <summary>
        ///  Performs a deep clone on this object's unmanaged memory. Stores the clone in
        ///  a new CustomCSharpString.
        /// </summary>
        /// <returns>
        ///  A CustomCSharpString object containing a reference to the cloned memory.
        /// </returns>
        public CustomCSharpString Clone(){

            // this.NativeString generates a new managed string. The constructor from a managed string moves it into unmanaged memory, completing the deep clone.
            CustomCSharpString clonedCopy = new CustomCSharpString(this.NativeString);
            return clonedCopy;
        }

        // Setters and Getters
        /// <summary>
        ///  Gets the capacity of this string. Represents the max length.
        /// </summary>
        public ulong Capacity{
            get{return this.capacity;}
        }

        /// <summary>
        ///  Gets the internal slice that represents this string.
        /// </summary>
        internal U16Slice StringSlice{
            get{return this.string_slice;}
        }

        /// <summary>
        ///  Gets this object in the form of a standard C# string.
        ///  Sets this object based on a standard C# string.
        /// </summary>
        public string NativeString{
            get
            {
                // Add each element in the slice to a string.
                string returnString = string.Empty;
                foreach (ushort element in this.string_slice){
                    returnString += ((char)element).ToString();
                }

                // Return compiled string
                return returnString;
            }
            set
            {
                // Free current slice containing string
                Marshal.FreeHGlobal(this.string_slice.slice_pointer);

                // Create a new string based on the input
                CustomCSharpString newString = new CustomCSharpString(value);

                // Retrieve values from the new string and assign them to this object.
                this.string_slice = newString.StringSlice;
                this.capacity = newString.Capacity;
            }
        }

        /// <summary>
        ///  Gets this object in the form of a UTF-8 Rust string.
        ///  Sets this object based on a UTF-8 Rust string.
        /// </summary>
        public CustomRustString RustString{
            get
            {
                // Check to make sure this isn't empty. If it is, return blank CustomRustString
                CustomRustString returnObject = new CustomRustString(this.string_slice.length);

                // Parse through and set array contents of utf8 to this string's contents converted
                Encoding utf16 = Encoding.Unicode;
                Encoding utf8 = Encoding.UTF8;
                byte[] source = Encoding.Unicode.GetBytes(this.NativeString);
                byte[] translated = Encoding.Convert
                (
                    utf16,
                    utf8,
                    source
                );

                // Assign to utf8 rust string and return afterwards
                int i = 0;
                foreach (byte utf8Char in translated){
                    returnObject.string_slice[i] = utf8Char;
                    i++;
                }
                return returnObject;
            }
            set
            {
                // Free current slice containing string
                Marshal.FreeHGlobal(this.string_slice.slice_pointer);

                // Create a new string based on the input
                CustomCSharpString newString = new CustomCSharpString(value);

                // Retrieve values from the new string and assign them to this object.
                this.string_slice = newString.StringSlice;
                this.capacity = newString.Capacity;
            }
        }
    
    }

    /// <summary>
    ///     Helper struct that helps transfer strings between Rust and C# as this struct is C palatable.
    ///     Represents a UTF-8 Rust String.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CustomRustString
    {
        internal ulong capacity;
        internal U8Slice string_slice;
    }
    public partial struct CustomRustString
    {
        internal CustomRustString(uint length){

            // Create an empty array of the requested length
            byte[] byteArray = new byte[length];

            // Make a pointer to said array
            GCHandle pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();

            // Set own intptr to array pointer
            this.string_slice.slice_pointer = pointer;
            this.string_slice.length = length;
            this.capacity = length;
        }
    }

}
