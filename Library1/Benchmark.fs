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

let run (conn : SQLite.Net.SQLiteConnection) (createTable : Reflection.MethodInfo) (tables : Type list) (data : int list) =
    tables
    |> List.map (fun x ->
        let createTableGeneric = createTable.MakeGenericMethod(x)
        createTableGeneric.Invoke(conn, [|CreateFlags.None|]) |> ignore
        let pi = x.GetProperty("Id")
        let ctor = x.GetConstructor(Type.EmptyTypes)
        let objList =
            data
            |> List.map (fun v ->
                let obj = ctor.Invoke([||])
                pi.SetValue(obj, (int64)v)
                obj
            )
        //objList |> List.iter (fun x -> pi.GetValue(x).ToString() |> printfn "%s")
        let startTime = DateTime.Now
        let result = objList |> conn.InsertAll // Constraint
        let endTime = DateTime.Now
        (startTime - endTime).ToString() |> printfn "%s"
    )

let test () =
    let fileName = "benchmark.db"
    IO.File.Delete fileName
    let conn = SQLiteTest.newSQLiteConnection fileName
    let createTable = conn.GetType().GetMethod("CreateTable", [|typeof<CreateFlags>|])
    //let tables = [typeof<TestTable_A>; typeof<TestTable_B>; typeof<TestTable_C>; typeof<TestTable_D>];
    let tables = [typeof<TestTable_A>];
    let len = 10000
    let intList = intList len
    let lists = run conn createTable tables intList
    ()
