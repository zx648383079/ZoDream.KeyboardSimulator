using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ZoDream.Shared.Input;

namespace ZoDream.Shared.Player.WinApi
{
    public static class MouseNativeMethods
    {
        [DllImport("user32.dll")]
        private static extern int SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MouseEventMove = 0x0001; //移动鼠标
        public const int MouseEventLeftDown = 0x0002; //模拟鼠标左键按下
        public const int MouseEventLeftUp = 0x0004; //模拟鼠标左键抬起
        public const int MouseEventRightDown = 0x0008; //鼠标右键按下
        public const int MouseEventRightUp = 0x0010; //鼠标右键抬起
        public const int MouseEventMiddleDown = 0x0020; //鼠标中键按下 
        public const int MouseEventMiddleUp = 0x0040; //中键抬起
        public const int MouseEventWheel = 0x0800;
        public const int MouseEventAbsolute = 0x8000; //标示是否采用绝对坐标

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
            mouse_event(MouseEventLeftDown | MouseEventLeftUp, x, y, 0, 0);
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
            mouse_event(MouseEventWheel, 0, 0, delta, 0);
        }
    }
}
