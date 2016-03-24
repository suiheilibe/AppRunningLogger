module AppRunningLogger.Benchmark

open SQLite.Net.Attributes
open SQLite.Net.Interop
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

/// <summary>
/// Unique属性
/// </summary>
type TestTable_C () =
    [<Unique>]
    member val Id : int64 = 0L with get, set

/// <summary>
/// 属性なし
/// </summary>
type TestTable_D () =
    member val Id : int64 = 0L with get, set

/// <summary>
/// int値リストを得る
/// </summary>
/// <param name="n"></param>
let intList n =
    List.init n (fun i -> i)

let test () =
    let len = 10000
    let seed = 1
    let conn = SQLiteTest.newSQLiteConnection "benchmark.db"
    //let tables = [typeof<TestTable_A>; typeof<TestTable_B>; typeof<TestTable_C>; typeof<TestTable_D>];
    let tables = [typeof<TestTable_D>];
    let createTable = conn.GetType().GetMethod("CreateTable", [|typeof<CreateFlags>|])
    let intList = intList len
    let lists =
        tables
        |> List.map (fun x ->
            let createTableGeneric = createTable.MakeGenericMethod(x)
            createTableGeneric.Invoke(conn, [|CreateFlags.None|]) |> ignore
            let objList =
                intList
                |> List.map (fun v ->
                    let pi = x.GetProperty("Id")
                    let ctor = x.GetConstructor(Type.EmptyTypes)
                    let obj = ctor.Invoke([||])
                    pi.SetValue(obj, 1L)
                    obj
                )
            objList |> conn.InsertAll // Constraint
        )
    ()