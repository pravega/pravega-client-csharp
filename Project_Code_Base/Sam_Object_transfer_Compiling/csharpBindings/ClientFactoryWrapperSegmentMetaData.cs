// Automatically generated by Interoptopus.

#pragma warning disable 0105
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Pravega;
#pragma warning restore 0105

namespace Pravega
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct ClientFactoryAsyncWrapper
    {
        uint temp;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CustomCSharpString
    {
        public uint capacity;
        public U16Slice string_slice;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CustomCSharpStringSlice
    {
        public IntPtr slice_pointer;
        public ulong length;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CustomRustString
    {
        public uint capacity;
        public U8Slice string_slice;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CustomRustStringSlice
    {
        public IntPtr slice_pointer;
        public ulong length;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CustomU128
    {
        public ulong first_half;
        public ulong second_half;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct DelegationTokenProviderWrapper
    {
        ScopedStreamWrapper stream;
        DelegationTokenWrapper token;
        bool signal_expiry;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct DelegationTokenWrapper
    {
        CustomCSharpString value;
        Optionu64 expiry_time;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct PravegaNodeUriWrapper
    {
        public CustomCSharpString string;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct ScopeWrapper
    {
        public CustomCSharpString name;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct ScopedSegmentWrapper
    {
        public ScopeWrapper scope;
        public StreamWrapper stream;
        public SegmentWrapper segment;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct ScopedStreamWrapper
    {
        public ScopeWrapper scope;
        public StreamWrapper stream;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct SegmentMetadataClient
    {
        ScopedSegmentWrapper segment;
        ClientFactoryAsyncWrapper factory;
        DelegationTokenProviderWrapper delegation_token_provider;
        PravegaNodeUriWrapper endpoint;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct SegmentWrapper
    {
        public long number;
        public TxIdWrapper tx_id;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct StreamWrapper
    {
        public CustomCSharpString name;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct TableWrapper
    {
        CustomCSharpString name;
        PravegaNodeUriWrapper endpoint;
        ClientFactoryAsyncWrapper factory;
        DelegationTokenProviderWrapper delegation_token_provider;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct TxIdWrapper
    {
        public CustomU128 inner;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct U16Slice
    {
        public IntPtr slice_pointer;
        public ulong length;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct U8Slice
    {
        public IntPtr slice_pointer;
        public ulong length;
    }

    ///Option type containing boolean flag and maybe valid data.
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Optionu64
    {
        ///Element that is maybe valid.
        ulong t;
        ///Byte where `1` means element `t` is valid.
        byte is_some;
    }

    public partial struct Optionu64
    {
        public static Optionu64 FromNullable(ulong? nullable)
        {
            var result = new Optionu64();
            if (nullable.HasValue)
            {
                result.is_some = 1;
                result.t = nullable.Value;
            }

            return result;
        }

        public ulong? ToNullable()
        {
            return this.is_some == 1 ? this.t : (ulong?)null;
        }
    }




    public class InteropException<T> : Exception
    {
        public T Error { get; private set; }

        public InteropException(T error): base($"Something went wrong: {error}")
        {
            Error = error;
        }
    }

}
