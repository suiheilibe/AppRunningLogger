namespace AppRunningTimer

type NormalizedPath = 
    { RealPath : string }

type Rule = 
    { AppId : uint64
      NormalizedPath : NormalizedPath
      Enabled : bool }

module PathUtil = 
    open System
    open System.IO
    
    let makeNormalizedPath path = 
        { RealPath = 
              Path.GetFullPath((new Uri(path)).LocalPath)
                  .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToUpperInvariant() }

module Main =
    let rules = [|{AppId = 0UL; NormalizedPath = PathUtil.makeNormalizedPath "F:\\GAMES\\mu100\\murasaki.exe"; Enabled = true}|];