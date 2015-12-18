﻿module AppRunningTimer.File1

open System.Diagnostics
open System.Threading
open SQLite.Net
open SQLite.Net.Platform.Win32

let rules = [|{AppId = 0UL; NormalizedPath = NormalizedPath "F:\\GAMES\\mu100\\murasaki.exe"; Enabled = true}|];

let mainLoop =
    while true do
        let processes = Process.GetProcesses()
        for p in processes do
            try
                let npath = NormalizedPath p.MainModule.FileName
                ()
            with | e -> ()
            Thread.Sleep 1000

let sqliteTest = new SQLiteConnection(new SQLitePlatformWin32(),"test.db")