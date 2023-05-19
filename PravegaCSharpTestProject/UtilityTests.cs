///
/// File: UtilityTests.cs
/// File Creator: John Sbur
/// Purpose: Continues testing in the utility area.
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

    public partial class PravegaCSharpTest
    {
        /// <summary>
        ///  CustomCSharpString Tests
        /// </summary>
        // Unit Test. CustomCSharpString default constructor
        [Test]
        public void CustomStringDefaultConstructorTest()
        {
            CustomCSharpString testString = new CustomCSharpString();
            string testCSharpString = testString.NativeString;
            Assert.That(testCSharpString, Is.EqualTo(" "));
        }

        // Unit Test. CustomCSharpString constructor from string as well as getting and setting the native string
        [Test]
        [TestCase("test")]
        [TestCase("")]
        public void CustomStringNativeStringAndConstructorTest(string testInput = "")
        {
            CustomCSharpString testString = new CustomCSharpString(testInput);
            string testCSharpString = testString.NativeString;

            if (testInput == "")
            {
                Assert.That(testString.NativeString, Is.EqualTo(" "));
            }
            else
            {
                Assert.That(testString.NativeString, Is.EqualTo(testInput));
            }
        }

        // Unit Test. CustomCSharpString constructor from large string
        [Test]
        public void CustomStringFromBigStringTest()
        {
            string testInput = string.Empty;

            for (int i = 0; i < Math.Pow(2, 15); i++)
            {
                testInput += "a";
            }

            CustomCSharpString testString = new CustomCSharpString(testInput);
            string testCSharpString = testString.NativeString;
            Assert.That(testCSharpString, Is.EqualTo(testInput));
        }

        // Unit Test. CustomCSharpString constructor from a rust string as well as getting and setting the rust string
        [Test]
        [TestCase("test")]
        [TestCase("")]
        public void CustomStringRustStringAndConstructorTest(string testInput = "")
        {
            CustomCSharpString testString = new CustomCSharpString(testInput);
            CustomRustString testRustString = testString.RustString;
            testString = new CustomCSharpString(testRustString);

            if (testInput == "")
            {
                Assert.That(testString.NativeString, Is.EqualTo(" "));
            }
            else
            {
                Assert.That(testString.NativeString, Is.EqualTo(testInput));
            }
        }

        // Unit Test. CustomCSharpString constructor from large rust string
        [Test]
        public void CustomStringFromBigRustStringTest()
        {
            string testInput = string.Empty;

            for (int i = 0; i < Math.Pow(2, 15); i++)
            {
                testInput += "a";
            }

            CustomCSharpString testString = new CustomCSharpString(testInput);
            CustomRustString testRustString = testString.RustString;
            testString = new CustomCSharpString(testRustString);
            Assert.That(testString.NativeString, Is.EqualTo(testInput));
        }

        // Unit Test. CustomCSharpString constructor from CustomCSharpString
        [Test]
        [TestCase("test")]
        [TestCase("")]
        public void CustomStringFromCustomStringTest(string testInput = "")
        {
            CustomCSharpString testString = new CustomCSharpString(testInput);
            CustomCSharpString testString2 = new CustomCSharpString(testString);
            Assert.That(testString.NativeString, Is.EqualTo(testString2.NativeString));
        }

        // Unit Test. CustomCSharpString clone
        [Test]
        public void CustomStringCloneTest()
        {
            CustomCSharpString testString = new CustomCSharpString("test");
            CustomCSharpString testString2 = testString.Clone();
            Assert.That(testString2.NativeString, Is.EqualTo(testString.NativeString));
        }

        // Unit Test. Checks that capacity updates with new strings
        [Test]
        [TestCase("test")]
        [TestCase("")]
        public void CustomStringCapacityTest(string testInput = "")
        {
            CustomCSharpString testString = new CustomCSharpString(testInput);
            if (testInput == "")
            {
                Assert.That(1, Is.EqualTo(testString.Capacity));
            }
            else
            {
                Assert.That(testInput.Length, Is.EqualTo(testString.Capacity));
            }
        }

        /// <summary>
        ///  RustStructWrapper Tests
        /// </summary>
        // Unit Test. RustStruct default constructor
        /*
        [Test]
        public void RustStructDefaultConstructorTest()
        {
            RustStructWrapper testWrapper = new RustStructWrapper();
            Assert.IsTrue(testWrapper.IsNull());
        }

        // Unit Test. RustStruct IsEqual
        [Test] 
        public void RustStructIsEqualTest()
        {
            RustStructWrapper testWrapper = new RustStructWrapper();
            RustStructWrapper testWrapper2 = new RustStructWrapper();
            if (testWrapper.IsNull() && testWrapper2.IsNull())
            {
                Assert.IsTrue(testWrapper.IsEqual(testWrapper2));
            }
            else
            {
                Assert.Fail();
            }
        }
        */


    }
}