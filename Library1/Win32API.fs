module AppRunningLogger.Win32API

open System.Runtime.InteropServices

[<DllImport("user32.dll")>]
extern nativeint GetForegroundWindow();

[<DllImport("user32.dll", SetLastError=true)>]
extern uint32 GetWindowThreadProcessId(nativeint hWnd, uint32& lpdwProcessId);