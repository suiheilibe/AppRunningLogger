namespace AppRunningLogger

open System
open System.IO

/// <summary>
/// 
/// </summary>
type CanonicalPath(path) =
    let canonicalPath = 
        Path.GetFullPath((new Uri(path)).LocalPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .ToUpperInvariant()
    member this.RawPath = canonicalPath
    override this.GetHashCode() = this.RawPath.GetHashCode()
    override this.Equals(o : obj) = 
        match o with
        | null -> false
        | :? CanonicalPath as cpath -> this.RawPath.Equals(cpath.RawPath)
        | _ -> false
    interface IEquatable<CanonicalPath> with
        member this.Equals(o : CanonicalPath) = this.RawPath.Equals(o.RawPath)