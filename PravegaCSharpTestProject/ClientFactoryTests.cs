///
/// File: ClientFactoryTests.cs
/// File Creator: John Sbur
/// Purpose: Continues testing in the client factory area.
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
    using Pravega.Utility;
    using System.Diagnostics;


    public partial class PravegaCSharpTest
    {
        //The number means how many times slower can the C# library be
        const int ConversionLossLimit = 8;

        const int TestingAmount = 10;

        public static String CreateDllPath()
        {
           var cwd = System.IO.Directory.GetCurrentDirectory();
           String code_base = "Project_Code_Base";
           int indexTo = cwd.IndexOf(code_base);
           String return_string;
           // If IndexOf could not find code_base String
           if (indexTo == -1)
           {
               return "";
           }
           return_string = cwd.Substring(0, indexTo + code_base.Length);
           return_string += @"\cSharpTest\PravegaCSharpLibrary\target\debug\deps\";

           return return_string;
        }
        [SetUp]
        public void SetupEnvironment()
        {
           Environment.CurrentDirectory = CreateDllPath();
        }
        // / <summary>
        // /  Client Factory Tests
        // / </summary> 
        // Unit Test. Client Factory default constructor
        [Test]
        public void ClientFactoryDefaultConstructorTest()
        {
            ClientFactory.Initialize();
            Assert.That(ClientFactory.Initialized(), Is.Not.EqualTo(false));
        }

        // Unit Test. Client Factory default constructor time. Aims for C# being at least 85% efficient
        [Test]
        public void ClientFactoryDefaultConstructorTimeTest()
        {
            double totalTime = 0;
            for (int i = 0; i < TestingAmount; i++)
            {
                // C# time
                Stopwatch timer = new Stopwatch();
                timer.Start();
                ClientFactory.Initialize();
                timer.Stop();
                ClientFactory.Destroy();
                double ticks = timer.ElapsedTicks;
                double timerNanoseconds = (ticks / Stopwatch.Frequency) * 1000000000;
                totalTime += timerNanoseconds;
            }
            totalTime = totalTime / TestingAmount; 
            Console.WriteLine("Rust Time: " + ClientFactoryTestMethods.DefaultConstructorTime().ToString());
            Console.WriteLine("C# Time: " + totalTime.ToString());

            Assert.IsTrue(ClientFactoryTestMethods.DefaultConstructorTime() > totalTime / ConversionLossLimit);
        }

        // Unit Test. Client Factory constructor from client config
        [Test]
        public void ClientFactoryClientConfigConstructorTest()
        {
            ClientConfig testConfig = new ClientConfig();
            testConfig.MaxConnectionsInPool = 10;
            ClientFactory.Initialize(testConfig);

            // Make sure client factory was initiated
            Assert.IsTrue(ClientFactory.Initialized());

            // Make sure that the config information was stored
            Assert.IsTrue(ClientFactory.Config.MaxConnectionsInPool == 10);

            // Make sure the config inputted was consumed
            Assert.IsTrue(testConfig.IsNull());
        }

        // Unit Test. Client Factory constructor from client config time. Aims for C# being at least 85% efficient
        [Test]
        public void ClientFactoryClientConfigConstructorTimeTest()
        {

            double totalTime;
            totalTime = 0;
            for (int i = 0; i < TestingAmount; i++)
            {
                ClientFactory.Initialize();
                ClientConfig testConfig = ClientFactory.Config;
                ClientFactory.Destroy();

                // C# time
                Stopwatch timer = new Stopwatch();
                timer.Start();
                ClientFactory.Initialize(testConfig);
                timer.Stop();
                ClientFactory.Destroy();
                double ticks = timer.ElapsedTicks;
                double timerNanoseconds = (ticks / Stopwatch.Frequency) * 1000000000;
                totalTime += timerNanoseconds;
            }
            totalTime = totalTime / TestingAmount;
            Console.WriteLine("Rust Time: " + ClientFactoryTestMethods.ConstructorConfigTime().ToString());
            Console.WriteLine("C# Time: " + totalTime.ToString());

            Assert.IsTrue((ClientFactoryTestMethods.ConstructorConfigTime() > totalTime / ConversionLossLimit));
        }

        // Unit Test. Client Factory constructor from client config and runtime
        [Test]
        public void ClientFactoryConfigRuntimeConstructorTest()
        {
            // Create a config for testing.
            ClientConfig testConfig = new ClientConfig();
            testConfig.MaxConnectionsInPool = 10;

            // Create a runtime for testing
            TokioRuntime testRuntime = new TokioRuntime();

            // Initialze factory with runtime and config created.
            ClientFactory.Initialize(testConfig, testRuntime);

            // Make sure client factory was initiated
            Assert.IsTrue(ClientFactory.Initialized());

            // Make sure that the config information was stored
            Assert.IsTrue(ClientFactory.Config.MaxConnectionsInPool == 10);

            // Make sure that the client factory's runtime isn't null
            Assert.IsFalse(ClientFactory.Runtime.IsNull());

            // Make sure the config inputted was consumed
            Assert.IsTrue(testConfig.IsNull());

            // Make sure the runtime inputted was consumed
            Assert.IsTrue(testRuntime.IsNull());
        }

        // Unit Test. Client Factory constructor from client config and runtime time. Aims for C# being at least 85% efficient
        [Test]
        public void ClientFactoryConfigRuntimeConstructorTimeTest()
        {

            // C# time
            double totalTime = 0;
            for (int i = 0; i < TestingAmount; i++)
            {

                ClientFactory.Initialize();
                TokioRuntime testRuntime = ClientFactory.Runtime;
                ClientConfig testConfig = ClientFactory.Config;
                ClientFactory.Destroy();

                Stopwatch timer = new Stopwatch();
                timer.Start();
                ClientFactory.Initialize(testConfig, testRuntime);
                timer.Stop();
                ClientFactory.Destroy();
                double ticks = timer.ElapsedTicks;
                double timerNanoseconds = (ticks / Stopwatch.Frequency) * 1000000000;
                totalTime += timerNanoseconds;
            }
            totalTime = totalTime / TestingAmount;
            Console.WriteLine("Rust Time: " + ClientFactoryTestMethods.ConstructorConfigAndRuntimeTime().ToString());
            Console.WriteLine("C# Time: " + totalTime.ToString());

            Assert.IsTrue((ClientFactoryTestMethods.ConstructorConfigAndRuntimeTime() > totalTime / ConversionLossLimit));
        }

        // Unit Test. Client Factory runtime
        [Test] 
        public void ClientFactoryRuntimeTest()
        {
            ClientFactory.Initialize();
            TokioRuntime testRuntime = ClientFactory.Runtime;
            Assert.IsFalse(testRuntime.IsNull());
        }

        // Unit Test. Client Factory handle time. Aims for C# being at least 85% efficient
        [Test]
        public void ClientFactoryRuntimeTimeTest()
        {
            double totalTime = 0;
            ClientFactory.Initialize();
            for (int i = 0; i < TestingAmount; i++)
            {

                // C# time
                Stopwatch timer = new Stopwatch();
                timer.Start();
                TokioRuntime testConfig = ClientFactory.Runtime;
                timer.Stop();
                double ticks = timer.ElapsedTicks;
                double timerNanoseconds = (ticks / Stopwatch.Frequency) * 1000000000;
                totalTime += timerNanoseconds;
            }
            totalTime = totalTime / TestingAmount;
            Console.WriteLine("Rust Time: " + ClientFactoryTestMethods.RuntimeTime().ToString());
            Console.WriteLine("C# Time: " + totalTime.ToString());

            Assert.IsTrue((ClientFactoryTestMethods.RuntimeTime() > totalTime / ConversionLossLimit));
        }

        // Unit Test. Client Factory handle
        [Test]
        public void ClientFactoryHandleTest()
        {
            ClientFactory.Initialize();
            TokioHandle testHandle = ClientFactory.Handle;
            Assert.IsFalse(testHandle.IsNull());
        }

        // Unit Test. Client Factory handle time. Aims for C# being at least 85% efficient
        [Test]
        public void ClientFactoryHandleTimeTest()
        {
            double totalTime = 0;
            ClientFactory.Initialize();

            for (int i = 0; i < TestingAmount; i++)
            {

                // C# time
                Stopwatch timer = new Stopwatch();
                timer.Start();
                TokioHandle testConfig = ClientFactory.Handle;
                timer.Stop();
                double ticks = timer.ElapsedTicks;
                double timerNanoseconds = (ticks / Stopwatch.Frequency) * 1000000000;
                totalTime += timerNanoseconds;
            }
            totalTime = totalTime / TestingAmount;
            Console.WriteLine("Rust Time: " + ClientFactoryTestMethods.HandleTime().ToString());
            Console.WriteLine("C# Time: " + totalTime.ToString());

            Assert.IsTrue((ClientFactoryTestMethods.HandleTime() > totalTime / ConversionLossLimit));
        }

        // Unit Test. Client Factory config
        [Test]
        public void ClientFactoryConfigTest()
        {
            ClientFactory.Initialize();
            ClientConfig testConfig = ClientFactory.Config;
            Assert.IsFalse(testConfig.IsNull());
        }

        // Unit Test. Client Factory config time. Aims for C# being at least 85% efficient
        [Test]
        public void ClientFactoryConfigTimeTest()
        {

            double totalTime = 0;
            ClientFactory.Initialize();
            for (int i = 0; i < TestingAmount; i++)
            {
                // C# time
                Stopwatch timer = new Stopwatch();
                timer.Start();
                ClientConfig testConfig = ClientFactory.Config;
                timer.Stop();
                double ticks = timer.ElapsedTicks;
                double timerNanoseconds = (ticks / Stopwatch.Frequency) * 1000000000;
                totalTime += timerNanoseconds;
            }
            totalTime = totalTime / TestingAmount;
            Console.WriteLine("Rust Time: " + ClientFactoryTestMethods.ConfigTime().ToString());
            Console.WriteLine("C# Time: " + totalTime.ToString());

            Assert.IsTrue((ClientFactoryTestMethods.ConfigTime() > totalTime / ConversionLossLimit));
        }

        // Unit Test. Client Factory to async
        [Test]
        public void ClientFactoryToAsyncTest()
        {
            ClientFactory.Initialize();
            ClientFactoryAsync testAsyncFactory = ClientFactory.ToAsync();
            Assert.IsFalse(testAsyncFactory.IsNull());
        }

        // Unit Test. Client Factory to async time. Aims for C# being at least 85% efficient
        [Test]
        public void ClientFactoryAsyncTimeTest()
        {
            double totalTime = 0;
            ClientFactory.Initialize();
            for (int i = 0; i < TestingAmount; i++)
            {
                // C# time
                Stopwatch timer = new Stopwatch();
                timer.Start();
                ClientFactoryAsync testAsyncFactory = ClientFactory.ToAsync();
                timer.Stop();
                double ticks = timer.ElapsedTicks;
                double timerNanoseconds = (ticks / Stopwatch.Frequency) * 1000000000;
                totalTime += timerNanoseconds;
            }
            totalTime = totalTime / TestingAmount;
            Console.WriteLine("Rust Time: " + ClientFactoryTestMethods.HandleTime().ToString());
            Console.WriteLine("C# Time: " + totalTime.ToString());

            Assert.IsTrue((ClientFactoryTestMethods.ToAsyncTime() > totalTime / ConversionLossLimit));
        }
    }
}