﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ZoDream.Shared.Input;

namespace ZoDream.Shared.Recorder.WinApi
{
    public static class MouseNativeMethods
    {
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
        internal static extern int GetDoubleClickTime();


        [DllImport("user32.dll")]
        private static extern int GetCursorPos(ref PointStruct lpPoint);

        public static Point GetMousePosition()
        {
            var point = new PointStruct();
            GetCursorPos(ref point);
            return new Point(point.X, point.Y);
        }
    }
}
