using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ZoDream.Shared.Recorder.WinApi
{
    /// <summary>
    ///     The Point structure defines the X- and Y- coordinates of a point.
    /// </summary>
    /// <remarks>
    ///     http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdi/rectangl_0tiq.asp
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct PointStruct
    {
        /// <summary>
        ///     Specifies the X-coordinate of the point.
        /// </summary>
        public int X;

        /// <summary>
        ///     Specifies the Y-coordinate of the point.
        /// </summary>
        public int Y;

        public PointStruct(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
