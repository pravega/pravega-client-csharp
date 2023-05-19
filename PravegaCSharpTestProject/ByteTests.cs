///
/// File: ByteTests.cs
/// File Creator: John Sbur
/// Purpose: Continues testing in the byte area.
/// 
namespace PravegaWrapperTestProject
{
    using System;
    using System.Runtime.CompilerServices;
    using Pravega;
    using NUnit.Framework;
    using Pravega.ClientFactoryModule;
    using Pravega.Config;
    using System.Threading.Tasks;
    using Pravega.Shared;
    using Pravega.Byte;
    using Pravega.ControllerCli;
    using Pravega.Utility;
    using System.Xml.Schema;

    public partial class PravegaCSharpTest
    {
        /// <summary>
        ///  ByteWriter tests
        ///  Tests done on ByteReader require a running Pravega Server in order to
        ///  socket into and run.
        /// </summary>

        /// <summary
        ///  Helper method for testing. Creates a ByteWriter with default values
        ///  under the scope "testScope" and the stream "testStream"
        /// ></summary>
        internal static ByteWriter CreateTestByteWriter(string streamName)
        {
            ClientFactory.Initialize();
            ControllerClient testController = ClientFactory.FactoryControllerClient;

            // Create a scope to base the stream on.
            Scope testScope = new Scope();
            testScope.NativeString = "testScope";
            testController.CreateScope(testScope).GetAwaiter().GetResult();

            // Create a stream config to control the stream
            StreamConfiguration streamConfiguration = new StreamConfiguration();
            streamConfiguration.ConfigScopedStream.Scope = testScope;
            streamConfiguration.ConfigScopedStream.Stream = new CustomCSharpString(streamName);

            // Create the stream
            testController.CreateStream(streamConfiguration).GetAwaiter().GetResult();

            // Create the ByteWriter
            return ClientFactory.CreateByteWriter(streamConfiguration.ConfigScopedStream).GetAwaiter().GetResult();
        }

        /// <summary>
        ///  ByteWriter Constructor Test. Create ByteWriter from inputted ScopedStream
        ///  
        ///  Prereq. 
        ///  -Scope is initialized and Stream is initialized on that scope
        ///  -Pravega Server is running.
        ///  -Client Factory is initialized.
        /// </summary>
        /// <param name="scopeToBaseOn">
        ///  Scope to test on.
        /// </param>
        /// <param name="streamName">
        ///  Stream to test on.
        /// </param>
        /// <param name="testCase">
        ///  Type of test being done. 
        ///  1 = normal
        ///  2 = boarder
        ///  3 = exception
        /// </param>
        [Test]
        [TestCase("testScope", "testStream")]
        [TestCase(" ", " ", 3)]
        public void ByteWriterConstructorTest(string scopeToBaseOn, string streamName, int testCase=1)
        {
            ClientFactory.Initialize();
            ControllerClient testController = ClientFactory.FactoryControllerClient;

            // Normal Case
            if (testCase == 1)
            {
                // Create a scope to base the stream on.
                Scope testScope = new Scope();
                testScope.NativeString = scopeToBaseOn;
                Assert.IsTrue(testController.CreateScope(testScope).GetAwaiter().GetResult());

                // Create a stream config to control the stream
                StreamConfiguration streamConfiguration = new StreamConfiguration();
                streamConfiguration.ConfigScopedStream.Scope = testScope;
                streamConfiguration.ConfigScopedStream.Stream = new CustomCSharpString(streamName);

                // Create the stream
                Assert.IsTrue(testController.CreateStream(streamConfiguration).GetAwaiter().GetResult());

                // Create the ByteWriter
                ClientFactory.CreateByteWriter(streamConfiguration.ConfigScopedStream).GetAwaiter().GetResult();
                Assert.Pass();
            }
            // Exception Case (We expect the code to fail)
            else
            {
                try
                {
                    // Create a scope to base the stream on.
                    Scope testScope = new Scope();
                    testScope.NativeString = scopeToBaseOn;
                    Assert.IsTrue(testController.CreateScope(testScope).GetAwaiter().GetResult());

                    // Create a stream config to control the stream
                    StreamConfiguration streamConfiguration = new StreamConfiguration();
                    streamConfiguration.ConfigScopedStream.Scope = testScope;
                    streamConfiguration.ConfigScopedStream.Stream = new CustomCSharpString(streamName);

                    // Create the stream
                    Assert.IsTrue(testController.CreateStream(streamConfiguration).GetAwaiter().GetResult());

                    // Create the ByteWriter
                    ClientFactory.CreateByteWriter(streamConfiguration.ConfigScopedStream).GetAwaiter().GetResult();
                }
                catch
                {
                    Assert.Pass();
                }
                Assert.Fail();
            }
        }

