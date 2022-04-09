using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
using ZoDream.Shared.Parser;
using ZoDream.Shared.Player.WinApi;
using ZoDream.Shared.Recorder.WinApi;

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
            Shared.Recorder.WinApi.MouseNativeMethods.GetCursorPos(ref point);
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
            var rect = new Shared.Player.WinApi.Rect();
            WindowNativeMethods.GetWindowRect(hwd, ref rect);
            PointTb.Text = $"{rect.Left},{rect.Top},{rect.Right},{rect.Bottom}";
            SizeTb.Text = $"{rect.Right - rect.Left},{rect.Bottom - rect.Top}";
        }

        private void TabBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsLoading = (sender as TabControl)!.SelectedIndex == 0;
            LastPoint = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            DispatcherTimer.Tick += DispatcherTimer_Tick;
            DispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            if (!IsLoading)
            {
                return;
            }
            var point = Shared.Recorder.WinApi.MouseNativeMethods.GetMousePosition();
            if (LastPoint != null && LastPoint == point)
            {
                return;
            }
            LastPoint = point;
            var screenSnapshot = GetScreenSnapshot(point.X - 25, point.Y - 25, 50, 50);
            if (screenSnapshot != null)
            {
                var bmp = Utils.Image.ToBitmapSource(screenSnapshot);
                bmp.Freeze();
                PreviewImage.Source = bmp;
            }
            CurrentTb.Text = $"{point.X-BasePoint.X},{point.Y - BasePoint.Y}";
            var color = WindowNativeMethods.GetPixelColor(point.X, point.Y);
            ColorTb.Text = System.Drawing.ColorTranslator.ToHtml(color);
            RTb.Text = color.R.ToString();
            GTb.Text = color.G.ToString();
            BTb.Text = color.B.ToString();
        }

        public Bitmap? GetScreenSnapshot(int x, int y, int width, int height)
        {
            try
            {
                var bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                var memoryGrahics = Graphics.FromImage(bitmap);
                memoryGrahics.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);
                DrawCursorImageToScreenImage(ref memoryGrahics, x, y);
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
        private static void DrawCursorImageToScreenImage(ref Graphics g, int x, int y)
        {
            CursorInfo vCurosrInfo;
            vCurosrInfo.cbSize = Marshal.SizeOf(typeof(CursorInfo));
            Shared.Recorder.WinApi.MouseNativeMethods.GetCursorInfo(out vCurosrInfo);
            if ((vCurosrInfo.flags & Shared.Recorder.WinApi.MouseNativeMethods.CURSOR_SHOWING) !=
                Shared.Recorder.WinApi.MouseNativeMethods.CURSOR_SHOWING)
            {
                return;
            }
            var vCursor = new System.Windows.Forms.Cursor(vCurosrInfo.hCursor);
            var vRectangle = new System.Drawing.Rectangle(new System.Drawing.Point(vCurosrInfo.ptScreenPos.X - vCursor.HotSpot.X - x,
                vCurosrInfo.ptScreenPos.Y - vCursor.HotSpot.Y - y), vCursor.Size);
            vCursor.Draw(g, vRectangle);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer.Stop();
        }

        private void BaseTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            LastPoint = null;
            if (string.IsNullOrWhiteSpace(BaseTb.Text))
            {
                BasePoint.X = 0;
                BasePoint.Y = 0;
                return;
            }
            var args = BaseTb.Text.Split(new char[] { ',' });
            BasePoint.X = Convert.ToInt32(args[0].Trim());
            BasePoint.Y = args.Length > 1 ? Convert.ToInt32(args[1].Trim()) : 0;
        }
    }
}
