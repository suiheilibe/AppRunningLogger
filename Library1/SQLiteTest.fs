module SQLiteTest

open SQLite.Net
open SQLite.Net.Async
open SQLite.Net.Attributes
open SQLite.Net.Interop
open SQLite.Net.Platform.Win32
open System
open System.Threading.Tasks

type App () =
    [<PrimaryKey;AutoIncrement>]
    member val Id : uint64 = 0UL with get, set
    [<Indexed>]
    member val CanonicalPath : string = "" with get, set

type AppRunningLog () =
    static let dtUnder = DateTime.MinValue
    static let tsUnder = TimeSpan.MinValue
    [<PrimaryKey;AutoIncrement>]
    member val Id : uint64 = 0UL with get, set
    [<NotNull;Indexed>]
    member val AppId : uint64 = 0UL with get, set
    member val Active : bool = false with get, set
    member val Begin : DateTime = dtUnder with get, set
    member val Period : TimeSpan = tsUnder with get, set

let conn =
    new SQLiteAsyncConnection(
        Func<_>(fun () ->
            new SQLiteConnectionWithLock(
                new SQLitePlatformWin32(),
                new SQLiteConnectionString("test.db", true, null, null,
                    Nullable(SQLiteOpenFlags.ReadWrite ||| SQLiteOpenFlags.Create ||| SQLiteOpenFlags.SharedCache)
                )
            )
        )
    )

let d = conn.CreateTableAsync<App>().Result.Results.Item(typeof<App>)

printfn "result: %d" d

Array.init 5 (fun _ -> conn.InsertAsync(new App(CanonicalPath = "This is a test string")) :> Task)
    |> Task.WaitAll

conn.Table<App>().ToListAsync().Result
    |> Seq.iter (fun x ->
        printfn "Id: %d" x.Id
        printfn "Text: %s" x.CanonicalPath
    )
