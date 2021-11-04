using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ZoDream.Shared.Player.WinApi;
using ZoDream.Shared.Utils;

namespace ZoDream.Shared.Parser
{
    public static class Snapshot
    {

        private static Rectangle? _virtualScreen;
        public static Rectangle VirtualScreen
        {
            get
            {
                
                if (_virtualScreen == null)
                {
                    var width = WindowNativeMethods.GetSystemMetrics(WindowNativeMethods.SM_CXSCREEN);
                    var height = WindowNativeMethods.GetSystemMetrics(WindowNativeMethods.SM_CYSCREEN);
                    _virtualScreen = new Rectangle(0, 0,
                        width, height);
                }
                return (Rectangle)_virtualScreen;
            }
        }

        /// <summary>
        /// 获取屏幕截图
        /// </summary>
        /// <returns></returns>
        public static Bitmap? GetScreenSnapshot()
        {
            try
            {
                var rc = VirtualScreen;
                var bitmap = new Bitmap(rc.Width, rc.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                using (Graphics memoryGrahics = Graphics.FromImage(bitmap))
                {
                    memoryGrahics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy);
                }

                return bitmap;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public static string GetRect(int x, int y, int endX, int endY)
        {
            return GetRect(GetScreenSnapshot(), x, y, endX, endY);
        }

        public static string GetRect(Bitmap? bitmap, int x, int y, int endX, int endY)
        {
            if (bitmap == null)
            {
                return string.Empty;
            }
            var sb = new StringBuilder();
            GetRectLine(x, y, endX, endY, (i, j) =>
            {
                var color = bitmap.GetPixel(i, j);
                sb.Append(color.ToArgb());
            });
            return Str.MD5Encode(sb.ToString());
        }

        public static void GetRectLine(int x, int y, int endX, int endY, Action<int, int> action)
        {
            if (x == endX || y == endY)
            {
                action.Invoke(x, y);
            }
            var k = (double)(y - endY) / (x - endX);
            var step = x < endX ? 1 : -1;
            for (int i = x; i != endX; i+= step)
            {
                var j = k * (i - x) + y;
                action.Invoke(i, (int)(step > 0 ? Math.Floor(j) : Math.Ceiling(j)));
            }
        }
    }
}
