module SQLiteTest

open SQLite.Net
open SQLite.Net.Async
open SQLite.Net.Attributes
open SQLite.Net.Interop
open SQLite.Net.Platform.Win32
open System
open System.Threading.Tasks

type Test() =
    [<PrimaryKey;AutoIncrement>]
    member val Id : int = 0 with get, set
    [<Indexed>]
    member val Text : string = "" with get, set

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

let d = conn.CreateTableAsync<Test>().Result.Results.Item(typeof<Test>)

printfn "result: %d" d

Array.init 5 (fun _ -> conn.InsertAsync(new Test(Text = "This is a test string")) :> Task)
    |> Task.WaitAll

conn.Table<Test>().ToListAsync().Result
    |> Seq.iter (fun x ->
        printfn "Id: %d" x.Id
        printfn "Text: %s" x.Text
    )
