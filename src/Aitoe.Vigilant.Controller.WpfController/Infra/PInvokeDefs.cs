using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Aitoe.Vigilant.Controller.WpfController.Infra
{
    internal static class PInvokeDefs
    {

        /// <summary>
        /// The WM_SIZING message is sent to a window that
        /// the user is resizing.  By processing this message,
        /// an application can monitor the size and position
        /// of the drag rectangle and, if needed, change its
        /// size or position. 
        /// </summary>
        internal const int WM_SIZING = 0x0214;

        /// <summary>
        /// The WM_SIZE message is sent to a window after its
        /// size has changed.
        /// </summary>
        internal const int WM_SIZE = 0x0005;

        internal static readonly int GWL_STYLE = (-16);
        internal const uint WS_CAPTION = 0x00C00000;
        internal const uint WS_THICKFRAME = 0x00040000;
        internal const uint WS_CLIPCHILDREN = 0x02000000;
        internal const uint WS_OVERLAPPED = 0x00000000;
        internal const int WS_CHILD = 0x40000000;
        internal const int SWP_NOZORDER = 0x0004;
        internal const int SWP_NOACTIVATE = 0x0010;
        internal const int WS_BORDER = 0x00800000;

        internal const int GWL_EXSTYLE = (-20);
        internal const uint WS_EX_APPWINDOW = 0x40000;

        internal const uint WM_SHOWWINDOW = 0x0018;
        internal const int SW_PARENTOPENING = 3;


        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32")]
        internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);


        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /*function to check for any errors that occurred*/
        [DllImport("kernel32.dll")]
        public static extern Int32 GetLastError();

        [DllImport("user32.dll")]
        private static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumWindowsProc ewp, int lParam);

        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern uint GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowText(IntPtr hWnd, StringBuilder lpString, uint nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        static bool IsApplicationWindow(IntPtr hWnd)
        {
            return (GetWindowLong(hWnd, GWL_EXSTYLE) & WS_EX_APPWINDOW) != 0;
        }

        public static IntPtr GetWindowHandle(int pid, string title)
        {
            var result = IntPtr.Zero;

            EnumWindowsProc enumerateHandle = delegate (IntPtr hWnd, int lParam)
            {
                int id;
                GetWindowThreadProcessId(hWnd, out id);

                if (pid == id)
                {
                    var clsName = new StringBuilder(256);
                    var hasClass = GetClassName(hWnd, clsName, 256);
                    if (hasClass)
                    {

                        var maxLength = (int)GetWindowTextLength(hWnd);
                        var builder = new StringBuilder(maxLength + 1);
                        GetWindowText(hWnd, builder, (uint)builder.Capacity);

                        var text = builder.ToString();
                        var className = clsName.ToString();

                        // There could be multiple handle associated with our pid, 
                        // so we return the first handle that satisfy:
                        // 1) the handle title/ caption matches our window title,
                        // 2) the window class name starts with HwndWrapper (WPF specific)
                        // 3) the window has WS_EX_APPWINDOW style

                        if (title == text && className.StartsWith("HwndWrapper") && IsApplicationWindow(hWnd))
                        {
                            result = hWnd;
                            return false;
                        }
                    }
                }
                return true;
            };

            EnumDesktopWindows(IntPtr.Zero, enumerateHandle, 0);

            return result;
        }

        /* place the following at the class level */
        [DllImport("kernel32.dll")]
        public static extern bool CreateProcess(
            string lpApplicationName,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        public struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }
    }
}
