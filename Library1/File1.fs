module AppRunningLogger.File1

open System.Diagnostics
open System.Threading
open AppRunningLogger.SQLiteTest

type AppRunningLoggerState =
    { Connection : DBConnection
    }

let getProcesses = Process.GetProcesses : unit -> Process []

let rec mainLoop (state : AppRunningLoggerState) =
    let appDefs = SQLiteTest.getAppDefinition state.Connection
    let appDict =
        appDefs
        |> Seq.map (fun x -> (x.CanonicalPath, x))
        |> dict
    let procs = getProcesses ()
    let procPaths =
        procs
        |> Array.map (fun x ->
            try
                x.MainWindowHandle |> ignore
                let npath = CanonicalPath x.MainModule.FileName
                Some npath.RawPath
            with | ex -> None
        )
        |> Array.choose id
    let newPaths =
        procPaths
        |> Array.filter (fun x -> appDict.ContainsKey x |> not)
    newPaths
    |> Array.map (fun x -> SQLiteTest.AppDefinition(CanonicalPath = x))
    |> SQLiteTest.addAppDefinition state.Connection
    |> printfn "new: %d"
    Thread.Sleep 1000
    mainLoop state
   
let startMainLoop () =
    let conn = SQLiteTest.initMainDB ()
    mainLoop { Connection = conn }
    ()