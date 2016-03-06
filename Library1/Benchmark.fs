module AppRunningLogger.Benchmark

open SQLite.Net.Attributes
open System

/// <summary>
/// PrimaryKey, Unique属性
/// </summary>
type TestTable_A () =
    [<PrimaryKey;Unique>]
    member val Id : int64 = 0L with get, set

/// <summary>
/// PrimaryKey属性
/// </summary>
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
    let len = 10000
    let seed = 1
    let conn = SQLiteTest.newSQLiteConnection "benchmark.db"
    let tables = [typeof<TestTable_A>; typeof<TestTable_B>; typeof<TestTable_C>; typeof<TestTable_D>];
    let createTable = conn.GetType().GetMethod("CreateTable", [|typeof<SQLite.Net.Interop.CreateFlags>|])
    let intList = randomList len seed
    let lists =
        tables
        |> List.map (fun x ->
            let createTableGeneric = createTable.MakeGenericMethod(x)
            //createTableGeneric.Invoke(conn, null) |> ignore
            intList
            |> List.map (fun v ->
                let pi = x.GetProperty("Id")
                let ctor = x.GetConstructor(Type.EmptyTypes)
                let obj = ctor.Invoke([||])
                pi.SetValue(obj, 1L)
                obj
            )
        )
    ()