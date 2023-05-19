///
/// File: ClientFactory.cs
/// File Creator: John Sbur
/// Purpose: Contains helper classes and methods that are used in the ClientFactory module.
///
#pragma warning disable 0105
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Pravega;
using Pravega.Byte;
using Pravega.Utility;
using Pravega.Config;
using Pravega.ControllerCli;
using Pravega.Shared;
using Pravega.Event;
using System.Runtime.CompilerServices;
#pragma warning restore 0105

// Make internals visible for testing
[assembly: InternalsVisibleTo("PravegaCSharpTestProject")]

namespace Pravega.ClientFactoryModule
{
    // Continues building the Interop class by adding method signatures found in Client Factory.
    public static partial class Interop {


        ////////
        /// Client Factory
        ////////
        
        // Client Factory default constructor (default client config, generated runtime)
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateClientFactory")]
        internal static extern IntPtr CreateClientFactory();

        // Client Factory constructor (inputted client config, generated runtime)
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateClientFactoryFromConfig")]
        internal static extern IntPtr CreateClientFactoryFromConfig(
            IntPtr clientConfigPointer
        );

        // Client Factory constructor (inputted client config, inputted runtime)
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateClientFactoryFromConfigAndRuntime")]
        internal static extern IntPtr CreateClientFactoryFromConfigAndRuntime(
            IntPtr clientConfigPointer,
            IntPtr runtimePointer
        );

        // Getters and Setters
        // ClientFactory.runtime
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetClientFactoryRuntime")]
        internal static extern IntPtr GetClientFactoryRuntime();

        // ClientFactory.runtime_handle
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetClientFactoryRuntimeHandle")]
        internal static extern IntPtr GetClientFactoryRuntimeHandle(
            IntPtr sourceClientFactory
        );

        // ClientFactory.runtime_handle
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetClientFactoryConfig")]
        internal static extern IntPtr GetClientFactoryConfig(
            IntPtr sourceClientFactory
        );

        // ClientFactory.controller_client
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetClientFactoryControllerClient")]
        internal static extern IntPtr GetClientFactoryControllerClient();
        
        // ClientFactory.to_async()
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ClientFactoryToAsync")]
        internal static extern IntPtr ClientFactoryToAsync();

        // ClientFactory testing functions
        // ClientFactory default constructor time take to complete in milliseconds in rust
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateClientFactoryTime")]
        internal static extern ulong CreateClientFactoryTime();

        // ClientFactory.new with config time take to complete in milliseconds in rust
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateClientFactoryFromConfigTime")]
        internal static extern ulong CreateClientFactoryFromConfigTime();

        // ClientFactory.new_with_runtime time take to complete in milliseconds in rust
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateClientFactoryFromConfigAndRuntimeTime")]
        internal static extern ulong CreateClientFactoryFromConfigAndRuntimeTime();
        
        // ClientFactory.runtime time take to complete in milliseconds in rust
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetClientFactoryRuntimeTime")]
        internal static extern ulong GetClientFactoryRuntimeTime();
        
        // ClientFactory.handle time take to complete in milliseconds in rust
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetClientFactoryRuntimeHandleTime")]
        internal static extern ulong GetClientFactoryRuntimeHandleTime();
        
        // ClientFactory.config time take to complete in milliseconds in rust
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetClientFactoryConfigTime")]
        internal static extern ulong GetClientFactoryConfigTime();
        
        // to_async time take to complete in milliseconds in rust
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ClientFactoryToAsyncTime")]
        internal static extern ulong ClientFactoryToAsyncTime();


        ////////
        /// 
        ////////
    }

    /// <summary>
    ///  Testing class. Measures time it takes to execute an equivalent function in Rust.
    ///     Time returned represents execution time in nanoseconds.
    ///  
    ///  Ex. C# contains ClientFactory.ToAsync()
    ///   Rust also contains a method called to_async() that is ran from
    ///   a clientfactory. The function ToAsyncTime() runs that function 
    ///   in a Rust .dll call and returns how long it took to execute.
    /// </summary>
    internal static class ClientFactoryTestMethods
    {
        /// <summary>
        ///   Return time it takes in Rust to run "to_async"
        /// </summary>
        /// <returns>
        ///  Execution time in nanoseconds
        /// </returns>
        public static float ToAsyncTime(){
            return Interop.ClientFactoryToAsyncTime();
        }

