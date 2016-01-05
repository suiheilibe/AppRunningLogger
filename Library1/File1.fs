module AppRunningLogger.File1

open System.Diagnostics
open System.Threading
open System.Collections.Generic

type private AppRunningLoggerState =
    { Connection     : SQLiteTest.DBConnection
      AppDefinitions : SQLiteTest.AppDefinition list
    }

let private getProcesses = Process.GetProcesses : unit -> Process []

let private canonicalize x =
    try
        Some (CanonicalPath x).RawPath
    with
        ex -> None

let private chooseListByFst xs =
    xs
    |> List.choose (function
        | (Some a, b) -> Some (a, b)
        | (None,   b) -> None
        )

let private toDictWithOptionalKeys values keys =
    List.zip keys values
    |> chooseListByFst
    |> dict

let rec private mainLoop (state : AppRunningLoggerState) =
    let appDefs = state.AppDefinitions
    let appDict =
        appDefs
        |> List.map (fun x -> canonicalize x.Path)
        |> toDictWithOptionalKeys appDefs
    let procs = getProcesses() |> List.ofArray
    let procPaths =
        procs
        |> List.map (fun x ->
            try
                x.MainWindowHandle |> ignore
                Some x.MainModule.FileName
            with
                ex -> None
            )
        |> List.choose id
    let procPathPairs =
        List.zip (procPaths |> List.map canonicalize) procPaths
        |> chooseListByFst
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
   
let startMainLoop() =
    try
        let conn = SQLiteTest.initMainDB()
        let appDefs = SQLiteTest.getAppDefinition conn |> List.ofSeq
        mainLoop { Connection = conn; AppDefinitions = appDefs }
    with | ex -> printfn "%s" ex.StackTrace
    ()