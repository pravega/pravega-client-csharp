///
/// File: ClientFactory.cs
/// File Creator: John Sbur
/// Purpose: Contains helper structs that are used in the ClientFactory module.
///
#pragma warning disable 0105
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Pravega;
using Pravega.Utility;
using Pravega.Config;
#pragma warning restore 0105

namespace Pravega.ClientFactory
{
    /// Contains the class that wraps the Rust client factory struct in idea. 
    /// Design Idea: Use a struct as a wrapper between rust and C#. 
    ///     Have the wrapper struct as an object within the C# class
    ///     and use this object for function calls in the wrapper as 
    ///     well as for represeting the contents of the class.
    public class ClientFactory : RustStructWrapper
    {
        // Override type to return this class's name.
        public virtual string Type(){
            return "ClientFactory";
        }

        // Default constructor. Initializes with a default ClientConfig
        public ClientFactory(){
            this._rustStructPointer = IntPtr.Zero;
        }

        // Constructor. Initializes with a ClientConfig. Consumes ClientConfig (sets to null after)
        public ClientFactory(ClientConfig factoryConfig){
            this._rustStructPointer = IntPtr.Zero;
        }

        // Constructor. Initializes with a ClientConfig and Runtime. Consumes ClientConfig and Runtime (sets to null after)
        
    }
}