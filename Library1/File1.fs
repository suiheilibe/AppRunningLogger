module AppRunningLogger.File1

open System.Diagnostics
open System.Threading
open AppRunningLogger.SQLiteTest

type AppRunningLoggerState =
    { Connection : DBConnection
    }

let getProcesses = Process.GetProcesses : unit -> Process []

let rec mainLoop (state : AppRunningLoggerState) =
    let procs = getProcesses()
    procs
    |> Array.map (fun x ->
        try
            x.MainWindowHandle |> ignore
            let npath = CanonicalPath x.MainModule.FileName
            Some (npath.RawPath, x)
        with | e -> None
    )
    |> Array.choose id
    |> dict
    |> ignore
    Thread.Sleep 1000
    mainLoop state
   
let startMainLoop () =
    let conn = SQLiteTest.initMainDB
    mainLoop { Connection = conn }
    ()