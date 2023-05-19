///
/// File: Config.cs
/// File Creator: John Sbur
/// Purpose: Contains helper structs under the config area. Implements the C# equivalent of the Rust wrapper structs
///
#pragma warning disable 0105
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Numerics;
using System.Runtime.InteropServices;
using Pravega;
using Pravega.Auth;
using Pravega.Retry;
using Pravega.Shared;
using Pravega.Utility;
using Pravega.Retry;

#pragma warning restore 0105

namespace Pravega.Config
{
     // Continues building the Interop class by adding method signatures found in the Config para-module.
    public static partial class Interop {

        ////////
        /// Client Config
        ////////
        // Default constructor
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateClientConfig")]
        internal static extern IntPtr CreateClientConfig();

        // Getters and Setters
        // MaxConnectionsInPool
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetClientConfigMaxConnectionsInPool")]
        internal static extern uint GetClientConfigMaxConnectionsInPool(IntPtr sourceClientConfig);
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetClientConfigMaxConnectionsInPool")]
        internal static extern void SetClientConfigMaxConnectionsInPool(IntPtr sourceClientConfig, uint newValue);

        // MaxControllerConnections
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetClientConfigMaxControllerConnections")]
        internal static extern uint GetClientConfigMaxControllerConnections(IntPtr sourceClientConfig);
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetClientConfigMaxControllerConnections")]
        internal static extern void SetClientConfigMaxControllerConnections(IntPtr sourceClientConfig, uint newValue);

        // RetryPolicy
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetClientConfigRetryPolicy")]
        internal static extern IntPtr GetClientConfigRetryPolicy(IntPtr sourceClientConfig);
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetClientConfigRetryPolicy")]
        internal static extern void SetClientConfigRetryPolicy(IntPtr sourceClientConfig, IntPtr newPolicy);

        // ControllerUri
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetClientConfigControllerUri")]
        internal static extern IntPtr GetClientConfigControllerUri(IntPtr sourceClientConfig);
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetClientConfigControllerUri")]
        internal static extern void SetClientConfigControllerUri(IntPtr sourceClientConfig, IntPtr newUri);

        // TransactionTimeoutTime
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetClientConfigTransactionTimeoutTime")]
        internal static extern ulong GetClientConfigTransactionTimeoutTime(IntPtr sourceClientConfig);
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetClientConfigTransactionTimeoutTime")]
        internal static extern void SetClientConfigTransactionTimeoutTime(IntPtr sourceClientConfig, ulong newValue);
        
        // Mock
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetClientConfigMock")]
        internal static extern uint GetClientConfigMock(IntPtr sourceClientConfig);
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetClientConfigMock")]
        internal static extern void SetClientConfigMock(IntPtr sourceClientConfig, uint newValue);

        // IsTlsEnabled
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetClientConfigIsTlsEnabled")]
        internal static extern uint GetClientConfigIsTlsEnabled(IntPtr sourceClientConfig);
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetClientConfigIsTlsEnabled")]
        internal static extern void SetClientConfigIsTlsEnabled(IntPtr sourceClientConfig, uint newValue);

    }

    /// <summary>
    ///  This class contains configuration that is passed on to Pravega client.
    /// </summary>
    public class ClientConfig : RustStructWrapper{

        /// <summary>
        ///    Default Constructor. Initializes with default Pravega Client Config
        ///    
        ///     Default configuration:
        ///     MaxConnectionsInPool = uint.MaxValue
        ///     MaxControllerConnections = 3
        ///     ConnectionType = Tokio
        ///     RetryPolicy = new RetryWithBackoff(); // Default constructor
        ///     TransactionTimeoutTime = 9000
        ///     Mock = false
        ///     IsTlsEnabled = (determined based on application)
        ///     IsAuthEnabled = false
        ///     RequestTimeout = (determined based on application)
        //      https://github.com/pravega/pravega-client-rust/blob/master/config/src/lib.rs
        /// </summary>
        public ClientConfig(){
            this._rustStructPointer = Interop.CreateClientConfig();      
        }

