module AppRunningLogger.Benchmark

let test () =
    let conn = SQLiteTest.newSQLiteConnection "benchmark.db"
    ()