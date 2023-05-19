///
/// File: RetryTests.cs
/// File Creator: John Sbur
/// Purpose: Continues testing in the retry area.
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

    public partial class PravegaCSharpTest
    {
        /// <summary>
        ///  RetryWithBackoff Tests
        /// </summary>
        // Unit Test. RetryWithBackoff Constructor
        [Test]
        public void RetryWithBackoffDefaultConstructor()
        {
            RetryWithBackoff testPolicy = new RetryWithBackoff();
            Assert.That(testPolicy.IsNull(), Is.Not.EqualTo(true));
        }
    }
}
