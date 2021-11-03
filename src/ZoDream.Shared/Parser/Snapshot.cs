using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ZoDream.Shared.Player.WinApi;

namespace ZoDream.Shared.Parser
{
    public static class Snapshot
    {

        private static Rectangle _virtualScreen;
        private static Rectangle VirtualScreen
        {
            get
            {
                if (_virtualScreen == null)
                {
                    _virtualScreen = new Rectangle(0, 0,
                    WindowNativeMethods.GetSystemMetrics(WindowNativeMethods.SM_CXSCREEN), 
                    WindowNativeMethods.GetSystemMetrics(WindowNativeMethods.SM_CYSCREEN));
                }
                return _virtualScreen;
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
            catch (Exception)
            {
            }
            return null;
        }
    }
}
