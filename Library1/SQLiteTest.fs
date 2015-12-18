module SQLiteTest

open SQLite.Net
open SQLite.Net.Async
open SQLite.Net.Attributes
open SQLite.Net.Interop
open SQLite.Net.Platform.Win32
open System

type Test() =
  [<PrimaryKey;AutoIncrement>]
  member val Id : int = 0 with get, set
  [<Indexed>]
  member val Text : string = "" with get, set

let conn =
  new SQLiteAsyncConnection(
    System.Func<_>(fun () ->
      new SQLiteConnectionWithLock(
        new SQLitePlatformWin32(),
        new SQLiteConnectionString("test.db", true, null, null,
          Nullable(SQLiteOpenFlags.ReadWrite ||| SQLiteOpenFlags.Create ||| SQLiteOpenFlags.SharedCache)
        )
      )
    )
  )

let result =
  conn.CreateTableAsync<Test>()
  |> Async.AwaitTask
  |> Async.RunSynchronously

let d =
  result.Results.Item(typeof<Test>)

printfn "result: %d" d

Seq.init 5 (fun _ -> conn.InsertAsync(new Test(Text = "This is a test string")) |> Async.AwaitTask)
  |> Async.Parallel
  |> Async.Ignore
  |> Async.RunSynchronously

//Seq.init 5 (fun _ -> conn.InsertAsync(new Test(Text = "This is a test string")) |> Async.AwaitTask)
//  |> Seq.iter (fun x -> Async.RunSynchronously x |> ignore)

conn.Table<Test>().ToListAsync()
  |> Async.AwaitTask
  |> Async.RunSynchronously
  |> Seq.iter (fun x ->
    printfn "Id: %d" x.Id
    printfn "Text: %s" x.Text
  )
