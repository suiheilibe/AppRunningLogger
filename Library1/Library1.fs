namespace AppRunningTimer

module PathUtil = 
    open System
    open System.IO
    
    type NormalizedPath = 
        { RealPath : string }
    
    let makeNormalizedPath path = 
        { RealPath = 
              Path.GetFullPath((new Uri(path)).LocalPath)
                  .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToUpperInvariant() }
