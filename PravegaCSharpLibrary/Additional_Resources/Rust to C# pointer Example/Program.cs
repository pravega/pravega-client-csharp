#pragma warning disable 0105
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#pragma warning restore 0105

namespace TransferTestCSharpSide
{
    class Program
    {
        public const string NativeLib = @"E:\CptS421\rust testing\TransferTestCSharpSide\TransferTestCSharpSide\ClientFactoryTest.dll";
        static void Main(string[] args)
        {
            
            IntPtr testptr = CreateClientFactoryHelper();
            Console.WriteLine("Hello from the C# side");
            Console.WriteLine("0x"+Convert.ToString(testptr.ToInt64(),16));

            IntPtr test2 = TestStructHelper();
            ReadTestStruct(test2);
            ReadTestStruct(test2);


        }

        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateClientFactoryHelper")]
        public static extern IntPtr CreateClientFactoryHelper();

        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TestStructHelper")]
        public static extern IntPtr TestStructHelper();

        [DllImport(NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ReadTestStruct")]
        public static extern void ReadTestStruct(IntPtr t);


    }
}
