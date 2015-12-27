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
