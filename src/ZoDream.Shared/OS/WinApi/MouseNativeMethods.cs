﻿using System.Runtime.InteropServices;
using System.Drawing;
using ZoDream.Shared.OS.WinApi.Models;

namespace ZoDream.Shared.OS.WinApi
{
    public static class MouseNativeMethods
    {
        public const int CURSOR_SHOWING = 0x00000001;
        /// <summary>
        ///     The GetDoubleClickTime function retrieves the current double-click time for the mouse. A double-click is a series
        ///     of two clicks of the
        ///     mouse button, the second occurring within a specified time after the first. The double-click time is the maximum
        ///     number of
        ///     milliseconds that may occur between the first and second click of a double-click.
        /// </summary>
        /// <returns>
        ///     The return value specifies the current double-click time, in milliseconds.
        /// </returns>
        /// <remarks>
        ///     http://msdn.microsoft.com/en-us/library/ms646258(VS.85).aspx
        /// </remarks>
        [DllImport("user32")]
        public static extern int GetDoubleClickTime();


        [DllImport("user32.dll")]
        public static extern int GetCursorPos(ref PointStruct lpPoint);

        [DllImport("user32.dll")]
        public static extern bool GetCursorInfo(out CursorInfo pci);

        [DllImport("user32.dll")]
        private static extern int SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public static void MoveTo(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void MoveTo(Point p)
        {
            MoveTo(p.X, p.Y);
        }


        public static void Click(int x, int y)
        {
            mouse_event((int)(MouseFlag.LeftDown | MouseFlag.LeftUp), x, y, 0, 0);
        }

        public static void Click(Point p)
        {
            Click(p.X, p.Y);
        }

        public static void DoubleClick(int x, int y)
        {
            Click(x, y);
            Click(x, y);
        }

        public static void Wheel(int delta)
        {
            mouse_event((int)MouseFlag.VerticalWheel, 0, 0, delta, 0);
        }
        public static Point GetMousePosition()
        {
            var point = new PointStruct();
            GetCursorPos(ref point);
            return new Point(point.X, point.Y);
        }
    }
}