        // Setters and Getters
        /// <summary>
        ///  Sets or Gets the max number of connections the configuration supports
        /// </summary>
        public uint MaxConnectionsInPool{
            get{
                if (this.IsNull()){
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else{
                    return Interop.GetClientConfigMaxConnectionsInPool(this._rustStructPointer);
                }
            }
            set{
                if (this.IsNull()){
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else{
                    Interop.SetClientConfigMaxConnectionsInPool(this._rustStructPointer, value);
                }
            }
        }
        /// <summary>
        ///  Sets or Gets the max number of controllers the configuration allows to be connected
        /// </summary>
        public uint MaxControllerConnections{
            get{
                if (this.IsNull()){
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else{
                    return Interop.GetClientConfigMaxControllerConnections(this._rustStructPointer);
                }
            }
            set{
                if (this.IsNull()){
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else{
                    Interop.SetClientConfigMaxControllerConnections(this._rustStructPointer, value);
                }
            }
        } 
        /// <summary>
        ///  Sets or Gets this configuration's retry policy
        /// </summary>
        public RetryWithBackoff RetryPolicy{
            get{
                if (this.IsNull()){
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else{
                    RetryWithBackoff newPolicy = new RetryWithBackoff();
                    newPolicy.RustStructPointer = Interop.GetClientConfigRetryPolicy(this._rustStructPointer);
                    return newPolicy;
                }
            }
            set{
                if (this.IsNull() || value.IsNull()){
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else{
                    Interop.SetClientConfigRetryPolicy(this._rustStructPointer, value.RustStructPointer);
                }
            }
        }
        /// <summary>
        ///  Sets or Gets this configuration's controller uri
        /// </summary>
        public PravegaNodeUri ControllerUri{
            get{
                if (this.IsNull()){
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else{
                    PravegaNodeUri newUri = new PravegaNodeUri();
                    newUri.RustStructPointer = Interop.GetClientConfigRetryPolicy(this._rustStructPointer);
                    return newUri;
                }
            }
            set{
                if (this.IsNull() || value.IsNull()){
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else{
                    Interop.SetClientConfigRetryPolicy(this._rustStructPointer, value.RustStructPointer);
                }
            }
        }
        /// <summary>
        ///  Sets or Gets this configuration's transaction timeout time
        /// </summary>
        public ulong TransactionTimeoutTime{
            get{
                if (this.IsNull()){
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else{
                    return Interop.GetClientConfigTransactionTimeoutTime(this._rustStructPointer);
                }
            }
            set{
                if (this.IsNull()){
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else{
                    Interop.SetClientConfigTransactionTimeoutTime(this._rustStructPointer, value);
                }
            }
        }
        /// <summary>
        ///  Sets or Gets a bool representing whether this configuration is a mock or not.
        /// </summary>
        public bool Mock{
            get{
                if (this.IsNull()){
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else{
                    if( Interop.GetClientConfigMock(this._rustStructPointer) == 0)
                    {
                        return false;
                    }
                    else{
                        return true;
                    }
                }
            }
            set{
                if (this.IsNull()){
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else{
                    if (value == true){
                        Interop.SetClientConfigMock(this._rustStructPointer, 1);
                    }
                    else{
                        Interop.SetClientConfigMock(this._rustStructPointer, 0);
                    }
                }
            }
        }
        /// <summary>
        ///  Sets or Gets a bool representing whether this configuration is tlsenabled or not
        /// </summary>
        public bool IsTlsEnabled{
            get{
                if (this.IsNull()){
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else{
                    if( Interop.GetClientConfigIsTlsEnabled(this._rustStructPointer) == 0)
                    {
                        return false;
                    }
                    else{
                        return true;
                    }
                }
            }
            set{
                if (this.IsNull()){
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else{
                    if (value == true){
                        Interop.SetClientConfigIsTlsEnabled(this._rustStructPointer, 1);
                    }
                    else{
                        Interop.SetClientConfigIsTlsEnabled(this._rustStructPointer, 0);
                    }
                }
            }
        }
    }
}