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
    let newAppDefs =
        procPathPairs
        |> Array.filter (fun x -> fst x |> appDict.ContainsKey |> not)
        |> Array.map (fun x -> SQLiteTest.AppDefinition(Path = snd x))
    newAppDefs
    |> SQLiteTest.addAppDefinition state.Connection
    printfn "new: %d" newAppDefs.Length
    let nextAppDefs = Array.append appDefs newAppDefs
    Thread.Sleep 1000
    mainLoop { Connection = state.Connection; AppDefinitions = nextAppDefs }
   
let startMainLoop () =
    try
        let conn = SQLiteTest.initMainDB ()
        let appDefs = SQLiteTest.getAppDefinition conn |> Array.ofSeq
        mainLoop { Connection = conn; AppDefinitions = appDefs }
    with | ex -> printfn "%s" ex.StackTrace
    ()