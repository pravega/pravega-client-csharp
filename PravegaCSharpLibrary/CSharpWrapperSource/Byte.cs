///
/// File: Byte.cs
/// File Creator: John Sbur
/// Purpose: Contains helper classes and methods that are used in the Byte module.
///
#pragma warning disable 0105
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Pravega;
using Pravega.ClientFactoryModule;
using Pravega.Utility;
using Pravega.Config;
using Pravega.Index;
using Pravega.Shared;
using Pravega.Event;
using System.Runtime.CompilerServices;
using static Pravega.Interop;
using System.Drawing;
#pragma warning restore 0105

namespace Pravega.Byte
{
    // Continues building the Interop class by adding method signatures found in Byte.
    public static partial class Interop
    {

        ////////
        /// Byte Writer
        ////////
        
        // ByteWriter default constructor (default client config, generated runtime)
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateByteWriter")]
        internal static extern IntPtr CreateByteWriter(
            CustomRustString scope, 
            CustomRustString stream, 
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackInvoke callback); 
        

        // ByteWriter current offset getter
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ByteWriterCurrentOffset")]
        internal static extern ulong ByteWriterCurrentOffset(
            IntPtr byteWriterPointer
        );

        // ByteWriter.write()
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ByteWriterWrite")]
        internal static extern ulong ByteWriterWrite(
            IntPtr byteWriterPointer,
            IntPtr bufferPointer,
            uint bufferSize,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackU64Invoke callback);
        

        // ByteWriter.flush()
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ByteWriterFlush")]
        internal static extern ulong ByteWriterFlush(
            IntPtr byteWriterPointer,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackU64Invoke callback);
        

        // ByteWriter.seal()
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ByteWriterSeal")]
        internal static extern ulong ByteWriterSeal(
            IntPtr byteWriterPointer,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackU64Invoke callback);
        

        // ByteWriter.trunate_data_before
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ByteWriterTruncateDataBefore")]
        internal static extern ulong ByteWriterTruncateDataBefore(
            IntPtr byteWriterPointer,
            long offset,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackU64Invoke callback);
        

        // ByteWriter.seek_to_tail()
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ByteWriterSeekToTail")]
        internal static extern ulong ByteWriterSeekToTail(
            IntPtr byteWriterPointer,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackU64Invoke callback);
        

        // ByteWriter.reset()
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ByteWriterReset")]
        internal static extern ulong ByteWriterReset(
            IntPtr byteWriterPointer,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackU64Invoke callback);
        

        ////////
        /// Byte Reader
        ////////
        
        // ByteReader default constructor
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateByteReader")]
        internal static extern void CreateByteReader(
            CustomRustString scope,
            CustomRustString stream,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackInvoke callback
        );


        // ByteReader current offset getter
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ByteReaderCurrentOffset")]
        internal static extern ulong ByteReaderCurrentOffset(
            IntPtr byteReaderPointer
        );

        // ByteReader available getter
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ByteReaderAvailable")]
        internal static extern ulong ByteReaderAvailable(
            IntPtr byteReaderPointer
        );

        // ByteReader.seek()
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ByteReaderSeek")]
        internal static extern void ByteReaderSeek(
            IntPtr byteReaderPointer,
            ulong mode,
            long nBytes,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackU64Invoke callback
        );
        

        // ByteReader.read()
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ByteReaderRead")]
        internal static extern void ByteReaderRead(
            IntPtr byteReaderPointer,
            ulong bytesRequested,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackArrayInvoke callback
        );
        

        // ByteReader.current_head
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ByteReaderCurrentHead")]
        internal static extern void ByteReaderCurrentHead(
            IntPtr byteReaderPointer,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackU64Invoke callback
        );
        

