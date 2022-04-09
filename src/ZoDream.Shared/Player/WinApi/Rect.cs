using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ZoDream.Shared.Player.WinApi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left; //最左坐标

        public int Top; //最上坐标

        public int Right; //最右坐标

        public int Bottom; //最下坐标
    }
}