        /// <summary>
        ///     Byte Writer Current Offset Test. Verify that offset is changed when writing froma  bytewriter
        /// </summary>
        [Test]
        public void ByteWriterCurrentOffsetTest()
        {
            // Testing variables
            List<byte> testList = new List<byte>();
            testList.Clear();
            testList.Add(0);
            testList.Add(1);
            testList.Add(2);
            testList.Add(3);
            testList.Add(4);

            // Create a byte writer
            ByteWriter testWriter = CreateTestByteWriter(RandomString(8));

            // Get current offset
            ulong currentOffset = testWriter.CurrentOffset;

            // Attempt to write to byte writer
            ulong result = testWriter.Write(testList).GetAwaiter().GetResult();

            // Pass if 0 or more variables were written, but not more than the size of the list. Fail otherwise
            if (result > (ulong)testList.Count || result < 0)
            {
                Assert.Fail();
            }

            // Verify that current offset changed by the amount written or more if the tail is longer than the amount written (in case streams are writing in parallel to it when it gets the current offset)
            Assert.IsTrue(currentOffset + result >= testWriter.CurrentOffset);
        }
 
        /// <summary>
        ///  ByteWriter Write Test. Write bytes inputted in test list to byte writer inputted
        ///  
        ///  Prereq. 
        ///  -ByteWriter is initialized
        ///  -Pravega Server is running.
        ///  -Client Factory is initialized.
        /// </summary>
        /// <param name="testWriter"></param>
        /// <param name="testList"></param>
        [Test]
        public void ByteWriterWriteTest()
        {
            // Testing variables
            List<byte> testList = new List<byte>();
            testList.Clear();
            testList.Add(0);
            testList.Add(1);
            testList.Add(2);
            testList.Add(3);
            testList.Add(4);

            // Create a byte writer
            ByteWriter testWriter = CreateTestByteWriter(RandomString(8));
                
            // Attempt to write to byte writer
            ulong result =  testWriter.Write(testList).GetAwaiter().GetResult();

            // Pass if 0 or more variables were written, but not more than the size of the list. Fail otherwise
            if (result <= (ulong)testList.Count && result >= 0) {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        /// <summary>
        ///  Byte Writer Flush Test. Tests the ability to flush from a byte writer object
        /// </summary>
        [Test]
        public void ByteWriterFlushTest()
        {
            // Testing variables
            List<byte> testList = new List<byte>();
            testList.Clear();
            testList.Add(0);
            testList.Add(1);
            testList.Add(2);
            testList.Add(3);
            testList.Add(4);

            // Create a byte writer
            ByteWriter testWriter = CreateTestByteWriter(RandomString(8));

            // Attempt to write to byte writer
            ulong result = testWriter.Write(testList).GetAwaiter().GetResult();

            // Attempt to flush the byte writer data to permanent data. Pass when done
            testWriter.Flush().GetAwaiter().GetResult();
            Assert.Pass();
        }

        /// <summary>
        ///  Byte Writer Seal Test. Tests the ability to seal a stream
        /// </summary>
        [Test]
        public void ByteWriterSealTest()
        {
            // Client Factory for testing
            ClientFactory.Initialize();
            ControllerClient testController = ClientFactory.FactoryControllerClient;

            // Create a scope to base the stream on.
            Scope testScope = new Scope();
            testScope.NativeString = "uniqueByteTestScope";
            testController.CreateScope(testScope).GetAwaiter().GetResult();

            // Create a stream config to control the stream
            StreamConfiguration streamConfiguration = new StreamConfiguration();
            streamConfiguration.ConfigScopedStream.Scope = testScope;
            streamConfiguration.ConfigScopedStream.Stream = new CustomCSharpString(RandomString(8));

            // Create the stream
            testController.CreateStream(streamConfiguration).GetAwaiter().GetResult();

            // Create the ByteWriter
            ByteWriter testWriter =  ClientFactory.CreateByteWriter(streamConfiguration.ConfigScopedStream).GetAwaiter().GetResult();

            // Seal the testWriter
            testWriter.Seal().GetAwaiter().GetResult();

            // Attempt to flush the data, which should error and show that the stream is sealled
            Assert.Pass();
        }

        /// <summary>
        ///  Byte Writer TruncateBefore Test. 
        /// </summary>
        [Test]
        public void ByteWriterTruncateBeforeTest()
        {
            // Testing variables
            List<byte> testList = new List<byte>();
            testList.Clear();
            testList.Add(0);
            testList.Add(1);
            testList.Add(2);
            testList.Add(3);
            testList.Add(4);

            // Generate stream name
            string streamName = RandomString(8);

            // Create a byte writer
            ByteWriter testWriter = CreateTestByteWriter(streamName);

            // Create a reader with an offset of 0. Trying to read a byte should fail.
            ByteReader testReader = CreateTestByteReader(streamName);

            // Make sure the offset is currently 0
            Assert.IsTrue(testReader.CurrentOffset == 0);

            // Attempt to write to byte writer
            ulong result = testWriter.Write(testList).GetAwaiter().GetResult();

            // Truncate data before "1"
            testWriter.TruncateDataBefore(1).GetAwaiter().GetResult();
            Assert.Pass();
        }

        /// <summary>
        ///  Byte Writer Seek To Tail Test. Verifies that seek to tail seeks to the end of the segment.
        /// </summary>
        [Test]
        public void ByteWriterSeekToTailTest()
        {
            // Testing variables
            List<byte> testList = new List<byte>();
            testList.Clear();
            testList.Add(0);
            testList.Add(1);
            testList.Add(2);
            testList.Add(3);
            testList.Add(4);

            // Generate stream name
            string streamName = RandomString(8);

            // Create two byte writers
            ByteWriter testWriter = CreateTestByteWriter(streamName);
            ByteWriter testWriter2 = CreateTestByteWriter(streamName);

            // Write data through one bytewriter and seektotail on the other. Check to see if the offset isn't 0
            ulong result = testWriter.Write(testList).GetAwaiter().GetResult();
            testWriter2.SeekToTail();
            Assert.IsTrue(testWriter2.CurrentOffset != 0);
        }

        /// <summary>
        ///   Byte Writer Reset Test.
        /// </summary>
        [Test]
        public void ByteWriterResetTest()
        {
            // Create two byte writers
            ByteWriter testWriter = CreateTestByteWriter(RandomString(8));

            // Resets the internal reactor. Makes sure nothing freezes up.
            testWriter.Reset();
            Assert.Pass();
        }

        /// <summary>
        ///  ByteReader tests
        ///  
        ///  Tests done on ByteReader require a running Pravega Server in order to
        ///  socket into and run.
        /// </summary>


        /// <summary
        ///  Helper method for testing. Creates a ByteReader with default values
        ///  under the scope "testScope" and the stream "testStream"
        /// ></summary>
        internal static ByteReader CreateTestByteReader(string streamName)
        {
            ClientFactory.Initialize();
            ControllerClient testController = ClientFactory.FactoryControllerClient;

            // Create a scope to base the stream on.
            Scope testScope = new Scope();
            testScope.NativeString = "testScope";
            testController.CreateScope(testScope).GetAwaiter().GetResult();

            // Create a stream config to control the stream
            StreamConfiguration streamConfiguration = new StreamConfiguration();
            streamConfiguration.ConfigScopedStream.Scope = testScope;
            streamConfiguration.ConfigScopedStream.Stream = new CustomCSharpString(streamName);

            // Create the stream
            testController.CreateStream(streamConfiguration).GetAwaiter().GetResult();

            // Create the ByteReader
            return ClientFactory.CreateByteReader(streamConfiguration.ConfigScopedStream).GetAwaiter().GetResult();
        }

        /// <summary>
        ///  ByteReader Constructor Test. Create ByteReader from inputted ScopedStream
        ///  
        ///  Prereq. 
        ///  -Scope is initialized and Stream is initialized on that scope
        ///  -Pravega Server is running.
        ///  -Client Factory is initialized.
        /// </summary>
        /// <param name="scopeToBaseOn">
        ///  Scope to test on.
        /// </param>
        /// <param name="streamName">
        ///  Stream to test on.
        /// </param>
        /// <param name="testCase">
        ///  Type of test being done. 
        ///  1 = normal
        ///  2 = boarder
        ///  3 = exception
        /// </param>
        [Test]
        [TestCase("testScope", "testStream")]
        [TestCase(" ", " ", 3)]
        public void ByteReaderConstructorTest(string scopeToBaseOn, string streamName, int testCase=1)
        {
            ClientFactory.Initialize();
            ControllerClient testController = ClientFactory.FactoryControllerClient;

            // Normal Case
            if (testCase == 1)
            {
                // Create a scope to base the stream on.
                Scope testScope = new Scope();
                testScope.NativeString = scopeToBaseOn;
                Assert.IsTrue(testController.CreateScope(testScope).GetAwaiter().GetResult());

                // Create a stream config to control the stream
                StreamConfiguration streamConfiguration = new StreamConfiguration();
                streamConfiguration.ConfigScopedStream.Scope = testScope;
                streamConfiguration.ConfigScopedStream.Stream = new CustomCSharpString(streamName);

                // Create the stream
                Assert.IsTrue(testController.CreateStream(streamConfiguration).GetAwaiter().GetResult());

                // Create the ByteWriter
                ClientFactory.CreateByteWriter(streamConfiguration.ConfigScopedStream).GetAwaiter().GetResult();
                Assert.Pass();
            }
            // Exception Case (We expect the code to fail)
            else
            {
                try
                {
                    // Create a scope to base the stream on.
                    Scope testScope = new Scope();
                    testScope.NativeString = scopeToBaseOn;
                    Assert.IsTrue(testController.CreateScope(testScope).GetAwaiter().GetResult());

                    // Create a stream config to control the stream
                    StreamConfiguration streamConfiguration = new StreamConfiguration();
                    streamConfiguration.ConfigScopedStream.Scope = testScope;
                    streamConfiguration.ConfigScopedStream.Stream = new CustomCSharpString(streamName);

                    // Create the stream
                    Assert.IsTrue(testController.CreateStream(streamConfiguration).GetAwaiter().GetResult());

                    // Create the ByteWriter
                    ClientFactory.CreateByteReader(streamConfiguration.ConfigScopedStream).GetAwaiter().GetResult();
                }
                catch
                {
                    Assert.Pass();
                }
                Assert.Fail();
            }

        }

        /// <summary>
        ///  Byte Reader Read Test. Tests byte reader's ability to read a payload
        /// </summary>
        [Test]
        public void ByteReaderReadTest()
        {
            // Testing variables
            List<byte> testList = new List<byte>();
            testList.Clear();
            testList.Add(0);
            testList.Add(1);
            testList.Add(2);
            testList.Add(3);
            testList.Add(4);

            // Generate stream
            string streamName = RandomString(8);

            // Create a byte writer
            ByteWriter testWriter = CreateTestByteWriter(streamName);

            // Create a byte reader
            ByteReader testReader = CreateTestByteReader(streamName);

            // Attempt to write to byte writer
            ulong result = testWriter.Write(testList).GetAwaiter().GetResult();

            // Read the amount written to the stream
            byte[] resultingBytes = testReader.Read(result).GetAwaiter().GetResult();

            // Assert that the contents of the read array is the same as the bytes written. If all are the same, pass the test
            for (int i = 0; i < resultingBytes.Length; i++)
            {
                if (resultingBytes[i] != testList[i])
                {
                    Assert.Fail();
                }
            }

            Assert.Pass();
        }

        /// <summary>
        ///  Byte Reader Current Offset Test. Tests byte reader's current offset, making sure it's 0 to start and non-zero after reading.
        /// </summary>
        [Test]
        public void ByteReaderCurrentOffsetTest()
        {
            // Testing variables
            List<byte> testList = new List<byte>();
            testList.Clear();
            testList.Add(0);
            testList.Add(1);
            testList.Add(2);
            testList.Add(3);
            testList.Add(4);

            // Generate stream name
            string streamName = RandomString(8);

            // Create a byte writer
            ByteWriter testWriter = CreateTestByteWriter(streamName);

            // Create a byte reader
            ByteReader testReader = CreateTestByteReader(streamName);

            // Make sure current offset is 0 to start
            Assert.IsTrue(testReader.CurrentOffset == 0);

            // Attempt to write to byte writer
            ulong result = testWriter.Write(testList).GetAwaiter().GetResult();

            // Read the amount written to the stream
            byte[] resultingBytes = testReader.Read(result).GetAwaiter().GetResult();

            // Make sure current offset was changed after reading
            Assert.IsTrue(testReader.CurrentOffset >= (ulong)resultingBytes.Length);

        }

        /// <summary>
        ///  Byte Reader Available Test. Tests byte reader's available field after having content written to its segment and before.
        /// </summary>
        [Test]
        public void ByteReaderAvailableTest()
        {
            // Testing variables
            List<byte> testList = new List<byte>();
            testList.Clear();
            testList.Add(0);
            testList.Add(1);
            testList.Add(2);
            testList.Add(3);
            testList.Add(4);

            // Client Factory for testing
            ClientFactory.Initialize();
            ControllerClient testController = ClientFactory.FactoryControllerClient;

            // Create a scope to base the stream on.
            Scope testScope = new Scope();
            testScope.NativeString = "uniqueByteTestScope2";
            testController.CreateScope(testScope).GetAwaiter().GetResult();

            // Create a stream config to control the stream
            StreamConfiguration streamConfiguration = new StreamConfiguration();
            streamConfiguration.ConfigScopedStream.Scope = testScope;
            streamConfiguration.ConfigScopedStream.Stream = new CustomCSharpString(RandomString(8));

            // Create the stream
            testController.CreateStream(streamConfiguration).GetAwaiter().GetResult();

            // Create the ByteWriter
            ByteWriter testWriter = ClientFactory.CreateByteWriter(streamConfiguration.ConfigScopedStream).GetAwaiter().GetResult();
            ByteReader testReader = ClientFactory.CreateByteReader(streamConfiguration.ConfigScopedStream).GetAwaiter().GetResult();

            // Check reader.available, which should be 0 if nothing has been written.
            Assert.IsTrue(testReader.Available == 0);

            // Write data to stream
            ulong result = testWriter.Write(testList).GetAwaiter().GetResult();

            // Check reader.available, which should be equal to or less than the amount written.
            Assert.IsTrue(testReader.Available <= result);
        }

        /// <summary>
        ///  ByteReaderSeek Test. Tests the different cases of seek and if it functions correctly.
        ///  
        ///  Prereq. 
        ///  -Scope is initialized and Stream is initialized on that scope
        ///  -Pravega Server is running.
        ///  -Client Factory is initialized.
        /// </summary>
        /// <param name="mode">
        ///  Seek mode being tested
        /// </param>
        /// <param name="amount">
        ///  Amount to seek
        /// </param>
        /// <param name="testCase">
        ///  Type of test being done. 
        ///  1 = normal
        ///  2 = boarder
        ///  3 = exception
        /// </param>
        [Test]
        [TestCase((ulong)0, 1)]
        [TestCase((ulong)0, 2)]
        [TestCase((ulong)0, 3)]
        [TestCase((ulong)0, 4)]
        [TestCase((ulong)0, 5)]
        [TestCase((ulong)1, 1)]
        [TestCase((ulong)1, 2)]
        [TestCase((ulong)1, 3)]
        [TestCase((ulong)1, 4)]
        [TestCase((ulong)1, 5)]
        [TestCase((ulong)2, 1)]
        [TestCase((ulong)2, 2)]
        [TestCase((ulong)2, 3)]
        [TestCase((ulong)2, 4)]
        [TestCase((ulong)2, 5)]
        public void ByteReaderSeekTest(ulong mode, long amount, int testCase=1)
        {
            // Testing variables
            List<byte> testList = new List<byte>();
            testList.Clear();
            testList.Add(0);
            testList.Add(1);
            testList.Add(2);
            testList.Add(3);
            testList.Add(4);

            // Client Factory for testing
            ClientFactory.Initialize();
            ControllerClient testController = ClientFactory.FactoryControllerClient;

            // Create a scope to base the stream on.
            Scope testScope = new Scope();
            testScope.NativeString = "uniqueByteTestScope3";
            testController.CreateScope(testScope).GetAwaiter().GetResult();

            // Create a stream config to control the stream
            StreamConfiguration streamConfiguration = new StreamConfiguration();
            streamConfiguration.ConfigScopedStream.Scope = testScope;
            streamConfiguration.ConfigScopedStream.Stream = new CustomCSharpString(RandomString(8));

            // Create the stream
            testController.CreateStream(streamConfiguration).GetAwaiter().GetResult();

            // Create the ByteWriter
            ByteWriter testWriter = ClientFactory.CreateByteWriter(streamConfiguration.ConfigScopedStream).GetAwaiter().GetResult();
            ByteReader testReader = ClientFactory.CreateByteReader(streamConfiguration.ConfigScopedStream).GetAwaiter().GetResult();


            // Normal case
            if (testCase == 1)
            {
                // Test seeking based on the mode and amount
                switch (mode)
                {
                    // Seek from beginning
                    case 0:
                        testReader.Seek(mode, amount);
                        Assert.IsTrue((long)testReader.CurrentOffset <= amount);
                        break;

                    // Seek from current
                    case 1:
                        testReader.Seek(mode, 1);
                        testReader.Seek(mode, amount);
                        Assert.IsTrue((long)testReader.CurrentOffset <= amount + 1);
                        break;

                    // Seek from end
                    case 2:
                        testReader.Seek(mode, amount);
                        Assert.IsTrue(testReader.CurrentOffset >= 0);
                        break;

                    default:
                        Assert.Fail();
                        break;
                }
            }
            
        }

        /// <summary>
        ///  Byte Reader Current Head Test. Tests reading current head before and after the stream is truncated
        /// </summary>
        [Test]
        public void ByteReaderCurrentHeadTest()
        {
            // Testing variables
            List<byte> testList = new List<byte>();
            testList.Clear();
            testList.Add(0);
            testList.Add(1);
            testList.Add(2);
            testList.Add(3);
            testList.Add(4);

            // Generate stream name
            string streamName = RandomString(8);

            // Create a byte writer
            ByteWriter testWriter = CreateTestByteWriter(streamName);

            // Create a byte reader
            ByteReader testReader = CreateTestByteReader(streamName);

            // Make sure current head is 0 to start
            Assert.IsTrue(testReader.CurrentHead().GetAwaiter().GetResult() == 0);

            // Attempt to write to byte writer
            ulong result = testWriter.Write(testList).GetAwaiter().GetResult();

            // Read the amount written to the stream
            byte[] resultingBytes = testReader.Read(result).GetAwaiter().GetResult();


            // Attempt to write to byte writer again
            result = testWriter.Write(testList).GetAwaiter().GetResult();

            // Truncate
            testWriter.TruncateDataBefore(1).GetAwaiter().GetResult();

            // Make sure current head was changed after truncating
            ulong testResult = testReader.CurrentHead().GetAwaiter().GetResult();
            Assert.IsTrue(testResult >= 1);

        }

        /// <summary>
        ///  Byte Reader Current Head Test. Tests reading current head before and after the stream is truncated
        /// </summary>
        [Test]
        public void ByteReaderCurrentTailTest()
        {
            // Testing variables
            List<byte> testList = new List<byte>();
            testList.Clear();
            testList.Add(0);
            testList.Add(1);
            testList.Add(2);
            testList.Add(3);
            testList.Add(4);

            // Generate stream name
            string streamName = RandomString(8);

            // Create a byte writer
            ByteWriter testWriter = CreateTestByteWriter(streamName);

            // Create a byte reader
            ByteReader testReader = CreateTestByteReader(streamName);

            // Get the current tail before writing
            ulong currentTail = testReader.CurrentTail().GetAwaiter().GetResult();

            // Attempt to write to byte writer
            ulong result = testWriter.Write(testList).GetAwaiter().GetResult();

            // Read the amount written to the stream
            byte[] resultingBytes = testReader.Read(result).GetAwaiter().GetResult();

            // Verify current tail changed
            Assert.IsTrue(currentTail != testReader.CurrentTail().GetAwaiter().GetResult());
        }
    }
}