        // ByteReader.current_tail
        [DllImport(Pravega.Interop.RustDLLPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ByteReaderCurrentTail")]
        internal static extern void ByteReaderCurrentTail(
            IntPtr byteReaderPointer,
            ulong key,
            [MarshalAs(UnmanagedType.FunctionPtr)] rustCallbackU64Invoke callback
        );
        
    }


    /// <summary>
    ///     Allows for writing raw bytes directly to a segment.
    ///     Typically created from a ClientFactory.
    /// </summary>
    public class ByteWriter : RustStructWrapper{

        // Default constructor for byte writer. Initializes with no pointer
        internal ByteWriter()
        {
            this._rustStructPointer = IntPtr.Zero;
        }

        /// <summary>
        ///   ByteWriter initializer. Creates a ByteWriter based on an inputted scopedstream.
        /// </summary>
        /// <param name="writerScopedStream">
        ///     ScopedStream to base the ByteWriter on
        /// </param>
        /// <returns>
        ///     A task that marks when this object is successfully initialized. 
        /// </returns>
        /// <exception cref="PravegaException">
        ///     Occurs if the ClientFactory isn't initialized when called.
        /// </exception>
        internal async Task InitializeByteWriter(
            ScopedStream writerScopedStream
        )
        {
            // If ClientFactory isn't initialized, throw an exception.
            if (!ClientFactory.Initialized())
            {
                throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
            }
            else
            {
                IntPtr byteWriterPointer = await GenerateByteWriterHelper(writerScopedStream);
                this._rustStructPointer = byteWriterPointer;
            }
        }

        /// <summary>
        ///  Internal method that generates a byte writer using a dll call. Sets this object's pointer when initialized successfully.
        /// </summary>
        /// <param name="writerScopedStream"></param>
        /// <returns></returns>
        private Task<IntPtr> GenerateByteWriterHelper(
            ScopedStream writerScopedStream
        )
        {
            TaskCompletionSource<IntPtr> task = new TaskCompletionSource<IntPtr>();

            // Create and pin the callback so it isn't garbage collected.
            rustCallback callback = (value) => {
                task.SetResult(value);
            };
            ulong key = CallbackDelegateManager.AddToRustCallbackDict(callback);
            Interop.CreateByteWriter(
                writerScopedStream.Scope.RustString,
                writerScopedStream.Stream.RustString,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackDict
            );
            return task.Task;
        }

        /// <summary>
        ///  Gets this object's current offset.
        /// </summary>
        public ulong CurrentOffset{
            get{
                // If ClientFactory isn't initialized, throw an exception.
                if (!ClientFactory.Initialized())
                {
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else if (!this.IsNull()){
                    return Interop.ByteWriterCurrentOffset(this.RustStructPointer);
                }
                else{
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
            }
        }

        /// <summary>
        ///  Writes the given data to the server asynchronously.
        ///  It doesn’t mean the data is persisted on the server
        ///  side when this method returns Ok, user should call
        ///  flush to ensure all data has been acknowledged by the server.
        /// </summary>
        /// <param name="buffer">
        ///  Buffer to write to the scoped stream
        /// </param>
        /// <returns>
        ///  Number of bytes written
        /// </returns>
        /// <exception cref="PravegaException">
        ///  Thrown when called and ClientFactory isn't initialized
        /// </exception>
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
            Interop.ByteWriterWrite(
                this._rustStructPointer,
                unmanagedBufferArray,
                bufferSize,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackU64Dict
            );
            return task.Task;
        }

        /// <summary>
        /// Flush data.
        ///
        /// It will wait until all pending appends have acknowledgment.
        /// </summary>
        /// <returns>
        ///  A task. Set to:
        ///  1 when successfully flushed
        ///  0 if unsuccessfully flushed
        /// </returns>
        /// <exception cref="PravegaException">
        ///  Thrown when called and ClientFactory isn't initialized
        /// </exception>
        public Task<ulong> Flush()
        {
            // If ClientFactory isn't initialized, throw an exception.
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
            Interop.ByteWriterFlush(
                this._rustStructPointer,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackU64Dict
            );
            return task.Task;
        }

        /// <summary>
        ///  Seal the segment and no further writes are allowed.
        /// </summary>
        /// <returns>
        ///  A task. Set to:
        ///  1 when complete and successful
        ///  0 if unsuccessfully sealled
        /// </returns>
        /// <exception cref="PravegaException">
        ///  Thrown when called and ClientFactory isn't initialized
        /// </exception>
        public Task<ulong> Seal()
        {
            // If ClientFactory isn't initialized, throw an exception.
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
            // Call bytewriter seal
            Interop.ByteWriterSeal(
                this._rustStructPointer,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackU64Dict
            );
            return task.Task;
        }

        /// <summary>
        ///  Truncate data before a given offset for the segment. No reads are allowed before
        ///  truncation point after calling this method.
        /// </summary>
        /// <param name="offset">
        ///  Offset to truncate before.
        /// </param>
        /// <returns>
        ///  A task. Set to:
        ///  1 when successfully truncated
        ///  0 if unsuccessfully truncated
        /// </returns>
        /// <exception cref="PravegaException">
        ///  Thrown if called when no ClientFactory is in place.
        /// </exception>        
        public Task<ulong> TruncateDataBefore(long offset)
        {
            // If ClientFactory isn't initialized, throw an exception.
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
            // Call bytewriter truncate data before
            Interop.ByteWriterTruncateDataBefore(
                this._rustStructPointer,
                offset,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackU64Dict
            );
            return task.Task;
        }

        /// <summary>
        /// Seek to the tail of the segment.
        ///
        /// This method is useful for tail reads.
        /// </summary>
        /// <returns>
        ///  A task. Result is set to 1 when complete
        /// </returns>
        /// <exception cref="PravegaException">
        ///  Thrown if called when no ClientFactory is in place.
        /// </exception>
        public Task<ulong> SeekToTail()
        {
            // If ClientFactory isn't initialized, throw an exception.
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
            // Call bytewriter seek to tail
            Interop.ByteWriterSeekToTail(
                this._rustStructPointer,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackU64Dict
            );
            return task.Task;
        }

        /// <summary>
        /// Reset the internal Reactor, making it ready for new appends.
        ///
        /// Use this method if you want to continue to append after ConditionalCheckFailure error.
        /// It will clear all pending events and set the Reactor ready.
        /// 
        /// </summary>
        /// <returns>
        ///  A task. Set to:
        ///  1 when successfully reset
        ///  0 if unsuccessfully reset
        /// </returns>
        public Task<ulong> Reset()
        {
            // If ClientFactory isn't initialized, throw an exception.
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
            // Call bytewriter reset
            Interop.ByteWriterReset(
                this._rustStructPointer,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackU64Dict
            );

            return task.Task;
        }

    }


    /// <summary>
    ///     A ByteReader enables reading raw bytes from a segment. 
    ///     Typically created from a ClientFactory.
    /// </summary>
    public class ByteReader : RustStructWrapper
    {
        /// <summary>
        ///  Default constructor for ByteReader. Initializes with this object's pointer set to zero.
        /// </summary>
        internal ByteReader()
        {
            this._rustStructPointer = IntPtr.Zero;
        }

        /// <summary>
        ///   ByteReader initializer. Creates a bytereader based on an inputted scopedstream.
        /// </summary>
        /// <param name="writerScopedStream">
        ///     ScopedStream to base the ByteReader on
        /// </param>
        /// <returns>
        ///     A task that marks when this object is successfully initialized. 
        /// </returns>
        /// <exception cref="PravegaException">
        ///     Occurs if the ClientFactory isn't initialized when called.
        /// </exception>
        internal async Task InitializeByteReader(
            ScopedStream writerScopedStream
        )
        {
            if (ClientFactory.Initialized())
            {
                IntPtr byteWriterPointer = await GenerateByteReaderHelper(writerScopedStream);
                this._rustStructPointer = byteWriterPointer;
            }
            else
            {
                throw new PravegaException(WrapperErrorMessages.ClientFactoryNotInitialized);
            }
        }

        /// <summary>
        ///  Internal method that generates a byte reader using a dll call. Sets this object's pointer when initialized successfully.
        /// </summary>
        /// <param name="writerScopedStream"></param>
        private Task<IntPtr> GenerateByteReaderHelper(
            ScopedStream writerScopedStream
        )
        {
            TaskCompletionSource<IntPtr> task = new TaskCompletionSource<IntPtr>();

            // Create and pin the callback so it isn't garbage collected.
            rustCallback callback = (value) => {
                task.SetResult(value);
            };
            ulong key = CallbackDelegateManager.AddToRustCallbackDict(callback);
            Interop.CreateByteReader(
                writerScopedStream.Scope.RustString,
                writerScopedStream.Stream.RustString,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackDict
            );

            return task.Task;
        }

        /// <summary>
        ///  Gets this object's current offset.
        /// </summary>
        public ulong CurrentOffset
        {
            get
            {
                // If ClientFactory isn't initialized, throw an exception.
                if (!ClientFactory.Initialized())
                {
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else if (!this.IsNull())
                {
                    return Interop.ByteReaderCurrentOffset(this.RustStructPointer);
                }
                else
                {
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
            }
        }

        /// <summary>
        /// Return the bytes that are available to read instantly without fetching from server.
        /// ByteReader has a buffer internally.This method returns the size of remaining data in that buffer.
        /// </summary>
        public ulong Available
        {
            get
            {
                // If ClientFactory isn't initialized, throw an exception.
                if (!ClientFactory.Initialized())
                {
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
                else if (!this.IsNull())
                {
                    return Interop.ByteReaderAvailable(this.RustStructPointer);
                }
                else
                {
                    throw new PravegaException(WrapperErrorMessages.RustObjectNotFound);
                }
            }
        }

        /// <summary>
        ///  The seek method for ByteReader allows seeking to a byte offset from the beginning
        ///     of the stream or a byte offset relative to the current position in the stream.
        ///     If the stream has been truncated, the byte offset will be relative to the original beginning of the stream.
        /// </summary>
        /// <param name="mode">
        ///     Determines which direction to seek from
        ///     0=from beginning stream
        ///     1=from current position
        ///     2=from end of stream
        ///     
        ///     *Inputting other modes not listed will result in the mode being set to 0
        /// </param>
        /// <param name="numberOfBytes">
        ///     Amount to seek
        /// </param>
        /// <returns>
        ///     A task that will eventually complete and contains the newoffset of this byte reader.
        /// </returns>
        public Task<ulong> Seek(ulong mode=0, long numberOfBytes=0)
        {
            // If Client Factory isn't initialized, throw
            if (!ClientFactory.Initialized())
            {
                throw new PravegaException(WrapperErrorMessages.ClientFactoryNotInitialized);
            }

            // If mode isn't 0, 1, or 2. Set to 0
            if (mode < 0 || mode > 2)
            {
                mode = 0;
            }

            TaskCompletionSource<ulong> task = new TaskCompletionSource<ulong>();

            // Create and pin the callback so it isn't garbage collected.
            rustCallbackU64 callback = (value) => {
                task.SetResult(value);
            };
            ulong key = CallbackDelegateManager.AddToRustCallbackU64Dictionary(callback);
            Interop.ByteReaderSeek(
                this._rustStructPointer,
                mode,
                numberOfBytes,
                key,
                CallbackDelegateManager.InvokeFromRustCallbackU64Dict
            );
            return task.Task;
        }

        /// <summary>
        ///  Reads data from the ByteReader on the current offset, filling in as much as possible up to the amount requested. Returns the buffer read afterwards
        /// </summary>
        /// <param name="numberOfBytesRequested">
        ///  Amount of bytes requested by the user. May not read the full amount, only the amount immediately available.
        /// </param>
        /// <returns>
        ///  Buffer containing bytes read
        /// </returns>
        /// <exception cref="PravegaException">
        ///     Occurs if the ClientFactory isn't initialized when called.
        /// </exception>
        public Task<byte[]> Read(ulong numberOfBytesRequested)
        {
            // If Client Factory isn't initialized, throw
            if (!ClientFactory.Initialized())
            {
                throw new PravegaException(WrapperErrorMessages.ClientFactoryNotInitialized);
            }

            TaskCompletionSource<byte[]> task = new TaskCompletionSource<byte[]>();

            // Create and pin the callback so it isn't garbage collected.
            rustCallbackArray callback = (arrPointer, numberOfBytesRead)
            =>
            {
                U8Slice buffer = new U8Slice(arrPointer, numberOfBytesRead);
                byte[] bufferManaged = buffer.Copied;
                task.SetResult(bufferManaged);
            };
            ulong key = CallbackDelegateManager.AddToRustCallbackArrayDictionary(callback);

            // Create a task and call ByteReaderRead. Await array pointer passback
            Interop.ByteReaderRead(
                this._rustStructPointer,
                numberOfBytesRequested,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackArrayDict
            );
           
            return task.Task;
        }

        /// <summary>
        ///  Return the head of current readable data in the segment asynchronously.
        ///
        ///  The ByteReader is initialized to read from the segment at offset 0. However, it might
        ///  encounter the SegmentIsTruncated error due to the segment has been truncated. In this case,
        ///  application should call this method to get the current readable head and read from it.
        /// </summary>
        /// <returns>
        ///  A ulong task. Result is set once the next readeable head has been found.
        /// </returns>
        /// <exception cref="PravegaException">
        ///     Occurs if the ClientFactory isn't initialized when called.
        /// </exception>
        public Task<ulong> CurrentHead()
        {
            // If ClientFactory isn't initialized, throw an exception.
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
            // Call bytewriter reset
            Interop.ByteReaderCurrentHead(
                this._rustStructPointer,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackU64Dict
            );
            
            return task.Task;
        }

        /// <summary>
        ///  Return the tail offset of the segment asynchronously.
        /// </summary>
        /// <returns>
        ///  A ulong task. Result is set once the tail has been found.
        /// </returns>
        /// <exception cref="PravegaException">
        ///     Occurs if the ClientFactory isn't initialized when called.
        /// </exception>
        public Task<ulong> CurrentTail()
        {
            // If ClientFactory isn't initialized, throw an exception.
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
            // Call bytewriter reset
            Interop.ByteReaderCurrentTail(
                this._rustStructPointer,
                key,
                CallbackDelegateManager.OneTimeInvokeFromRustCallbackU64Dict
            );
            return task.Task;
        }
    }


}