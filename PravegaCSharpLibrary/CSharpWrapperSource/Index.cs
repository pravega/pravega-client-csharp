///
/// File: ClientFactory.cs
/// File Creator: John Sbur
/// Purpose: Contains helper classes and methods that are used in the Index module.
///
#pragma warning disable 0105
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using Pravega.Shared;

using System.Text;
using Pravega;

using Pravega.Utility;
using Pravega.Config;
#pragma warning restore 0105

namespace Pravega.Index
{
    public class IndexWriter : RustStructWrapper {
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public virtual string Type(){
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
            return "IndexWriter";
        }

        public IndexWriter(ScopedStream s)
        {
            this.RustStructPointer = IntPtr.Zero;
        }
    }

    public class IndexReader : RustStructWrapper{
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public virtual string Type(){
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
            return "IndexReader";
        }

        public IndexReader(ScopedStream s)
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

    
}