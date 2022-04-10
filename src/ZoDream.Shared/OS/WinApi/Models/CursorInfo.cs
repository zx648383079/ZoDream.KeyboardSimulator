using System;
using System.Runtime.InteropServices;

namespace ZoDream.Shared.OS.WinApi.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CursorInfo
    {
        public int cbSize;
        public int flags;
        public IntPtr hCursor;
        public PointStruct ptScreenPos;
    }
}
