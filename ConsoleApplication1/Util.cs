using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AppRunningTimer
{
    using RulesDictionary = Dictionary<string, Rule>;

    class Util
    {
        /// <summary>
        /// Get normalized path.
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

        /// <summary>
        /// Converts `Rule` array to a new dictionary.
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public static RulesDictionary RulesToDictionary(Rule[] rules)
        {
            var dict = new Dictionary<string, Rule>();
            foreach (var rule in rules) {
                var normalizedPath = rule.normalizedPath;
                if (dict.ContainsKey(normalizedPath))
                {
                    // TODO: 重複ルールの扱い
                }
                else
                {
                    dict.Add(normalizedPath, rule);
                }
            }
            return dict;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        public static void MatchProcessesWithRules(RulesDictionary dict)
        {

        }
    }
}
