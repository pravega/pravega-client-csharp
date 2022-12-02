///
/// File: Utility.cs
/// File Creator: John Sbur
/// Purpose: Contains helper structs that are used throughout many different areas in the wrapper as smaller components.
///  Implements the C# equivalent of the Rust wrapper structs
///
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
    /// Value Structs
    /////////////////////////////////////////
    // U128 wrapper for sending between C# and Rust
    // NOTE: u128 normally is comprised of 1 value, but u128 is not C palatable and as such can't be transferred
    //  between C# and Rust. The solution here is to split the two halves of the u128 into two u64 values that
    //  are C palatable. 
    //  -When sent from one side to another, a u128 value is split into the two halves and bitwise ORed into the
    //      two halves of this struct.
    //  -When recieved from another wise, the first and second halves are ORed at different points on a u128 value
    //      initialized at 0. first_half -> bits 0-63 and second_half -> bits 64-127. This builds the u128 back up
    //      from its parts.
    //  There isn't an easy way to transfer a value this big between the two sides, but doing so is O(1) each time.
    //      For now, this is the fastest way to go between the two without risking using slices.
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CustomU128
    {
        public ulong first_half;
        public ulong second_half;
    }
    public partial struct CustomU128{

        // Equals
        public bool Equals(CustomU128 obj)
        {
            // equals for U128
            if (obj.first_half == this.first_half && obj.second_half == this.second_half){
                return true;
            }
            return false;
        }

        // String
        // No easy implementation. After 2 hours of tinkering, C# stores large numbers calculated as x.xxx...Ey where E represents
        //  its 10^y. Because of this, trying to parse through the number as a string for a character isn't possible as it will
        //  return the exponent y. Furthermore, no tricks like using /10 or %10 are possible since the number is too large and in 
        //  testing the rounding will only go up to a certain value less than the maximum u128 value.
    }


    /////////////////////////////////////////
    /// Slice Structs
    /////////////////////////////////////////
    /// Used to hold a slice of Rust strings (Usually a vectory or array)
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CustomRustStringSlice
    {
        public IntPtr slice_pointer;
        public ulong length;
    }
    /// Used to hold a slice of Rust strings (Usually a vectory or array)
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CustomCSharpStringSlice
    {
        public IntPtr slice_pointer;
        public ulong length;
    }
    ///A pointer to an array of data someone else owns which may be modified.
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct U8Slice
    {
        public IntPtr slice_pointer;
        public ulong length;
    }
    public partial struct U8Slice : IEnumerable<byte>
    {
        public U8Slice(GCHandle handle, ulong count)
        {
            this.slice_pointer = handle.AddrOfPinnedObject();
            this.length = count;
        }
        public U8Slice(IntPtr handle, ulong count)
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
        public ulong length;
    }
    public partial struct U16Slice : IEnumerable<ushort>
    {
        public U16Slice(GCHandle handle, ulong count)
        {
            this.slice_pointer = handle.AddrOfPinnedObject();
            this.length = count;
        }
        public U16Slice(IntPtr handle, ulong count)
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
        public ulong length;
    }
    public partial struct U128U64TupleSlice : IEnumerable<U128U64TupleSlice>
    {
        public U128U64TupleSlice(GCHandle handle, ulong count)
        {
            this.slice_pointer = handle.AddrOfPinnedObject();
            this.length = count;
        }
        public U128U64TupleSlice(IntPtr handle, ulong count)
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
    /// String Structs
    /////////////////////////////////////////
    /// <summary>
    ///     Helper struct that helps transfer strings between Rust and C# as this struct is C palatable. Represents a UTF-16 C# String
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CustomCSharpString
    {
        public ulong capacity;
        public U16Slice string_slice;
    }
    public partial struct CustomCSharpString
    {
        // Default constructor. Creates Custom CSharp string from standard string in C#
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

        // Convert back from slice into C# string
        public string ConvertToString(){

            // Add each element in the slice to a string.
            string returnString = string.Empty;
            foreach (ushort element in this.string_slice){
                returnString += ((char)element).ToString();
            }

            // Return compiled string
            return returnString;
        }

        // Convert from Custom C# string into Custom Rust string
        public CustomRustString ConvertToRustString(){

            // Check to make sure this isn't empty. If it is, return blank CustomRustString
            CustomRustString returnObject = new CustomRustString(this.string_slice.length);

            // Parse through and set array contents of utf8 to this string's contents converted
            Encoding utf16 = Encoding.Unicode;
            Encoding utf8 = Encoding.UTF8;
            byte[] source = Encoding.Unicode.GetBytes(this.ConvertToString());
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
        public CustomRustString(ulong length){

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
