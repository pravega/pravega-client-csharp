///
/// File: Shared.cs
/// File Creator: John Sbur
/// Purpose: Contains helper structs under the shared area. Implements the C# equivalent of the Rust wrapper structs
///
#pragma warning disable 0105
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Pravega;
using Pravega.Utility;
#pragma warning restore 0105

namespace Pravega.Shared
{
    //  ***** Wrapper for TxId *****
    public class TxId : U128
    {
    }

    /// <summary>
    ///  Represents the type of scaling for data streams
    /// </summary>
    public enum ScalingType
    {
        FixedNumSegments = 0,
        ByRateInKbytesPerSec = 1,
        ByRateInEventsPerSec = 2
    }

    /// <summary>
    ///  Unlike systems with static partitioning, Pravega can automatically scale 
    ///     individual data streams to accommodate changes in data ingestion rate.
    /// </summary>
    public class Scaling
    {
        private ScalingType type;
        private int targetRate;
        private int scaleFactor;
        private int minimumNumberOfSegments;

        /// <summary>
        ///  Default constructor. Initializes all values to 0 escept minimumNumberOfSegments which is set to 1.
        /// </summary>
        public Scaling()
        {
            this.type = ScalingType.FixedNumSegments;
            this.targetRate = 0;
            this.scaleFactor = 0;
            this.minimumNumberOfSegments = 1;
        }

        /// <summary>
        ///  Constructor. Takes minimum of the scaling type as an input and others optionally.
        /// </summary>
        /// <param name="type">
        ///     Scaling type.
        /// </param>
        /// <param name="targetRate">
        ///     Target rate to scale.
        /// </param>
        /// <param name="scaleFactor">
        ///     How much to scale.
        /// </param>
        /// <param name="minimumNumberOfSegments">
        ///     Minimum number of segments for this to be applied.
        /// </param>
        public Scaling(ScalingType type, int targetRate=0, int scaleFactor=0, int minimumNumberOfSegments=0)
        {
            this.type = type;
            this.targetRate = targetRate;
            this.scaleFactor = scaleFactor;
            this.minimumNumberOfSegments = minimumNumberOfSegments;
        }

        // Setters and Getters
        /// <summary>
        ///  Gets or Sets type
        /// </summary>
        public ScalingType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
        /// <summary>
        ///  Gets or Sets target rate
        /// </summary>
        public int TargetRate
        {
            get { return this.targetRate; }
            set { this.targetRate = value; }
        }
        /// <summary>
        ///  Gets or Sets scale factor
        /// </summary>
        public int ScaleFactor
        {
            get { return this.scaleFactor; }
            set { this.scaleFactor = value; }
        }
        /// <summary>
        ///  Gets or Sets minimum number of segments to apply the scaling policy to
        /// </summary>
        public int MinimumNumberOfSegments
        {
            get { return this.minimumNumberOfSegments; }
            set { this.minimumNumberOfSegments = value; }
        }
    }

    /// <summary>
    ///  Represents the type of retention in place for a client controller.
    /// </summary>
    public enum RetentionType
    {
        None = 0, Time = 1, Size = 2
    }

    /// <summary>
    /// The retention policy for a Pravega Stream specifies whether truncation happens based on size or time. 
    ///     It contains a minimum limit (min limit) and a maximum limit (max limit) such that the Controller 
    ///     truncates data respecting those limits. For example, a size-based policy specifies the amount of data 
    ///     to retain, so if a retention policy specifies a minimum limit = 20 GB and a maximum limit = 100 GB, 
    ///     then the truncation cycle ensures that the Stream has at least 20 GB and not more than 100 GB.
    /// </summary>
    public class Retention
    {
        private RetentionType policy;
        private int retentionParameter;

        /// <summary>
        ///  Constructor for retention. Takes a policy and amount corresponding to that policy as an input
        /// </summary>
        /// <param name="policy">
        ///     Whether retention is time based or size based
        /// </param>
        /// <param name="retentionParameter">
        ///     The amount that the retention type should keep track of.
        /// </param>
        public Retention(RetentionType policy, int retentionParameter)
        {
            this.policy = policy;
            this.retentionParameter = retentionParameter;
        }

        /// <summary>
        ///  Default constructor for retention. Sets policy to none and parameter to 0
        /// </summary>
        public Retention()
        {
            this.policy = RetentionType.None;
            this.retentionParameter = 0;
        }

        // Setters and getters
        /// <summary>
        ///  Gets or Sets the retention policy for this object.
        /// </summary>
        public RetentionType Policy
        {
            get { return this.policy; }
            set { this.policy = value; }
        }
        /// <summary>
        ///  Gets or Sets the retention parameter for this object.
        /// </summary>
        public int RetentionParameter
        {
            get { return this.retentionParameter; }
            set { this.retentionParameter = value;}
        }
    }

    /// <summary>
    ///  The configuration of a Stream.
    /// </summary>
    public class StreamConfiguration
    {
        
        private ScopedStream configScopedStream;
        private Retention configRetention;
        private Scaling configScaling;
        private List<string>? tags;

        /// <summary>
        ///  Default constructor. Initializes with null pointers.
        /// </summary>
        public StreamConfiguration()
        {
            this.configScopedStream = new ScopedStream();
            this.configRetention = new Retention();
            this.configScaling = new Scaling();
            this.tags = new List<string>();
        }

