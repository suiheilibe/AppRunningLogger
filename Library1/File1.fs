module AppRunningLogger.File1

open System.Diagnostics
open System.Threading
open System.Collections.Generic

type AppRunningLoggerState =
    { Connection     : SQLiteTest.DBConnection
      AppDefinitions : SQLiteTest.AppDefinition list
    }

let getProcesses = Process.GetProcesses : unit -> Process []

let canonicalize x = (CanonicalPath x).RawPath

let canonicalizeList =
    List.map (fun x ->
        try
            Some (canonicalize x)
        with | ex -> None
        )

let toDictWithOptionalKeys values keys =
    List.zip keys values
    |> List.choose (function
        | (Some a, b) -> Some (a, b)
        | (None,   b) -> None
        )
    |> dict

let rec mainLoop (state : AppRunningLoggerState) =
    let appDefs = state.AppDefinitions
    let appDict =
        appDefs
        |> List.map (fun x -> x.Path)
        |> canonicalizeList
        |> toDictWithOptionalKeys appDefs
    let procs = getProcesses () |> List.ofArray
    let procPathPairs =
        procs
        |> List.map (fun x ->
            try
                x.MainWindowHandle |> ignore
                let fileName = x.MainModule.FileName
                Some (canonicalize fileName, fileName) // (<Canonicalized path>, <`Process` path>)
            with | ex -> None
            )
        |> List.choose id
    let newAppDefs =
        procPathPairs
        |> List.filter (fun x -> fst x |> appDict.ContainsKey |> not)
        |> List.map (fun x -> SQLiteTest.AppDefinition(Path = snd x))
    newAppDefs
    |> SQLiteTest.addAppDefinition state.Connection
    printfn "new: %d" newAppDefs.Length
    let nextAppDefs = List.append newAppDefs appDefs
    Thread.Sleep 1000
    mainLoop { Connection = state.Connection; AppDefinitions = nextAppDefs }
   
let startMainLoop () =
    try
        let conn = SQLiteTest.initMainDB ()
        let appDefs = SQLiteTest.getAppDefinition conn |> List.ofSeq
        mainLoop { Connection = conn; AppDefinitions = appDefs }
    with | ex -> printfn "%s" ex.StackTrace
    ()