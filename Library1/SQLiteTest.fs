module AppRunningLogger.SQLiteTest

open SQLite.Net
open SQLite.Net.Attributes
open SQLite.Net.Interop
open SQLite.Net.Platform.Win32
open System
open System.Collections.Generic

type AppDefinition () =
    [<PrimaryKey;Unique>]
    member val Id : int64 = 0L with get, set
    [<Unique>]
    member val Path : string = "" with get, set
    interface IComparable<AppDefinition> with
        member this.CompareTo(o : AppDefinition) = this.Id.CompareTo(o.Id)

type AppRunningLog () =
    static let dtUnder = DateTime.MinValue
    [<PrimaryKey;AutoIncrement>]
    member val Id : int64 = 0L with get, set
    [<NotNull;Indexed>]
    member val AppId : int64 = 0L with get, set
    member val Active : bool = false with get, set
    member val Begin : DateTime = dtUnder with get, set
    member val End : DateTime = dtUnder with get, set

type DBConnection = SQLiteConnection

let newSQLiteConnection dbName =
    new SQLiteConnection(
            new SQLitePlatformWin32(),
            dbName,
            true
        )

let initialSettings = function
    | (conn : SQLiteConnection) ->
        conn.ExecuteScalar<string> "PRAGMA synchronous = NORMAL" |> ignore
        conn.ExecuteScalar<string> "PRAGMA journal_mode = WAL" |> ignore
        ()

/// <summary>
/// 
/// </summary>
let initMainDB () =
    let conn = newSQLiteConnection "main.db"
    initialSettings conn
    conn.CreateTable<AppDefinition>() |> ignore
    conn.CreateTable<AppRunningLog>() |> ignore
    conn

let getAppDefinition (conn : DBConnection) =
    conn.Table<AppDefinition>()

let addAppDefinition (conn : DBConnection) (appDefs : IEnumerable<AppDefinition>) =
    conn.InsertAll appDefs |> ignore

let getAppRunningLog (conn : DBConnection) minId =
    conn.Table<AppRunningLog>().Where(fun x -> x.Id >= minId)