        /// <summary>
        ///  Return time it takes in Rust to run "config"
        /// </summary>
        /// <returns>
        ///  Execution time in nanoseconds
        /// </returns> 
        public static float ConfigTime()
        {
            return Interop.ClientFactoryToAsyncTime();
        }

        /// <summary>
        ///   Return time it takes in Rust to run "runtime_handle"
        /// </summary>
        /// <returns>
        ///  Execution time in nanoseconds
        /// </returns>
        public static float HandleTime(){
            return Interop.GetClientFactoryRuntimeHandleTime();
        }

        /// <summary>
        ///   Return time it takes in Rust to run "runtime"
        /// </summary>
        /// <returns>
        ///  Execution time in nanoseconds
        /// </returns>
        public static float RuntimeTime(){
            return Interop.GetClientFactoryRuntimeTime();
        }
         
        /// <summary>
        ///   Return time it takes in Rust to run the client factory constructor that takes a runtime and config
        /// </summary>
        /// <returns>
        ///  Execution time in nanoseconds
        /// </returns>
        public static float ConstructorConfigAndRuntimeTime(){
            return Interop.CreateClientFactoryFromConfigAndRuntimeTime();
        }

        /// <summary>
        ///   Return time it takes in Rust to run the client factory constructor that takes a config
        /// </summary>
        /// <returns>
        ///  Execution time in nanoseconds
        /// </returns>
        public static float ConstructorConfigTime(){
            return Interop.CreateClientFactoryFromConfigTime();
        }

        /// <summary>
        ///   Return time it takes in Rust to the the client factory default constructor
        /// </summary>
        /// <returns>
        ///  Execution time in nanoseconds
        /// </returns>
        public static float DefaultConstructorTime(){
            return Interop.CreateClientFactoryTime();
        }
    }

    /// <summary>
    ///  Applications should use ClientFactory to create resources they need.
    ///
    ///  ClientFactory contains a connection pool that is shared by all the 
    ///    readers and writers it creates. It also contains a tokio runtime that is 
    ///    used to drive async tasks. Spawned tasks in readers and writers are tied 
    ///    to this runtime. Tokio runtime represents the asynchronous execution
    ///    environement that client factory will execute asynchronous functions from.
    ///     
    /// </summary>
    public static class ClientFactory
    {
        /// <summary>
        ///  Represents a reference to this static object's Rust object counterpart.
        /// </summary>
        private static IntPtr _rustStructPointer = IntPtr.Zero;

        /// <summary>
        ///  Represents whether this client factory has been initialized or not.
        /// </summary>
        private static bool initialized = false;

        /// <summary>
        ///  Initializer. 
        ///  * Initializes the Static Client Factory only if it hasn't been initialized yet.
        ///  
        ///  - Initializes ClientFactory with a default ClientConfig and
        ///  a generated config if no ClientConfig or Runtime is inputted.
        ///  LocalHost = 9090
        /// 
        ///  Default configuration:
        ///   MaxConnectionsInPool = uint.MaxValue
        ///   MaxControllerConnections = 3
        ///   ConnectionType = Tokio
        ///   RetryPolicy = new RetryWithBackoff(); // Default constructor
        ///   TransactionTimeoutTime = 9000
        ///   Mock = false
        ///   IsTlsEnabled = (determined based on application)
        ///   IsAuthEnabled = false
        ///   RequestTimeout = (determined based on application)
        ///   
        ///   - Initializes Client Factory with a ClientConfig if no Runtime is inputted
        ///   Consumes ClientConfig (sets to null after)
        ///   
        ///   - Initializes Client Factory with a ClientConfig and Runtime if both are inputted
        ///   Consumes both ClientConfig and Runtime (sets to null after)
        ///   
        ///   - If only a Runtime is inputted, an error will be thrown as only the previous 3 cases are valid
        ///   initializers.
        ///   
        /// </summary>
        /// <param name="factoryConfig">
        ///  Config to base the client factory on.
        /// </param>
        public static void Initialize(ClientConfig? factoryConfig = null, TokioRuntime? factoryRuntime = null)
        {
            // If Client Factory is already initialized, then we don't need to reinitialize it
            //  and can safely return.
            if (ClientFactory.Initialized())
            {
                return;
            }

            if (factoryConfig == null && factoryRuntime == null)
            {
                ClientFactory._rustStructPointer = Interop.CreateClientFactory();
            }
            else if (factoryRuntime == null)
            {
                // Input pointer into constructor
                // Not possible by basic philosophy.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                ClientFactory._rustStructPointer = Interop.CreateClientFactoryFromConfig(factoryConfig.RustStructPointer);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                // Mark ClientConfig as null
                factoryConfig.MarkAsNull();
            }
            else if (factoryConfig != null)
            {
                // Input pointer into constructor
                ClientFactory._rustStructPointer = Interop.CreateClientFactoryFromConfigAndRuntime(
                    factoryConfig.RustStructPointer,
                    factoryRuntime.RustStructPointer
                );

                // Mark ClientConfig and Runtime as null
                factoryConfig.MarkAsNull();
                factoryRuntime.MarkAsNull();
            }
            else
            {
                throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
            }

            // Set initialized to true when done.
            ClientFactory.initialized = true;
        }

