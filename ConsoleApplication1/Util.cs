using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AppRunningTimer
{
    class Util
    {
        /// <summary>
        /// http://stackoverflow.com/questions/1266674/how-can-one-get-an-absolute-or-normalized-file-path-in-net
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }

        public static Dictionary<string,Rule> RulesToDictionary(Rule[] rules)
        {
            foreach (var rule in rules) {
                
            }
            return null;
        }
    }
}
