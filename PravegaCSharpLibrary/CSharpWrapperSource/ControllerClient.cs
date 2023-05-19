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
using Pravega.Utility;
using Pravega.Config;
using Pravega.Shared;
using Pravega.ClientFactoryModule;
using static Pravega.Interop;
using System.Threading.Tasks;
#pragma warning restore 0105

namespace Pravega.ControllerCli
{
    // Continues building the Interop class by adding method signatures found in Client Factory.
    public static partial class Interop
    {
        ////////
        /// Controller Client
        ////////
        // ControllerClient.create_scope()
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ControllerClientImplCreateScope")]
        internal static extern void ControllerClientImplCreateScope(
            IntPtr controllerClientPointer,
            CustomRustString newScope,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackU64Invoke callback);

        // ControllerClient.create_stream()
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ControllerClientImplCreateStream")]
        internal static extern void ControllerClientImplCreateStream(
            IntPtr controllerClientPointer,
            CustomRustString newStream,
            CustomRustString targetScope,
            int scalingType,
            int scalingTargetRate,
            int scalingFactor,
            int scalingMinNumSegments,
            int retentionType,
            int retentionParam,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackU64Invoke callback);
        
    }

    /// <summary>
    ///  Controller APIs for administrative action for streams
    /// </summary>
    public class ControllerClient : RustStructWrapper
    {
        
        /// <summary>
        ///  Default constructor for controller client. Creates with a null pointer
        /// </summary>
        internal ControllerClient(){
            this._rustStructPointer = IntPtr.Zero;
        }
        
        /// <summary>
        ///  Creates a scope within the controller client's handle with the newScope's name.
        /// </summary>
        /// <param name="newScope">
        ///  New scope to be created
        /// </param>
        /// <returns>
        ///  A task that shows when the scope has been successfully created.
        ///  Set to true when done.
        /// </returns>
        public Task<bool> CreateScope(Scope newScope)
        {
            // Verify ClientFactory is initialized. If not, throw.
            if (!ClientFactory.Initialized()){
                throw new PravegaException(WrapperErrorMessages.ClientFactoryNotInitialized);
            }

            // Create and pin the callback so it isn't garbage collected.
            TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();
            rustCallbackU64 callback = (value) => {
                task.SetResult(true);
            };
            ulong key = CallbackDelegateManager.AddToRustCallbackU64Dictionary(callback);
            Interop.ControllerClientImplCreateScope(
                this._rustStructPointer,
                newScope.RustString,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackU64Dict
            );
           
            
            return task.Task;
        }
        
        /// <summary>
        ///  Creates a scope within the controller client's handle with the newScope's name.
        /// </summary>
        /// <param name="streamConfiguration">
        ///  configuration to base the stream on
        /// </param>
        /// <returns>
        ///  A task that shows when the stream has been successfully created.
        ///  Set to true when done.
        /// </returns>
        public Task<bool> CreateStream(StreamConfiguration streamConfiguration){

            // Verify ClientFactory is initialized. If not, throw.
            if (!ClientFactory.Initialized()){
                throw new PravegaException(WrapperErrorMessages.ClientFactoryNotInitialized);
            }

            TaskCompletionSource<bool> task = new TaskCompletionSource<bool>();
            
            rustCallbackU64 callback = (value) => {
                if (value == 1)
                {
                    task.SetResult(true);
                }
                else
                {
                    task.SetResult(false);
                }
            };
            ulong key = CallbackDelegateManager.AddToRustCallbackU64Dictionary(callback);
            
            Interop.ControllerClientImplCreateStream(
                this.RustStructPointer,
                streamConfiguration.ConfigScopedStream.Stream.RustString,
                streamConfiguration.ConfigScopedStream.Scope.RustString,
                (int)streamConfiguration.ConfigScaling.Type,
                streamConfiguration.ConfigScaling.TargetRate,
                streamConfiguration.ConfigScaling.ScaleFactor,
                streamConfiguration.ConfigScaling.MinimumNumberOfSegments,
                (int)streamConfiguration.ConfigRetention.Policy,
                streamConfiguration.ConfigRetention.RetentionParameter,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackU64Dict
            );
            
            return task.Task;
        }
    }

}