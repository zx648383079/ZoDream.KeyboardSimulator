using System.Runtime.InteropServices;
using System.Drawing;

namespace ZoDream.Shared.Player.WinApi
{
    public static class MouseNativeMethods
    {
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
    }
}
