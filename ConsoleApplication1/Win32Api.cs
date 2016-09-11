using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace AppRunningLogger
{
    class Win32Api
    {
        [DllImport("user32.dll")]
        extern static IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        extern static UInt32 GetWindowThreadProcessId(IntPtr hWnd, ref UInt32 lpdwProcessId);
    }
}
