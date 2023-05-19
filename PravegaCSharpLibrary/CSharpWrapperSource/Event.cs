/// File: Event.cs
/// File Creator: John Sbur
/// Purpose: Contains helper structs under the event module. Implements the C# equivalent of the Rust wrapper structs

#pragma warning disable 0105
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Pravega.ClientFactoryModule;
using Pravega.Shared;
using Pravega.Utility;
using static Pravega.Interop;
#pragma warning restore 0105

namespace Pravega.Event
{

    // Continues building the Interop class by adding method signatures found in Byte.
    public static partial class Interop
    {
        // ReaderGroup constructor
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateReaderGroup")]
        internal static extern IntPtr CreateReaderGroup(IntPtr clientFactoryPointer,
            CustomRustString scope,
            CustomRustString stream,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackInvoke callback);

        // EventWriter constructor
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateEventWriter")]
        internal static extern IntPtr CreateEventWriter(IntPtr clientFactoryPointer,
            CustomRustString scope,
            CustomRustString stream,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackInvoke callback);

        // Event Writer Write Event by Routing Key
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "WriteEventByRoutingKey")]
        internal static extern IntPtr WriteEventByRoutingKey(
            IntPtr eventPointer,
            CustomRustString routingKey,
            IntPtr bufferPointer,
            uint bufferSize,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackU64Invoke callback);

        /// <summary>
        /// Drops the EventWriter.
        /// </summary>
        /// <param name="EWpointer">The event writer pointer.</param>
        /// <returns></returns>
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "EventWriterDrop")]
        internal static extern IntPtr DropEW(IntPtr EWpointer);

        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "EventWriterFlush")]
        internal static extern ulong EventWriterFlush(
            IntPtr byteWriterPointer,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackU64Invoke callback);

        /// <summary>
        /// Calls the rust dll file to write an event
        /// </summary>
        /// <param name="byteWriterPointer">The EventWriter pointer.</param>
        /// <param name="bufferPointer">The buffer pointer.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="key">The key for the delegate manager.</param>
        /// <param name="callback">The callback function pointer.</param>
        /// <returns></returns>
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "EventWriterWrite")]
        internal static extern ulong EventWriterWrite(
        IntPtr byteWriterPointer,
        IntPtr bufferPointer,
        uint bufferSize,
        ulong key,
        [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackU64Invoke callback);
    }

    /// <summary>
    ///     Wrapper for Event Writer
    /// </summary> 
    public class EventWriter : RustStructWrapper
    {

        internal EventWriter()
        {
            this.RustStructPointer = IntPtr.Zero;
        }

        internal async Task InitializeEventWriter(
            ScopedStream writerScopedStream
        )
        {
            if (ClientFactory.Initialized())
            {
                IntPtr EWPointer = await GenerateEventWriterHelper(writerScopedStream);
                this._rustStructPointer = EWPointer;
                Console.WriteLine("From C#");
                Console.WriteLine(this._rustStructPointer);
            }
            else
            {
                throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
            }
        }
        private Task<IntPtr> GenerateEventWriterHelper(
            ScopedStream writerScopedStream
        )
        {
            TaskCompletionSource<IntPtr> task = new TaskCompletionSource<IntPtr>();
            rustCallback callback = (value) => {
                task.SetResult(value);
            };
            ulong key = CallbackDelegateManager.AddToRustCallbackDict(callback);
            Interop.CreateEventWriter(
                ClientFactory.RustStructPointer,
                writerScopedStream.Scope.RustString,
                writerScopedStream.Stream.RustString,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackDict
            );
            return task.Task;
        }
        public Task<ulong> WriteRoutingKey(
            List<byte> buffer,
            String routingKey
        )
        {
            // If ClientFactory isn't initialized, throw an exception.
            if (!ClientFactory.Initialized())
            {
                throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
            }
            //CustomRustString routingKeyRust = new CustomRustString((uint)routingKey.Length);
            CustomCSharpString routingKeyCSharp = new CustomCSharpString(routingKey);
            // Create task
            TaskCompletionSource<ulong> task = new TaskCompletionSource<ulong>();

            // Split the list into local variables. 
            byte[] bufferArray = buffer.ToArray();
            uint bufferSize = (uint)bufferArray.Length;
            // Marshal the array to unmanaged memory.
            IntPtr unmanagedBufferArray = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte))
                       * (int)bufferSize);
            Marshal.Copy(bufferArray, 0, unmanagedBufferArray, (int)bufferSize);

            // Create and pin the callback so it isn't garbage collected.
            rustCallbackU64 callback = (value) => {
                task.SetResult(value);
            };
            ulong key = CallbackDelegateManager.AddToRustCallbackU64Dictionary(callback);

            // Write to stream the unmanaged buffer
            Interop.WriteEventByRoutingKey(
                this._rustStructPointer,
                routingKeyCSharp.RustString,
                unmanagedBufferArray,
                bufferSize,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackU64Dict
            );
            return task.Task;
        }

        /// <summary>
        /// Drops the event writer.
        /// </summary>
        public void DropEventWriter()
        {
            Interop.DropEW(this.RustStructPointer);
        }

        /// <summary>
        /// Flushes Data
        /// </summary>
        /// <returns>
        /// Task w/ 0 is success, 1 if failure</returns>
        /// <exception cref="Pravega.PravegaException"></exception>
        /// Error if client factory not initialized
        public Task<ulong> EventWriterFlush()
        {
            if (!ClientFactory.Initialized())
            {
                throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
            }

            // Create task
            TaskCompletionSource<ulong> task = new TaskCompletionSource<ulong>();

            // Create and pin the callback so it isn't garbage collected.
            rustCallbackU64 callback = (value) => {
                task.SetResult(value);
            };
            ulong key = CallbackDelegateManager.AddToRustCallbackU64Dictionary(callback);
            // Call bytewriter flush
            Interop.EventWriterFlush(
                this._rustStructPointer,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackU64Dict
            );
            return task.Task;
        }

        /// <summary>
        /// Writes an event.
        /// </summary>
        /// <param name="buffer">Bytes to be writtena s an event</param>
        /// <returns>
        /// Task that returns 0 is successs, 1 if failure
        /// </returns>
        /// <exception cref="Pravega.PravegaException"></exception>
        /// Exception if ClientFactory not initialized
        public Task<ulong> Write(
           List<byte> buffer
       )
        {
            // If ClientFactory isn't initialized, throw an exception.
            if (!ClientFactory.Initialized())
            {
                throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
            }

            // Create task
            TaskCompletionSource<ulong> task = new TaskCompletionSource<ulong>();

            // Split the list into local variables. 
            byte[] bufferArray = buffer.ToArray();
            uint bufferSize = (uint)bufferArray.Length;

            // Marshal the array to unmanaged memory.
            IntPtr unmanagedBufferArray = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte))
                       * (int)bufferSize);
            Marshal.Copy(bufferArray, 0, unmanagedBufferArray, (int)bufferSize);

            // Create and pin the callback so it isn't garbage collected.
            rustCallbackU64 callback = (value) => {
                task.SetResult(value);
            };
            ulong key = CallbackDelegateManager.AddToRustCallbackU64Dictionary(callback);
            // Write to stream the unmanaged buffer
            Interop.EventWriterWrite(
                this._rustStructPointer,
                unmanagedBufferArray,
                bufferSize,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackU64Dict
            );
            return task.Task;
        }
    }


    /// <summary>
    ///     Wrapper for Event Reader
    /// </summary> 
    public class EventReader : RustStructWrapper
    {

        internal EventReader(ScopedStream s)
        {
            this.RustStructPointer = IntPtr.Zero;
        }

    }




    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct SegmentMetadataClientHolder
    {
        byte inner;
    }





    /// <summary>
    ///     Wrapper for Reader Group
    /// </summary> 
    public class ReaderGroup : RustStructWrapper
    {

        internal ReaderGroup()
        {
            this._rustStructPointer = IntPtr.Zero;
        }

        internal async Task InitializeReaderGroup(
            ScopedStream writerScopedStream
        )
        {
            if (ClientFactory.Initialized())
            {
                IntPtr ReaderGroupPointer = await GenerateReaderGroupHelper(writerScopedStream);
                this._rustStructPointer = ReaderGroupPointer;
                Console.WriteLine(this._rustStructPointer);
            }
            else
            {
                throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
            }
        }
        private Task<IntPtr> GenerateReaderGroupHelper(
            ScopedStream writerScopedStream
        )
        {
            TaskCompletionSource<IntPtr> task = new TaskCompletionSource<IntPtr>();
            rustCallback callback = (value) => {
                task.SetResult(value);
            };
            ulong key = CallbackDelegateManager.AddToRustCallbackDict(callback);
            Interop.CreateReaderGroup(
                ClientFactory.RustStructPointer,
                writerScopedStream.Scope.RustString,
                writerScopedStream.Stream.RustString,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackDict
            );
            return task.Task;
        }
    }

    /// <summary>
    ///     Wrapper for ReaderGroupConfig
    /// </summary>     
    public class ReaderGroupConfig : RustStructWrapper
    {

        internal ReaderGroupConfig(ScopedStream s)
        {
            this._rustStructPointer = IntPtr.Zero;
        }
    }

}