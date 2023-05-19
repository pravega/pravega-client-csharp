///
/// File: PathGen.cs
/// File Creator: Brandon Cook
/// Purpose: Contains methods for helping C# find Rust .dll files.
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pravega.Pathgen
{
    public class PathGen
    {
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
    }
}
