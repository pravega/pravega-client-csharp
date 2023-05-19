///
/// File: ClientConfigTests.cs
/// File Creator: John Sbur
/// Purpose: Continutes testing in the ClientConfig area
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
    using Pravega.Retry;
    using Pravega.Shared;

    public partial class PravegaCSharpTest
    {

        /// <summary>
        ///  Client Config Tests
        /// </summary>
        // Unit Test. Client Config Constructor
        [Test]
        public void ConfigConstructor()
        {
            ClientConfig testConfig = new ClientConfig();
            Assert.That(testConfig.IsNull(), Is.Not.EqualTo(true));
        }

        // Unit Test. Client Config Max Connections In Pool
        [Test]
        [TestCase((uint)0)]
        [TestCase((uint)10)]
        [TestCase(uint.MaxValue)]
        public void ConfigMaxConnectionsInPoolTest(uint testInput = 0)
        {
            ClientConfig testConfig = new ClientConfig();
            testConfig.MaxConnectionsInPool = testInput;
            Assert.That(testInput, Is.EqualTo(testConfig.MaxConnectionsInPool));
        }

        // Unit Test. Client Config Max Controller Connections
        [Test]
        [TestCase((uint)0)]
        [TestCase((uint)10)]
        [TestCase(uint.MaxValue)]
        public void ConfigMaxControllerConnectionsTest(uint testInput = 0)
        {
            ClientConfig testConfig = new ClientConfig();
            testConfig.MaxControllerConnections = testInput;
            Assert.That(testInput, Is.EqualTo(testConfig.MaxControllerConnections));
        }

        // Unit Test. Client Config Retry Policy
        [Test]
        public void ConfigRetryPolicyTest()
        {
            ClientConfig testConfig = new ClientConfig();

            // Setter and getter functions for RetryPolicy clone, so there's not need to check whether the pointers match. 
            // We only need to check whether it crashes and whether the RetryPolicy values are the same
            RetryWithBackoff testRetry = testConfig.RetryPolicy;
            testConfig.RetryPolicy = testRetry;
            testRetry = testConfig.RetryPolicy;
            Assert.Pass();
        }

        // Unit Test. Client Config Controller Uri
        [Test]
        public void ConfigControllerUriTest()
        {
            ClientConfig testConfig = new ClientConfig();

            // Setter and getter functions for ControllerUri clone, so there's not need to check whether the pointers match. 
            // We only need to check whether it crashes and whether the RetryPolicy values are the same
            PravegaNodeUri testUri = testConfig.ControllerUri;
            testConfig.ControllerUri = testUri;
            testUri = testConfig.ControllerUri;
            Assert.Pass();
        }

        // Unit Test. Client Config Transaction Timeout
        [Test]
        [TestCase((uint)0)]
        [TestCase((uint)10)]
        [TestCase(ulong.MaxValue)]
        public void ConfigTransactionTimeoutTest(ulong testInput = 0)
        {
            ClientConfig testConfig = new ClientConfig();
            testConfig.TransactionTimeoutTime = testInput;
            Assert.That(testInput, Is.EqualTo(testConfig.TransactionTimeoutTime));
        }

        // Unit Test. Client Config Mock
        [Test]
        public void ConfigMockTest()
        {
            ClientConfig testConfig = new ClientConfig();
            testConfig.Mock = true;
            Assert.That(testConfig.Mock, Is.True);
            testConfig.Mock = false;
            Assert.That(testConfig.Mock, Is.False);
        }

        // Unit Test. Client Config TLSEnabled
        [Test]
        public void ConfigTlsEnabledTest()
        {
            ClientConfig testConfig = new ClientConfig();
            testConfig.IsTlsEnabled = true;
            Assert.That(testConfig.IsTlsEnabled, Is.True);
            testConfig.IsTlsEnabled = false;
            Assert.That(testConfig.IsTlsEnabled, Is.False);
        }

        // Unit Test. 
    }
}