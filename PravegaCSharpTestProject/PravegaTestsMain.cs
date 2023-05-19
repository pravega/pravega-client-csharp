///
/// File: PravegaTest.cs
/// File Creator: John Sbur
/// Purpose: Contains the test class for the PravegaCSharp wrapper library. Uses NUnit for testing.
///     Methods tested are in different files under their module's name.
///
namespace PravegaWrapperTestProject
{
    using System;
    using System.Runtime.CompilerServices;
    using Pravega;
    using NUnit.Framework;

    public partial class PravegaCSharpTest
    {
        /// <summary>
        ///  Helper test function
        /// </summary>
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [SetUp]
        public void Setup()
        {
            var cwd = System.IO.Directory.GetCurrentDirectory();
            String code_base = "Project_Code_Base";
            int indexTo = cwd.IndexOf(code_base);
            String return_string;
            // If IndexOf could not find code_base String
            if (indexTo == -1)
            {
                return_string = "";
            }
            else
            {
                return_string = cwd.Substring(0, indexTo + code_base.Length);
                return_string += @"\cSharpTest\PravegaCSharpLibrary\target\debug\deps\";
            }
            Environment.CurrentDirectory = return_string;

        }
    }
}