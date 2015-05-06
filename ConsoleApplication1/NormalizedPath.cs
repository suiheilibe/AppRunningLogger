using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AppRunningTimer
{
    class NormalizedPath
    {
        public string path { get; private set; }
        /// <summary>
        /// Normalized path
        /// http://stackoverflow.com/questions/1266674/how-can-one-get-an-absolute-or-normalized-file-path-in-net
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public NormalizedPath(string path)
        {
            this.path = Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }
    }
}
