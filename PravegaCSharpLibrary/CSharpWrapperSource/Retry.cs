///
/// File: Shared.cs
/// File Creator: John Sbur
/// Purpose: Contains helper structs under the shared area. Implements the C# equivalent of the Rust wrapper structs
///
#pragma warning disable 0105
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Pravega;
using Pravega.Utility;
#pragma warning restore 0105

namespace Pravega.Retry
{
    public static partial class Interop
    {

        // Set path of Retry .dll specifically

        ////////
        /// RetryWithBackoff
        ////////
        // Default constructor
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateDefaultRetryWithBackOff")]
        internal static extern IntPtr CreateDefaultRetryWithBackOff();
        
    }

    public class RetryWithBackoff : RustStructWrapper
    {
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public virtual string Type()
        {
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
            return "RetryWithBackoff";
        }

        // Default constructor
        public RetryWithBackoff(){
            this._rustStructPointer = Interop.CreateDefaultRetryWithBackOff();
        }
    }
    /*
    Originally from pravega-client-rust/retry/src/retry_policy.rs
    as:
    pub struct RetryWithBackoff {
        attempt: usize,

        initial_delay: Duration,
        backoff_coefficient: u32,
        max_attempt: Option<usize>,
        max_delay: Option<Duration>,
        expiration_time: Option<Instant>,
    }
    */
}

    
