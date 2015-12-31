module AppRunningLogger.File1

open System.Diagnostics
open System.Threading
open System.Collections.Generic

type AppRunningLoggerState =
    { Connection     : SQLiteTest.DBConnection
      AppDefinitions : SQLiteTest.AppDefinition []
    }

let getProcesses = Process.GetProcesses : unit -> Process []

let canonicalize x = (CanonicalPath x).RawPath

let rec mainLoop (state : AppRunningLoggerState) =
    let appDefs = state.AppDefinitions
    let appDict =
        appDefs
        |> Array.map (fun x ->
            try
                Some (canonicalize x.Path, x)
            with | ex -> None
            )
        |> Array.choose id
        |> dict
    let procs = getProcesses ()
    let procPathPairs =
        procs
        |> Array.map (fun x ->
            try
                x.MainWindowHandle |> ignore
                let fileName = x.MainModule.FileName
                Some (canonicalize fileName, fileName) // (<Canonicalized path>, <`Process` path>)
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
    try
        let conn = SQLiteTest.initMainDB ()
        let appDefs = SQLiteTest.getAppDefinition conn
        mainLoop { Connection = conn; AppDefinitions = Array.ofSeq appDefs }
    with | ex -> printfn "%s" ex.StackTrace
    ()