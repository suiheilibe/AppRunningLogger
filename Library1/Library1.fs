namespace AppRunningTimer

open System
open System.IO

type NormalizedPath(path) =
    let normalizedPath = Path.GetFullPath((new Uri(path)).LocalPath)
                          .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToUpperInvariant()
    member this.RealPath with get () = normalizedPath

type Rule = 
    { AppId : uint64
      NormalizedPath : NormalizedPath
      Enabled : bool }

type RuleX(appId, normalizedPath, enabled) = class end
    

module Main =
    let rules = [|{AppId = 0UL; NormalizedPath = NormalizedPath "F:\\GAMES\\mu100\\murasaki.exe"; Enabled = true}|];