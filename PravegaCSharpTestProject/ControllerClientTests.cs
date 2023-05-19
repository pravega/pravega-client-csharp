///
/// File: ControllerClientTests.cs
/// File Creator: John Sbur
/// Purpose: Continues testing in the controller client area.
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
    using Pravega.ControllerCli;
    using Pravega.Shared;
    using Pravega.Utility;

    public partial class PravegaCSharpTest
    {
        /// <summary>
        ///  Controller Client Tests
        /// </summary>

        /// <summary>
        ///  ControllerClient Create Stream Test. Tests the ability to create streams on a Pravega server from C#
        ///  
        ///  Prereq. 
        ///  -Scope is initialized and Stream is initialized on that scope
        ///  -Pravega Server is running.
        ///  -Client Factory is initialized.
        /// </summary>
        /// <param name="scopeToBaseOn">
        ///  Scope the stream is to be based on
        /// </param>
        /// <param name="streamName">
        ///  Stream being tested
        /// </param>
        /// <param name="testCase">
        ///  Type of test being done. 
        ///  1 = normal
        ///  2 = boarder
        ///  3 = exception
        /// </param>
        [Test]
        [TestCase("testScope", " ", 3)]
        [TestCase("testScope", "testStream")]
        public void ControllerClientCreateStream(string scopeToBaseOn, string streamName, int testCase=1)
        {
            ClientFactory.Initialize();
            ControllerClient testController = ClientFactory.FactoryControllerClient;

            // Create a scope to base the stream on.
            Scope testScope = new Scope();
            testScope.NativeString = scopeToBaseOn;
            testController.CreateScope(testScope).GetAwaiter().GetResult();

            // Normal case
            if (testCase == 1)
            {
                // Create a stream config to control the stream
                StreamConfiguration streamConfiguration = new StreamConfiguration();
                streamConfiguration.ConfigScopedStream.Scope = testScope;
                streamConfiguration.ConfigScopedStream.Stream = new CustomCSharpString(streamName);

                // Create the stream
                Assert.IsTrue(testController.CreateStream(streamConfiguration).GetAwaiter().GetResult());

            }

            // Exception Case
            else if (testCase == 3)
            {
                try
                {
                    // Create a stream config to control the stream
                    StreamConfiguration streamConfiguration = new StreamConfiguration();
                    streamConfiguration.ConfigScopedStream.Scope = testScope;
                    streamConfiguration.ConfigScopedStream.Stream = new CustomCSharpString(streamName);

                    // Create the stream
                    Assert.IsTrue(testController.CreateStream(streamConfiguration).GetAwaiter().GetResult());
                }
                catch
                {
                    Assert.Pass();
                }
                Assert.Fail();
            }
            
        }

        /// <summary>
        ///  ControllerClient Create Scope Test. Tests the ability to create scopes on a Pravega server from C#
        ///  
        ///  Prereq. 
        ///  -Scope is initialized and Stream is initialized on that scope
        ///  -Pravega Server is running.
        ///  -Client Factory is initialized.
        /// </summary>
        /// <param name="scopeName">
        ///  Scope being tested
        /// </param>
        /// <param name="testCase">
        ///  Type of test being done. 
        ///  1 = normal
        ///  2 = boarder
        ///  3 = exception
        /// </param>
        [Test]
        [TestCase(" ", 3)]
        [TestCase("testScope")]
        public void ControllerClientCreateScope(string scopeName, int testCase=1)
        {
            ClientFactory.Initialize();
            ControllerClient testController = ClientFactory.FactoryControllerClient;

            // Normal case
            if (testCase == 1)
            {
                // Create a scope and verify signs of life.
                Scope testScope = new Scope();
                testScope.NativeString = scopeName;
                Assert.IsTrue(testController.CreateScope(testScope).GetAwaiter().GetResult());
            }

            // Exception case
            else if (testCase == 3)
            {
                try
                {
                    // Create a scope and verify signs of life.
                    Scope testScope = new Scope();
                    testScope.NativeString = scopeName;
                    Assert.IsTrue(testController.CreateScope(testScope).GetAwaiter().GetResult());
                }
                catch
                {
                    Assert.Pass();
                }
                Assert.Fail();
            }
        }
        
    }
}