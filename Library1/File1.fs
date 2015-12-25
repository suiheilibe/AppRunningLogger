module AppRunningLogger.File1

open System.Diagnostics
open System.Threading

let getProcesses = Process.GetProcesses : unit -> Process []

let rec mainLoop () =
    let procs = getProcesses()
    procs
    |> Array.map (fun x ->
        try
            x.MainWindowHandle |> ignore
            let npath = NormalizedPath x.MainModule.FileName
            Some (npath, x)
        with | e -> None
    )
    |> Array.choose id
    |> dict
    |> ignore
    Thread.Sleep 1000
    mainLoop()
    