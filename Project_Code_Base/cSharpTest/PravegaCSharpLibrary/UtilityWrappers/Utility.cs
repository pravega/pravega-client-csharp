// Automatically generated by Interoptopus.

#pragma warning disable 0105
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Pravega;
#pragma warning restore 0105

namespace Pravega.Utility
{
    /////////////////////////////////////////
    /// Abstract Class for which all Rust Structs are represented in C#.
    /////////////////////////////////////////
    public class RustStructWrapper{

        protected IntPtr _rustStructPointer;

        // Default constructor
        public RustStructWrapper(){
            this._rustStructPointer = IntPtr.Zero;
        }

        // Getter for the rust pointer
        internal IntPtr RustStructPointer{
            get{return this._rustStructPointer;}
        }

        // Internal so that only this library can set the pointer and the user cannot.
        internal void SetRustStructPointer(IntPtr newPointer){
            this._rustStructPointer = newPointer;
        }

        // Method for determining if the class's pointer was consumed in rust or uninitialized.
        public bool IsNull(){
            if(this._rustStructPointer == IntPtr.Zero) return true;
            else return false;
        }

        // Method to mark the class as consumed or null. (Rust ownership consumed the object this 
        //  class's pointer pointed to. Therefore, the pointer here is marked as null for safety)
        public void MarkAsNull(){
            this._rustStructPointer = IntPtr.Zero;
        }

        // Virtual method meant to type check
        public virtual string Type(){
            return string.Empty;
        }
    }


    // Classes from Rust Libraries that need representation in C#
    public abstract class TokioRuntime{
        public virtual string Type(){
            return "tokio.Runtime";
        }
    }

  
    // Contains pointer to rust u128 and functions for running computations with the u128.
    public class CustomU128 : RustStructWrapper
    {
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public virtual string Type(){
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
            return "u128";
        }
    }
    
    /////////////////////////////////////////
    /// Value Structs
    /////////////////////////////////////////

    /////////////////////////////////////////
    /// Slice Structs
    /////////////////////////////////////////
    /// Used to hold a slice of Rust strings (Usually a vectory or array)
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CustomRustStringSlice
    {
        public IntPtr slice_pointer;
        public uint length;
    }
    /// Used to hold a slice of Rust strings (Usually a vectory or array)
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CustomCSharpStringSlice
    {
        public IntPtr slice_pointer;
        public uint length;
    }
    ///A pointer to an array of data someone else owns which may be modified.
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct U8Slice
    {
        public IntPtr slice_pointer;
        public uint length;
    }
    public partial struct U8Slice : IEnumerable<byte>
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
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct U16Slice
    {
        public IntPtr slice_pointer;
        public uint length;
    }
    public partial struct U16Slice : IEnumerable<ushort>
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
    /*
        Demonstration on converting a string into a u16 slice

        string testString = "test";
        U16Slice test;
        test.slice_pointer = Marshal.StringToHGlobalAnsi(testString);
        test.length = (ulong)testString.Length;
        CustomCSharpString testCustomString = new CustomCSharpString();
        testCustomString.string_slice = test;
        for (ulong i = 0; i < testCustomString.string_slice.length; i++)
        {
            Console.WriteLine((char)testCustomString.string_slice[(int)i]);
        }           
    */
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct U128U64TupleSlice
    {
        public IntPtr slice_pointer;
        public uint length;
    }
    public partial struct U128U64TupleSlice : IEnumerable<U128U64TupleSlice>
    {
        public U128U64TupleSlice(GCHandle handle, uint count)
        {
            this.slice_pointer = handle.AddrOfPinnedObject();
            this.length = count;
        }
        public U128U64TupleSlice(IntPtr handle, uint count)
        {
            this.slice_pointer = handle;
            this.length = count;
        }
        public U128U64TupleSlice this[int i]
        {
            get
            {
                if (i >= Count) throw new IndexOutOfRangeException();
                var size = 200; //128+64 plus little extra
                var ptr = new IntPtr(slice_pointer.ToInt64() + i * size);
                return Marshal.PtrToStructure<U128U64TupleSlice>(ptr);
            }
            set
            {
                if (i >= Count) throw new IndexOutOfRangeException();
                var size = 200;
                var ptr = new IntPtr(slice_pointer.ToInt64() + i * size);
                Marshal.StructureToPtr<U128U64TupleSlice>(value, ptr, false);
            }
        }
        public U128U64TupleSlice[] Copied
        {
            get
            {
                var rval = new U128U64TupleSlice[length];
                for (var i = 0; i < (int)length; i++)
                {
                    rval[i] = this[i];
                }
                return rval;
            }
        }
        public int Count => (int)length;
        public IEnumerator<U128U64TupleSlice> GetEnumerator()
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
    /// Tuple Structs
    /////////////////////////////////////////
    /// <summary>
    ///     Helper struct that helps transfer tuples containing u16 values between Rust and C# as this struct is C palatable.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct U16Tuple
    {
        ushort value1;
        ushort value2;
    }



    /////////////////////////////////////////
    /// String Structs/Classes
    /////////////////////////////////////////
    /// <summary>
    ///     Helper struct that helps transfer strings between Rust and C# as this struct is C palatable. Represents a UTF-16 C# String
    /// </summary>
    public class CustomCSharpString
    {
        protected ulong capacity;
        protected U16Slice string_slice;

        // Default Constructor. Creates with string " "
        public CustomCSharpString(){
            
            // Set up slice with length equal to source's length
            CustomCSharpString newCustomCSharpString = new CustomCSharpString(" ");
            this.string_slice = newCustomCSharpString.string_slice;
            this.capacity = newCustomCSharpString.capacity;
        }

        
        // Constructor. Creates Custom CSharp string from standard string in C#.
        public CustomCSharpString(string source){
            
            // Set up a slice for the CSharp string using Marshal.
            U16Slice source_slice;
            source_slice.slice_pointer = Marshal.StringToHGlobalUni(source);
            source_slice.length = (uint)source.Length;

            // Set this object's string slice to the source_slice and capacity to being the length
            this.string_slice = source_slice;
            this.capacity = source_slice.length;
        }

        // Constructor from CustomRustString.
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

        // Constructor from CustomCSharpString.
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

        // Destructor. Destroys the string slice this points to.
        ~CustomCSharpString(){
            Marshal.FreeHGlobal(this.string_slice.slice_pointer);
        } 
    
        // Deep Clone function. Returns copy as a CustomCSharpString
        public CustomCSharpString Clone(){

            // this.NativeString generates a new managed string. The constructor from a managed string moves it into unmanaged memory, completing the deep clone.
            CustomCSharpString clonedCopy = new CustomCSharpString(this.NativeString);
            return clonedCopy;
        }

        // Setters and Getters
        public ulong Capacity{
            get{return this.capacity;}
        }

        internal U16Slice StringSlice{
            get{return this.string_slice;}
        }

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
    
        // Overrideable type function
        public virtual string Type(){
            return "Utility.CustomCSharpString";
        }
    }
    /// <summary>
    ///     Helper struct that helps transfer strings between Rust and C# as this struct is C palatable. Represents a UTF-8 Rust String
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CustomRustString
    {
        public ulong capacity;
        public U8Slice string_slice;
    }
    public partial struct CustomRustString
    {
        public CustomRustString(uint length){

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


    

    public class InteropException<T> : Exception
    {
        public T Error { get; private set; }

        public InteropException(T error): base($"Something went wrong: {error}")
        {
            Error = error;
        }
    }

}
