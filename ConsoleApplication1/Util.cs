using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AppRunningLogger
{
    using RulesDictionary = Dictionary<string, Rule>;

    class Util
    {
        /// <summary>
        /// Converts `Rule` array to a new dictionary.
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public static RulesDictionary RulesToDictionary(Rule[] rules)
        {
            var dict = new Dictionary<string, Rule>();
            foreach (var rule in rules) {
                var normalizedPath = rule.NormalizedPath;
                if (dict.ContainsKey(normalizedPath.RealPath))
                {
                    // TODO: 重複ルールの扱い
                }
                else
                {
                    dict.Add(normalizedPath.RealPath, rule);
                }
            }
            return dict;
        }

        /// <summary>
        /// RulesDictionary と Process[] を受け取り、マッチしたルールを配列で返す
        /// </summary>
        /// <param name="dict"></param>
        public static Rule[] MatchProcessesWithRules(RulesDictionary dict, Process[] processes)
        {
            var rules = new Rule[0];
            foreach (var process in processes) {
                try
                {
                    var npath = new NormalizedPath(process.MainModule.FileName);
                }
                catch(Exception e)
                {

                }
            }
            return rules;
        }
    }
}
