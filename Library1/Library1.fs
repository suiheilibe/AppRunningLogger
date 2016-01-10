namespace AppRunningLogger

open System
open System.IO

/// <summary>
/// 
/// </summary>
type CanonicalPath(path) =
    let canonicalPath =
        Path.GetFullPath((new Uri(path)).LocalPath)
            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .ToUpperInvariant()
    member this.RawPath with get () = canonicalPath
    override this.GetHashCode() = this.RawPath.GetHashCode()
    override this.Equals(obj : obj) =
        match obj with
        | null -> false
        | :? CanonicalPath as cpath -> this.RawPath.Equals(cpath.RawPath)
        | _ -> false