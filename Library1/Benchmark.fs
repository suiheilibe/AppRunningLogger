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
    let tables = [typeof<TestTable_A>; typeof<TestTable_B>; typeof<TestTable_C>; typeof<TestTable_D>];
    tables
    |> List.iter (fun x ->
        let createTable = conn.GetType().GetMethod("CreateTable", Type.EmptyTypes)
        let createTableGeneric = createTable.MakeGenericMethod(x)
        createTableGeneric.Invoke(conn, null) |> ignore
    )
    let intList = randomList len seed
    List.iter (printfn "%d") intList
    ()