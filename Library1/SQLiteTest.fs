module SQLiteTest

open SQLite.Net
open SQLite.Net.Attributes
open SQLite.Net.Interop
open SQLite.Net.Platform.Win32
open System
open System.Threading.Tasks

type App () =
    [<PrimaryKey>]
    member val Id : uint64 = 0UL with get, set
    [<Indexed>]
    member val CanonicalPath : string = "" with get, set

type AppRunningLog () =
    static let dtUnder = DateTime.MinValue
    [<PrimaryKey;AutoIncrement>]
    member val Id : uint64 = 0UL with get, set
    [<NotNull;Indexed>]
    member val AppId : uint64 = 0UL with get, set
    member val Active : bool = false with get, set
    member val Begin : DateTime = dtUnder with get, set
    member val End : DateTime = dtUnder with get, set

let conn =
    new SQLiteConnection(
        new SQLitePlatformWin32(),
        "test.db",
        true
    )

let d = conn.CreateTable<App>()

printfn "result: %d" d

[1 .. 5]
    |> Seq.iter (fun _ ->
        conn.Insert(new App(CanonicalPath = "This is a test string")) |> ignore
    )

conn.Table<App>()
    |> Seq.iter (fun x ->
        printfn "Id: %d" x.Id
        printfn "Text: %s" x.CanonicalPath
    )
