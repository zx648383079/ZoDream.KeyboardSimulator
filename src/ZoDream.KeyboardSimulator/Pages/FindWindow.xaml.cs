using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using ZoDream.Shared.OS.WinApi;
using ZoDream.Shared.OS.WinApi.Models;

namespace ZoDream.KeyboardSimulator.Pages
{
    /// <summary>
    /// FindWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FindWindow : Window
    {
        public FindWindow()
        {
            InitializeComponent();
        }

        private DispatcherTimer DispatcherTimer;
        private bool IsLoading = true;
        private bool IsMouseUp = true;
        private System.Drawing.Point? LastPoint;
        private System.Drawing.Point BasePoint = new System.Drawing.Point(0, 0);

        private void DrogBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }
            IsMouseUp = false;
            Cursor = Cursors.Cross;
            DrogBtn.CaptureMouse();
        }

        private void DrogBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseUp)
            {
                return;
            }
            IsMouseUp = true;
            Cursor = Cursors.Arrow;
            DrogBtn.ReleaseMouseCapture();
            LoadWindowInfo();
        }

        private void DrogBtn_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseUp)
            {
                return;
            }
            
        }

        private void LoadWindowInfo()
        {
            var point = new PointStruct();
            Shared.OS.WinApi.MouseNativeMethods.GetCursorPos(ref point);
            var hwd = WindowNativeMethods.WindowFromPoint(point);
            HwdTb.Text = hwd.ToString();
            if (hwd == IntPtr.Zero)
            {
                return;
            }
            var sb = new StringBuilder(256);
            WindowNativeMethods.GetClassName(hwd, sb, 256);
            ClsTb.Text = sb.ToString();
            sb.Clear();
            WindowNativeMethods.GetWindowText(hwd, sb, 256);
            NameTb.Text = sb.ToString();
            var rect = new RectStruct();
            WindowNativeMethods.GetWindowRect(hwd, ref rect);
            WindowRectTb.SetIntArr(rect.Left, rect.Top, rect.Right, rect.Bottom);
            SizeTb.SetIntArr(rect.Right - rect.Left, rect.Bottom - rect.Top);
            rect = WindowNativeMethods.GetClientRect(hwd);
            ClientRectTb.SetIntArr(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        private void TabBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsLoading = (sender as TabControl)!.SelectedIndex == 0;
            LastPoint = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            DispatcherTimer.Tick += DispatcherTimer_Tick;
            DispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            if (!IsLoading)
            {
                return;
            }
            var point = Shared.OS.WinApi.MouseNativeMethods.GetMousePosition();
            if (LastPoint != null && LastPoint == point)
            {
                return;
            }
            LastPoint = point;
            var screenSnapshot = GetScreenSnapshot(point.X, point.Y, 20, 20, 150, 150);
            if (screenSnapshot != null)
            {
                var bmp = Utils.Image.ToBitmapSource(screenSnapshot);
                bmp.Freeze();
                PreviewImage.Source = bmp;
            }
            CurrentTb.SetIntArr(point.X-BasePoint.X, point.Y - BasePoint.Y);
            var color = WindowNativeMethods.GetPixelColor(point.X, point.Y);
            ColorTb.Text = System.Drawing.ColorTranslator.ToHtml(color);
            RTb.Text = color.R.ToString();
            GTb.Text = color.G.ToString();
            BTb.Text = color.B.ToString();
        }

        public Bitmap? GetScreenSnapshot(int cursorX, int cursorY, 
            int width, int height,
            int distWidth, int distHeight)
        {
            var x = cursorX - width / 2;
            var y = cursorY - height / 2;
            
            try
            {
                var bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (var memoryGrahics = Graphics.FromImage(bitmap))
                {
                    memoryGrahics.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);
                }
                var target = new Bitmap(distWidth, distHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                var g = Graphics.FromImage(target);
                g.CompositingMode = CompositingMode.SourceCopy;
                using (var ia = new ImageAttributes())
                {
                    ia.SetWrapMode(WrapMode.TileFlipXY);
                    g.DrawImage(bitmap, new System.Drawing.Rectangle(0, 0, distWidth, distHeight), 0, 0,
                        width, height, GraphicsUnit.Pixel, ia);
                }
                bitmap.Dispose();
                DrawCursorImageToScreenImage(ref g, a => (a - x) * distWidth / width, b => (b-y) * distHeight/ height);
                return target;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public Bitmap? GetScreenSnapshot(int x, int y, int width, int height)
        {
            try
            {
                var bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                var memoryGrahics = Graphics.FromImage(bitmap);
                memoryGrahics.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);
                DrawCursorImageToScreenImage(ref memoryGrahics, a => a - x, b => b - y);
                memoryGrahics.Dispose();
                return bitmap;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        /// <summary>
        /// 将鼠标指针形状绘制到屏幕截图上
        /// </summary>
        /// <param name="g"></param>
        private static void DrawCursorImageToScreenImage(ref Graphics g, Func<int, int> xFunc, Func<int, int> yFunc)
        {
            CursorInfo vCurosrInfo;
            vCurosrInfo.cbSize = Marshal.SizeOf(typeof(CursorInfo));
            Shared.OS.WinApi.MouseNativeMethods.GetCursorInfo(out vCurosrInfo);
            if ((vCurosrInfo.flags & Shared.OS.WinApi.MouseNativeMethods.CURSOR_SHOWING) !=
                Shared.OS.WinApi.MouseNativeMethods.CURSOR_SHOWING)
            {
                return;
            }
            var vCursor = new System.Windows.Forms.Cursor(vCurosrInfo.hCursor);
            var vRectangle = new System.Drawing.Rectangle(new System.Drawing.Point(
                xFunc(vCurosrInfo.ptScreenPos.X - vCursor.HotSpot.X),
                yFunc(vCurosrInfo.ptScreenPos.Y - vCursor.HotSpot.Y)), vCursor.Size);
            vCursor.Draw(g, vRectangle);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer.Stop();
        }

        private void BaseTb_ValueChanged(object sender, int[] _)
        {
            LastPoint = null;
            var p = BaseTb.GetIntArr(2);
            BasePoint.X = p[0];
            BasePoint.Y = p[1];
        }

        private void CvtBtn_Click(object sender, RoutedEventArgs e)
        {
            ConvertPoint();
        }

        private void ConvertPoint()
        {
            var oldRect = CvtOldTb.GetIntArr(4);
            var newRect = CvtNewTb.GetIntArr(4);
            var oldPoint = CvtOldPointTb.GetIntArr(2);
            CvtNewPointTb.SetIntArr(
                (oldRect[2] != 0 ? ((oldPoint[0] + oldRect[0]) / oldRect[2]) : 0) - newRect[0], 
                (oldRect[3] != 0 ? ((oldPoint[1] + oldRect[1]) / oldRect[3]) : 0) - newRect[1] 
            );
        }

        private void CvtOldPointTb_ValueChanged(object sender, int[] _)
        {
            ConvertPoint();
        }
    }
}
