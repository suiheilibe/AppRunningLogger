module AppRunningLogger.File1

open System.Diagnostics
open System.Threading
open System.Collections.Generic
open System.Runtime.InteropServices

type AppRunningLoggerState =
    { Connection     : SQLiteTest.DBConnection
      AppDefinitions : SQLiteTest.AppDefinition list
    }

type ProcessSub =
    { FileName : string
      Id       : int
    }

let getProcesses = Process.GetProcesses : unit -> Process []

let canonicalize x =
    try
        Some (CanonicalPath x).RawPath
    with
        ex -> None

let chooseListByFst xs =
    xs
    |> List.choose (function
        | (Some a, b) -> Some (a, b)
        | (None,   b) -> None
        )

let toDictWithOptionalKeys values keys =
    List.zip keys values
    |> chooseListByFst
    |> dict

let procToProcSub (procs : Process list) =
    procs
    |> List.map (fun x ->
        try
            Some { FileName = x.MainModule.FileName; Id = x.Id }
        with
            ex -> None
        )
    |> List.choose id

[<DllImport("user32.dll")>]
extern nativeint GetForegroundWindow();

[<DllImport("user32.dll", SetLastError=true)>]
extern uint32 GetWindowThreadProcessId(nativeint hWnd, uint32 *lpdwProcessId);

let rec mainLoop (state : AppRunningLoggerState) =
    let appDefs = state.AppDefinitions
    let appDict =
        appDefs
        |> List.map (fun x -> canonicalize x.Path)
        |> toDictWithOptionalKeys appDefs
    let procs = getProcesses() |> List.ofArray
    let procSubs = procToProcSub procs
    let procPairs=
        List.zip (procSubs |> List.map (fun x -> canonicalize x.FileName)) procSubs
        |> chooseListByFst
    let newAppDefs =
        procPairs
        |> List.filter (fun x -> fst x |> appDict.ContainsKey |> not)
        |> List.map (fun x -> SQLiteTest.AppDefinition(Path = (snd x).FileName))
    newAppDefs
    |> SQLiteTest.addAppDefinition state.Connection
    printfn "new: %d" newAppDefs.Length
    let nextAppDefs = List.append newAppDefs appDefs
    let hwnd = GetForegroundWindow()
    printfn "%d" hwnd
    let mutable pid = 0u
    printfn "%d" <| GetWindowThreadProcessId(hwnd, &&pid)
    printfn "%d" pid
    Thread.Sleep 1000
    mainLoop { Connection = state.Connection; AppDefinitions = nextAppDefs }
   
let startMainLoop() =
    try
        let conn = SQLiteTest.initMainDB()
        let appDefs = SQLiteTest.getAppDefinition conn |> List.ofSeq
        mainLoop { Connection = conn; AppDefinitions = appDefs }
    with | ex -> printfn "%s" ex.StackTrace
    ()