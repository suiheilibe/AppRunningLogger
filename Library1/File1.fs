module AppRunningLogger.File1

open System.Diagnostics
open System.Threading

type AppRunningLoggerState =
    { Connection : SQLiteTest.DBConnection
    }

let getProcesses = Process.GetProcesses : unit -> Process []

let rec mainLoop (state : AppRunningLoggerState) =
    let appDefs = SQLiteTest.getAppDefinition state.Connection
    let appDict =
        appDefs
        |> Seq.map (fun x ->
            try
                Some ((CanonicalPath x.Path).RawPath, x)
            with | ex -> None
            )
        |> Seq.choose id
        |> dict
    let procs = getProcesses ()
    let procPathPairs =
        procs
        |> Array.map (fun x ->
            try
                x.MainWindowHandle |> ignore
                let fileName = x.MainModule.FileName
                Some ((CanonicalPath fileName).RawPath, fileName) // (<Canonicalized path>, <`Process` path>)
            with | ex -> None
            )
        |> Array.choose id
    let newPathPairs =
        procPathPairs
        |> Array.filter (fun x -> fst x |> appDict.ContainsKey |> not)
    newPathPairs
    |> Array.map (fun x -> SQLiteTest.AppDefinition(Path = snd x))
    |> SQLiteTest.addAppDefinition state.Connection
    |> printfn "new: %d"
    Thread.Sleep 1000
    mainLoop state
   
let startMainLoop () =
    let conn = SQLiteTest.initMainDB ()
    mainLoop { Connection = conn }
    ()