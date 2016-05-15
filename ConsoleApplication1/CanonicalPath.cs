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

        public string RawPath
        {
            get { return this.canonicalPath; }
        }

        public override int GetHashCode()
        {
            return this.RawPath.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var cpath = obj as CanonicalPath;
            if (cpath == null)
            {
                return false;
            }
            else
            {
                return this.RawPath.Equals(cpath.RawPath);
            }
        }
    }
}
