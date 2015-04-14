using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRunningTimer
{
    class Rule
    {
        public ulong appId { get; private set; }
        public string normalizedPath { get; private set; }
        public bool enabled { get; private set; }
        /// <summary>
        /// 例外
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="normalizedPath"></param>
        /// <param name="enabled"></param>
        public Rule(ulong appId, string normalizedPath, bool enabled)
        {
            this.appId = appId;
            this.normalizedPath = Util.NormalizePath(normalizedPath);
            this.enabled = enabled;
        }
    }
}
