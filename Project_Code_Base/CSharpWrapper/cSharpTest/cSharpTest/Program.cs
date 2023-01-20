﻿
 namespace Pravega
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    public static class Program
    {
        public class Test
        {
            public int i;

            public void inc()
            {
                i++;
            }
        }

        public static void func(Test test)
        {
            test.inc();
        }

        static void Main()
        {
            Test test = new Test();
            test.i = 0;
            test.inc();
            Console.WriteLine(test.i.ToString());
            func(test);
            Console.WriteLine(test.i.ToString());


            // test creating a thing
            //IntPtr clientFactoryObject = Interop.CreateClientFactoryTest();
            //Console.WriteLine(clientFactoryObject.ToString());
            //IntPtr runtimeObject = Interop.TestGetRuntime(clientFactoryObject);
            //Console.WriteLine(runtimeObject.ToString());

        }

        private static Random random = new Random();

        /// <summary>
        ///  Creates a random string of length 'length'
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        ///  Tests the functionality of converting strings between C# and Rust as well as passing in and recieving string from Rust.
        /// </summary>
        /// <param name="testInput"></param>
        public static void StringConversionTest(string testInput)
        {
            // Showing taking a string from C# and turning it into a CustomString for sending into Rust
            Console.WriteLine(Environment.NewLine + "--Starting in C#--");
            string testString = testInput;
            //  Convert C# string into CustomC#String
            CustomCSharpString customCSharpTest = new CustomCSharpString(testString);
            Console.WriteLine("test convert from custom c#: " + customCSharpTest.ConvertToString());
            //  Convert CustomC#String into CustomRustString
            CustomRustString customRustString = customCSharpTest.ConvertToRustString();

            // Perform operations in rust
            Console.WriteLine(Environment.NewLine + "--Going into Rust--");
            CustomRustString testReturn = Interop.test(customRustString);
            Console.WriteLine(Environment.NewLine);

            // Check that the rust string can be extracted and turned back into a normal C# string.
            Console.WriteLine(Environment.NewLine + "--Coming out of Rust--");
            customCSharpTest = new CustomCSharpString(testReturn);
            Console.WriteLine("Back in C#, testing return: " + customCSharpTest.ConvertToString());
        }
    }
}