        /// <summary>
        ///  Constructor. Builds StreamConfiguration from inputs with tags being optional.
        /// </summary>
        /// <param name="newSS">
        ///     ScopedStream to build the configuration from.
        /// </param>
        /// <param name="newRetention">
        ///     Retention policy of the StreamConfiguration
        /// </param>
        /// <param name="newScaling">
        ///     Scaling policy of the StreamConfiguration
        /// </param>
        /// <param name="newTags">
        ///     Optional tags of the StreamConfiguration
        /// </param>
        public StreamConfiguration(ScopedStream newSS, Retention newRetention, Scaling newScaling, List<string>? newTags = null)
        {
            this.configScopedStream = newSS;
            this.configRetention = newRetention;
            this.configScaling = newScaling;
            this.tags = newTags;
        }

        // Setters and Getters
        /// <summary>
        ///  Gets or Sets this object's scoped stream
        /// </summary>
        public ScopedStream ConfigScopedStream
        {
            get { return this.configScopedStream; }
            set { this.configScopedStream = value;}
        }
        /// <summary>
        ///  Gets or Sets this object's retention policy
        /// </summary>
        public Retention ConfigRetention
        {
            get { return this.configRetention; }
            set { this.configRetention = value;}
        }
        /// <summary>
        ///  Gets or Sets this object's scaling policy
        /// </summary>
        public Scaling ConfigScaling
        {
            get { return this.configScaling; }
            set { this.configScaling = value; }
        }
        /// <summary>
        ///  Gets or Sets this object's tags
        /// </summary>
        public List<string>? Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }
    }


    //  ***** Wrapper for DelegationToken *****
    public class DelegationToken : RustStructWrapper{
    }


    //  ***** Wrapper for Scope *****
    public class Scope : CustomCSharpString{
    }
    

    //  ***** Wrapper for Stream *****
    public class Stream : CustomCSharpString
    {
    }


    //  ***** Wrapper for ScopedStream *****
    public class ScopedStream
    {
        private Scope _scope;
        private Stream _stream;

        // Default Constructor
        public ScopedStream(){
            this._scope = new Scope();
            this._stream = new Stream();
        }

        // Setters and Getters
        public CustomCSharpString Scope{
            get{return (CustomCSharpString)this._scope;}
            set{this._scope.NativeString = value.NativeString;}
        }
        public CustomCSharpString Stream{
            get{return (CustomCSharpString)this._stream;}
            set{this._stream.NativeString = value.NativeString;}
        }

        public virtual string Type(){
            return "Shared.ScopedStream";
        }
    }


    //  ***** Wrapper for ScopedSegment *****]
    public class ScopedSegment
    {
        private Scope _scope;
        private Stream _stream;
        private Segment _segment;

        // Default Constructor
        public ScopedSegment(){
            this._scope = new Scope();
            this._segment = new Segment();
            this._stream = new Stream();
        }

        // Setters and Getters
        public CustomCSharpString Scope{
            get{return (CustomCSharpString)this._scope;}
            set { this._scope.NativeString = value.NativeString; }
        }
        public CustomCSharpString Stream{
            get{return (CustomCSharpString)this._stream;}
            set { this._stream.NativeString = value.NativeString; }
        }
        public Segment Segment{
            get{return this._segment;}
            set{this._segment = value;}
        }

        public virtual string Type(){
            return "Shared.ScopedSegment";
        }
    }



    //  ***** Wrapper for Segment *****
    public class Segment
    {
        private long _number;
        private TxId _txId;

        // Constructor
        public Segment(){
            this._number = 0;
            this._txId = new TxId();
        }

        // Setters and Getters
        public long Number{
            get{return this._number;}
            set{this._number = value;}
        }

        public virtual string Type(){
            return "Shared.Segment";
        }
    }





    // ***** Wrapper for PravegaNodeUri *****
    public class PravegaNodeUri : RustStructWrapper
    {
    }
    /* 
        Originally from pravega-client-rust/shared/src/lib.rs
        as:
        #[derive(From, Shrinkwrap, Debug, Clone, Hash, PartialEq, Eq)]
        pub struct PravegaNodeUri(pub String);
    */

    public class AsyncSegmentReaderImpl : RustStructWrapper
    {

        public AsyncSegmentReaderImpl(ScopedSegment s)

        {

            this.RustStructPointer = IntPtr.Zero;

        }
    }

    public class RawClientImpl : RustStructWrapper
    {

        public RawClientImpl(ScopedSegment s)

        {

            this.RustStructPointer = IntPtr.Zero;

        }

        public RawClientImpl(PravegaNodeUri p)
        {
            this.RustStructPointer = IntPtr.Zero;
        }

    }

    public class SegmentMetaDataClient : RustStructWrapper
    {

        public SegmentMetaDataClient(ScopedSegment s)

        {

            this.RustStructPointer = IntPtr.Zero;

        }


    }

    public class DelegationTokenProvider : RustStructWrapper
    {
        public DelegationTokenProvider(ScopedStream s)

        {

            this.RustStructPointer = IntPtr.Zero;

        }
    }
}