        // Setters and Getters
        /// <summary>
        ///  Allows internal code to get or set this object's internal pointer to a rust object.
        /// </summary>
        internal static IntPtr RustStructPointer
        {
            get { return ClientFactory._rustStructPointer; }
            set { ClientFactory._rustStructPointer = value; }
        }
        
        /// <summary>
        ///  Gets the runtime object that this clientfactory's asynchronous operations 
        ///  execute on.
        /// </summary>
        public static TokioRuntime Runtime{
            get
            {
                if (ClientFactory.initialized == false){
                    throw new PravegaException(WrapperErrorMessages.ClientFactoryNotInitialized);
                }
                else{
                    IntPtr runtimePointer = Interop.GetClientFactoryRuntime();
                    TokioRuntime runtimeObject = new TokioRuntime();
                    runtimeObject.RustStructPointer = runtimePointer;
                    
                    // debug
                    //Console.WriteLine("debug: runtime pointer = " + runtimeObject.RustStructPointer.ToString());
                    return runtimeObject;
                }
            }
        }
        
        /// <summary>
        ///  Gets the handle of the runtime object that this clientfactory's asynchronous
        ///  operations execute on.
        /// </summary>
        public static TokioHandle Handle{
            get
            {
                if (ClientFactory.initialized == false)
                {
                    throw new PravegaException(WrapperErrorMessages.ClientFactoryNotInitialized);
                }
                else
                {
                    IntPtr runtimePointer = Interop.GetClientFactoryRuntimeHandle(ClientFactory._rustStructPointer);
                    TokioHandle runtimeObject = new TokioHandle();
                    runtimeObject.RustStructPointer = runtimePointer;

                    // debug
                    //Console.WriteLine("debug: handle pointer = " + runtimeObject.RustStructPointer.ToString());
                    return runtimeObject;
                }
            }
        }
        
        /// <summary>
        /// Gets the configuration settings of this client factory.
        /// </summary>
        public static ClientConfig Config{
            get
            {
                if (ClientFactory.initialized == false)
                {
                    throw new PravegaException(WrapperErrorMessages.ClientFactoryNotInitialized);
                }
                else
                {
                    IntPtr runtimePointer = Interop.GetClientFactoryConfig(ClientFactory._rustStructPointer);
                    ClientConfig runtimeObject = new ClientConfig();
                    runtimeObject.RustStructPointer = runtimePointer;

                    // debug
                    //Console.WriteLine("debug: config pointer = " + runtimeObject.RustStructPointer.ToString());
                    return runtimeObject;
                }
            }     
        }
        
        /// <summary>
        /// Gets the controller client of this client factory. Responsible for creating streams and other fundamental operations
        /// in Pravega.
        /// </summary>
        public static ControllerClient FactoryControllerClient
        {
            get
            {
                if (ClientFactory.initialized == false)
                {
                    throw new PravegaException(WrapperErrorMessages.ClientFactoryNotInitialized);
                }
                else
                {
                    IntPtr controllerClientPointer = Interop.GetClientFactoryControllerClient();
                    ControllerClient controllerClientObject = new ControllerClient();
                    controllerClientObject.RustStructPointer = controllerClientPointer;

                    return controllerClientObject;
                }
            }
        }

