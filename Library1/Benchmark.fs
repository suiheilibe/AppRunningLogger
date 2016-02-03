module AppRunningLogger.Benchmark

open SQLite.Net.Attributes

type TestTable () =
    [<PrimaryKey;Unique>]
    member val Id : int64 = 0L with get, set
    [<Unique>]
    member val Path : string = "" with get, set

let test () =
    let conn = SQLiteTest.newSQLiteConnection "benchmark.db"
    ()