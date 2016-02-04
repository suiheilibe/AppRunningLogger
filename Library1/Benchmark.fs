module AppRunningLogger.Benchmark

open SQLite.Net.Attributes

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



let test () =
    let conn = SQLiteTest.newSQLiteConnection "benchmark.db"
    ()