        // Methods
        /// <summary>
        ///   Gets the asynchronous factory this object is tied to, clones it, and returns
        ///   it.
        /// </summary>
        /// <returns>
        ///   A clone of this object's ClientFactoryAsync.
        /// </returns>
        /// <exception cref="PravegaException">
        ///   This error is thrown if this object is set to null or uninitialized.
        /// </exception>
        public static ClientFactoryAsync ToAsync()
        {
            if (ClientFactory.initialized == false)
            {
                throw new PravegaException(WrapperErrorMessages.ClientFactoryNotInitialized);
            }
            else
            {
                IntPtr clientFactoryAsyncClone = Interop.ClientFactoryToAsync();
                ClientFactoryAsync newClientFactoryAsync = new ClientFactoryAsync();
                newClientFactoryAsync.RustStructPointer = clientFactoryAsyncClone;

                return newClientFactoryAsync;
            }
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
        public static bool Initialized()
        {
            return ClientFactory.initialized;
        }

        /// <summary>
        ///  This method marks this object as being deallocated and therefore no longer accessible.
        /// </summary>
        internal static void Destroy()
        {
            ClientFactory._rustStructPointer = IntPtr.Zero;
            ClientFactory.initialized = false;
        }

        /// <summary>
        ///  Creates a new byte writer from a ScopedStream input and uses this object's
        ///  ClientFactoryAsync.
        /// </summary>
        /// <param name="writerScopedStream">
        ///  ScopedStream to base the bytewriter on.
        /// </param>
        /// <returns>
        ///  Newly created bytewriter running on this clientfactory's runtime.
        /// </returns>
        public static async Task<ByteWriter> CreateByteWriter(ScopedStream writerScopedStream){

            if (!ClientFactory.Initialized())
            {
                throw new PravegaException(WrapperErrorMessages.ClientFactoryNotInitialized);
            }
            else
            {
                ByteWriter returnWriter = new ByteWriter();
                await returnWriter.InitializeByteWriter(writerScopedStream);
                return returnWriter;
            }

        }

        /// <summary>
        ///  Creates a new bytereader from a ScopedStream input and uses this object's
        ///  ClientFactoryAsync.
        ///  ClientFactoryAsync.
        /// </summary>
        /// <param name="readerScopedStream">
        ///  ScopedStream to base the bytereader on.
        /// </param>
        /// <returns>
        ///  Newly created bytereader running on this clientfactory's runtime.
        /// </returns>
        public static async Task<ByteReader> CreateByteReader(ScopedStream readerScopedStream)
        {
            if (!ClientFactory.Initialized())
            {
                throw new PravegaException(WrapperErrorMessages.ClientFactoryNotInitialized);
            }
            else
            {
                ByteReader returnReader = new ByteReader();
                await returnReader.InitializeByteReader(readerScopedStream);
                return returnReader;
            }
        }

        /// <summary>
        /// Creates the reader group from a scopedScream
        /// </summary>
        /// <param name="readerGroupScopedStream">The reader group scoped stream.</param>
        /// <returns>A newly initialized Readergroup</returns>
        /// <exception cref="Pravega.PravegaException"></exception>
        public static async Task<ReaderGroup> CreateReaderGroup(ScopedStream readerGroupScopedStream)
        {
            if (!ClientFactory.Initialized())
            {
                throw new PravegaException(WrapperErrorMessages.ClientFactoryNotInitialized);
            }
            else
            {
                ReaderGroup newGroup = new ReaderGroup();
                await newGroup.InitializeReaderGroup(readerGroupScopedStream);
                return newGroup;
            }
        }
        public static async Task<EventWriter> CreateEventWriter(ScopedStream readerGroupScopedStream)
        {
            if (!ClientFactory.Initialized())
            {
                throw new PravegaException(WrapperErrorMessages.ClientFactoryNotInitialized);
            }
            else
            {
                EventWriter newEW = new EventWriter();
                await newEW.InitializeEventWriter(readerGroupScopedStream);
                return newEW;
            }
        }

    }


    /// <summary>
    ///  Applications can use ClientFactoryAsync from a synchronized ClientFactory to monitor
    ///  asynchronous operations.
    /// </summary>
    public class ClientFactoryAsync : RustStructWrapper{

    }
        

}