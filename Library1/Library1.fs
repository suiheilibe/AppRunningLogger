namespace AppRunningTimer

open System
open System.IO
open System.Diagnostics

type NormalizedPath(path) =
    let normalizedPath = Path.GetFullPath((new Uri(path)).LocalPath)
                          .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToUpperInvariant()
    member this.RealPath with get () = normalizedPath

type Rule = 
    { AppId : uint64
      NormalizedPath : NormalizedPath
      Enabled : bool }

type RuleX(appId, normalizedPath, enabled) = class end
