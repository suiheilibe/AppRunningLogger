using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace AppRunningTimer
{
    class Program
    {
        static void Main(string[] args)
        {
            Rule[] rules = null;
            try
            {
                rules = new[] { new Rule(0, new NormalizedPath("F:\\GAMES\\mu100\\murasaki.exe"), true) };
            }
            catch (Exception e)
            {
                // TODO: 例外記録メソッド
                Console.WriteLine("An error occurred: '{0}'", e);
            }
            if (rules == null) {
                rules = new Rule[0];
            }
            var rulesDict = Util.RulesToDictionary(rules);
            var cnt = 0;
            while (true) {
                var processes = GetProcesses();
                foreach (var p in processes)
                {
                    if (p.MainWindowTitle == "murasaki")
                    {
                        cnt++;
                        Console.WriteLine(cnt);
                        var module = p.MainModule;

                        Console.WriteLine(module.FileName);
                    }
                }
                Thread.Sleep(1000);
            }
        }
        static Process[] GetProcesses()
        {
            return Process.GetProcesses();
        }
    }
}
