using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ZoDream.Shared.OS.WinApi.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RectStruct
    {
        public int Left; //最左坐标

        public int Top; //最上坐标

        public int Right; //最右坐标

        public int Bottom; //最下坐标
    }
}
