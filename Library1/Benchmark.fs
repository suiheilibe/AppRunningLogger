module AppRunningLogger.Benchmark

open SQLite.Net.Attributes
open System

type TestTable_A () =
    [<PrimaryKey;Unique>]
    member val Id : int64 = 0L with get, set

type TestTable_B () =
    [<PrimaryKey>]
    member val Id : int64 = 0L with get, set

type TestTable_C () =
    [<Unique>]
    member val Id : int64 = 0L with get, set

type TestTable_D () =
    member val Id : int64 = 0L with get, set

let randomList n seed =
    let rnd = Random(seed)
    List.init n (fun i -> rnd.Next())

let test () =
    let len = Int32.MaxValue / 8
    let seed = 1
    let conn = SQLiteTest.newSQLiteConnection "benchmark.db"
    conn.CreateTable<TestTable_A>() |> ignore
    conn.CreateTable<TestTable_B>() |> ignore
    conn.CreateTable<TestTable_C>() |> ignore
    conn.CreateTable<TestTable_D>() |> ignore
    let intList = randomList len seed
    List.iter (printfn "%d") intList
    ()