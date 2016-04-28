using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRunningLogger
{
    class CanonicalPath
    {
        string canonicalPath;
        CanonicalPath(string path)
        {
            canonicalPath = Path.GetFullPath((new Uri(path)).LocalPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .ToUpperInvariant();
        }
    }
}
