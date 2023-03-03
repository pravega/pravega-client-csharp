#pragma warning disable 0105
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
#pragma warning restore 0105

// Overarching namespace for the C# Wrapper. Contains definitions that apply to all wrappers.
namespace Pravega {


    // The static class that manages .dll function call signatures in C#. Built upon in different modules.
    // Contains globals for the C# wrapper as well
    public static partial class Interop
    {
        // String constants

        internal const string RustDllPath = @"C:\Users\john_\Desktop\Programming\Senior Project CS421\dell-pravegaapi\dell-pravegaapi\Project_Code_Base\cSharpTest\PravegaCSharpLibrary\target\debug\PravegaCSharp.dll";

        /// <summary>
        /// Delegate function used for async callbacks from rust.
        /// </summary>
        internal delegate void rustCallback(IntPtr arg);


    }

    // Error class for wrapper exceptions
    public class PravegaException : Exception
    {

        public PravegaException(string error)
            : base($"{error}.")
        {
        }
    }

    // Class containing preset error messages
    internal static class WrapperErrorMessages{

        // For when a rust object called or used cannot be found (consumed, set to null, or could not be dereferenced)
        public static string RustObjectNotFound{
            get
            {
                return "Pravega object not found exception.";
            }
        }

        // For when a function is called with Client Factory, but client factory is not initialized
        public static string ClientFactoryNotInitialized
        {
            get
            {
                return "Client Factory was not initialized, but a function requiring Client Factory to be initialized was called.";
            }
        }
    }
}