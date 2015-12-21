module SQLiteTest

open SQLite.Net
open SQLite.Net.Attributes
open SQLite.Net.Interop
open SQLite.Net.Platform.Win32
open System
open System.Threading.Tasks

type App () =
    [<PrimaryKey;AutoIncrement>]
    member val Id : int64 = 0L with get, set
    [<Indexed>]
    member val CanonicalPath : string = "" with get, set

type AppRunningLog () =
    static let dtUnder = DateTime.MinValue
    [<PrimaryKey;AutoIncrement>]
    member val Id : int64 = 0L with get, set
    [<NotNull;Indexed>]
    member val AppId : int64 = 0L with get, set
    member val Active : bool = false with get, set
    member val Begin : DateTime = dtUnder with get, set
    member val End : DateTime = dtUnder with get, set

let conn =
    new SQLiteConnection(
        new SQLitePlatformWin32(),
        "test.db",
        true
    )

conn.ExecuteScalar<string>("PRAGMA synchronous = NORMAL") |> ignore
conn.ExecuteScalar<string>("PRAGMA journal_mode = WAL") |> ignore

let d = conn.CreateTable<App>()

printfn "result: %d" d

[1 .. 5]
    |> Seq.iter (fun i ->
        conn.Insert(new App(CanonicalPath = "This is a test string")) |> ignore
    )

conn.Table<App>()
    |> Seq.iter (fun x ->
        printfn "Id: %d" x.Id
        printfn "Text: %s" x.CanonicalPath
    )
