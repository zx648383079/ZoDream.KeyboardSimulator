using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace ZoDream.Shared.Player.WinApi
{
    public static class WindowNativeMethods
    {
        public const int OPEN_PROCESS_ALL = 2035711;
        public const int PAGE_READWRITE = 4;
        public const int PROCESS_CREATE_THREAD = 2;
        public const int PROCESS_HEAP_ENTRY_BUSY = 4;
        public const int PROCESS_VM_OPERATION = 8;
        public const int PROCESS_VM_READ = 256;
        public const int PROCESS_VM_WRITE = 32;

        private const int PAGE_EXECUTE_READWRITE = 0x4;
        private const int MEM_COMMIT = 4096;
        private const int MEM_RELEASE = 0x8000;
        private const int MEM_DECOMMIT = 0x4000;
        private const int PROCESS_ALL_ACCESS = 0x1F0FFF;
        // WindowMessage 参数
        public const int WM_KEYDOWN = 0X100;
        public const int WM_KEYUP = 0X101;
        public const int WM_SYSCHAR = 0X106;
        public const int WM_SYSKEYUP = 0X105;
        public const int WM_SYSKEYDOWN = 0X104;
        public const int WM_CHAR = 0X102;
        public const int WM_DOWN = 0X0028;
        public const int WM_ENTER = 0X000D;
        public const int WM_CLOSE = 0X0010;
        public const int WM_QUIT = 0X0012;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        public const int SM_CXDRAG = 68;
        public const int SM_CYDRAG = 69;
        /// <summary>
        /// 获取屏幕宽度（像素）
        /// </summary>
        public const int SM_CXSCREEN = 0;
        /// <summary>
        /// 获取屏幕高度（像素）
        /// </summary>
        public const int SM_CYSCREEN = 1;


        /// <summary>
        /// 通过窗口的标题来查找窗口的句柄
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string? lpClassName, string lpWindowName);
        /// <summary>
        /// 在DLL库中的发送消息函数
        /// </summary>
        /// <param name="hWnd">目标窗口的句柄 </param>
        /// <param name="Msg">在这里是WM_COPYDATA</param>
        /// <param name="wParam">第一个消息参数</param>
        /// <param name="lParam">第二个消息参数</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, ref CopyDataStruct lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref CopyDataStruct lParam);

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);
        /// <summary>
        /// 获取焦点
        /// </summary>
        /// <param name="hwnd"></param>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern void SetForegroundWindow(IntPtr hwnd);
        /// <summary>
        /// 最大化窗口-3，最小化窗口-2，正常大小窗口-1；
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        /// <summary>
        /// 获取程序窗口尺寸，包括标题栏，左右下边框等
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref Rect lpRect);

        /// <summary>
        /// 获取程序窗口尺寸，去掉了标题栏，左右下边框等之后仅仅是个大小，返回值的左上角永远为0，0）
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetClientRect(IntPtr hWnd, ref Rect lpRect);

        [DllImport("user32.dll")]
        public extern static IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        public extern static int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        public extern static uint GetPixel(IntPtr hdc, int x, int y);

        /// <summary>
        /// 获取窗口句柄
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static IntPtr WindowFromPoint(Recorder.WinApi.PointStruct point);

        /// <summary>
        /// 获取窗口标题
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="sb"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static int GetWindowText(IntPtr hwnd, StringBuilder sb, int maxCount);
        /// <summary>
        /// 获取窗口类名
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="sb"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static int GetClassName(IntPtr hwnd, StringBuilder sb, int maxCount);
        /// <summary>
        /// 得到目标进程句柄的函数
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpdwProcessId"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static int GetWindowThreadProcessId(int hwnd, ref int lpdwProcessId);
        /// <summary>
        /// 得到目标进程句柄的函数
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpdwProcessId"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static int GetWindowThreadProcessId(IntPtr hwnd, ref int lpdwProcessId);

        /// <summary>
        /// 打开进程
        /// </summary>
        /// <param name="dwDesiredAccess"></param>
        /// <param name="bInheritHandle"></param>
        /// <param name="dwProcessId"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public extern static int OpenProcess(int dwDesiredAccess, int bInheritHandle, int dwProcessId);

        /// <summary>
        /// 打开进程
        /// </summary>
        /// <param name="dwDesiredAccess"></param>
        /// <param name="bInheritHandle"></param>
        /// <param name="dwProcessId"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public extern static IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

        /// <summary>
        /// 关闭句柄的函数 
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
        public static extern int CloseHandle(int hObject);

        /// <summary>
        /// 读内存
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll ")]
        public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, int size, out IntPtr lpNumberOfBytesWritten);

        /// <summary>
        /// 读内存
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll ")]
        public static extern int ReadProcessMemory(int hProcess, int lpBaseAddress, ref int buffer, int size, int lpNumberOfBytesWritten);

        /// <summary>
        /// 读内存
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll ")]
        public static extern int ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesWritten);

        /// <summary>
        /// 写内存
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, int size, out IntPtr lpNumberOfBytesWritten);

        /// <summary>
        /// 写内存
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpBaseAddress"></param>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="lpNumberOfBytesWritten"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern int WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesWritten);

        /// <summary>
        /// 创建线程
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpThreadAttributes"></param>
        /// <param name="dwStackSize"></param>
        /// <param name="lpStartAddress"></param>
        /// <param name="lpParameter"></param>
        /// <param name="dwCreationFlags"></param>
        /// <param name="lpThreadId"></param>
        /// <returns></returns>
        [DllImport("kernel32", EntryPoint = "CreateRemoteThread")]
        public static extern int CreateRemoteThread(int hProcess, int lpThreadAttributes, int dwStackSize, int lpStartAddress, int lpParameter, int dwCreationFlags, ref int lpThreadId);

        /// <summary>
        /// 开辟指定进程的内存空间  
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpAddress"></param>
        /// <param name="dwSize"></param>
        /// <param name="flAllocationType"></param>
        /// <param name="flProtect"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern int VirtualAllocEx(IntPtr hProcess, int lpAddress, int dwSize, short flAllocationType, short flProtect);

        /// <summary>
        /// 开辟指定进程的内存空间
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpAddress"></param>
        /// <param name="dwSize"></param>
        /// <param name="flAllocationType"></param>
        /// <param name="flProtect"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern int VirtualAllocEx(int hProcess, int lpAddress, int dwSize, int flAllocationType, int flProtect);

        /// <summary>
        /// 释放内存空间
        /// </summary>
        /// <param name="hProcess"></param>
        /// <param name="lpAddress"></param>
        /// <param name="dwSize"></param>
        /// <param name="flAllocationType"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern int VirtualFreeEx(int hProcess, int lpAddress, int dwSize, int flAllocationType);
        /// <summary>
        /// 捕获鼠标
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetCapture(IntPtr h);
        /// <summary>
        /// 释放鼠标
        /// </summary>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int index);

        private static Rectangle? _virtualScreen;
        public static Rectangle VirtualScreen
        {
            get
            {

                if (_virtualScreen == null)
                {
                    var width = GetSystemMetrics(WindowNativeMethods.SM_CXSCREEN);
                    var height = GetSystemMetrics(WindowNativeMethods.SM_CYSCREEN);
                    _virtualScreen = new Rectangle(0, 0,
                        width, height);
                }
                return (Rectangle)_virtualScreen;
            }
        }

        public static Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            var pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            return Color.FromArgb((int)(pixel & 0x000000FF), (int)(pixel & 0x0000FF00) >> 8, (int)(pixel & 0x00FF0000) >> 16);
        }
    }
}
