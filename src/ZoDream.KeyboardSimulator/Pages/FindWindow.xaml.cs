using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using ZoDream.Shared.Utils;

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
            MouseNativeMethods.GetCursorPos(ref point);
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
            var point = MouseNativeMethods.GetMousePosition();
            if (LastPoint != null && LastPoint == point)
            {
                return;
            }
            LastPoint = point;
            LoadPointColor(point.X, point.Y);
        }

        private void LoadPointColor(int x, int y, bool update = true)
        {
            var screenSnapshot = GetScreenSnapshot(x, y, 20, 20, 150, 150);
            if (screenSnapshot != null)
            {
                var bmp = Utils.Image.ToBitmapSource(screenSnapshot);
                bmp.Freeze();
                PreviewImage.Source = bmp;
            }
            if (update)
            {
                CurrentTb.SetIntArr(x - BasePoint.X, y - BasePoint.Y);
            }
            var color = WindowNativeMethods.GetPixelColor(x, y);
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
                var w = 1 * distWidth / width;
                g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Red, 1),
                    (cursorX - x) * distWidth / width - w/2,
                    (cursorY - y) * distHeight / height - w / 2,
                    w, w
                    );
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
            MouseNativeMethods.GetCursorInfo(out vCurosrInfo);
            if ((vCurosrInfo.flags & MouseNativeMethods.CURSOR_SHOWING) !=
                MouseNativeMethods.CURSOR_SHOWING)
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


        private void CvtOldPointTb_ValueChanged(object sender, int[] _)
        {
            ConvertPoint();
        }

        private void RectSpotBtn_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Thread.Sleep(500);
            var page = new SpotWindow();
            page.Spot += (_, x, y, endX, endY, e) =>
            {
                page.Close();
                Show();
                RectTb.SetIntArr(x, y, endX, endY);
                RectHashTb.Text = e;
            };
            page.Show();
            page.Activate();
        }

        private void AutoLoadTb_ValueChanged(object sender, bool value)
        {
            IsLoading = value;
        }

        private void CurrentTb_ValueChanged(object sender, int[] value)
        {
            if (IsLoading)
            {
                return;
            }
            var point = CurrentTb.GetIntArr(2);
            var basePoint = BaseTb.GetIntArr(2);
            LoadPointColor(point[0] + basePoint[0], point[1] + basePoint[1], false);
        }

        private void PreviewImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsLoading)
            {
                return;
            }
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }
            IsMouseUp = false;
            Cursor = Cursors.Cross;
            PreviewImage.CaptureMouse();
        }

        private void PreviewImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseUp)
            {
                return;
            }
            IsMouseUp = true;
            Cursor = Cursors.Arrow;
            PreviewImage.ReleaseMouseCapture();
            var point = new PointStruct();
            MouseNativeMethods.GetCursorPos(ref point);
            LoadPointColor(point.X, point.Y, true);
        }

        private void CvtAbsInput_ValueChanged(object sender, bool value)
        {
            CvtOldLabel.Text = value ? "原绝对坐标" : "原相对坐标";
            ConvertPoint();
        }

        private void CvtOldPointTb_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Link;
            e.Handled = true;
        }

        private void CvtOldPointTb_PreviewDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }
            var items = (IEnumerable<string>)e.Data.GetData(DataFormats.FileDrop);
            if (items == null)
            {
                return;
            }
            ConvertFile(items);
        }

        private async void ConvertFile(IEnumerable<string> items)
        {
            var oldRect = CvtOldTb.GetIntArr(4);
            var newRect = CvtNewTb.GetIntArr(4);
            var isAbs = CvtAbsInput.Value;
            foreach (var item in items)
            {
                await ConvertFile(item, oldRect, newRect, isAbs);
            }
        }

        private async Task ConvertFile(string fileName, int[] oldRect, int[] newRect,
            bool oldIsAbs = false)
        {
            if (!File.Exists(fileName))
            {
                return;
            }
            var content = await ZoDream.Language.Storage.File.ReadAsync(fileName);
            var res = Regex.Replace(content,
                @"(Move|MoveTo|MoveTween|GetPixelColor|IsPixelColor|IsRectColor)\(([^\(\)]+?)\)", match =>
            {
                if (!match.Success)
                {
                    return match.Value;
                }
                var args = match.Groups[2].Value.Split(',');
                if (args.Length < 2)
                {
                    return match.Value;
                }
                var res = ConvertPoint(Str.ToInt(args[0]), Str.ToInt(args[1]), oldRect, newRect, oldIsAbs);
                args[0] = res[0].ToString();
                args[1] = res[1].ToString();
                if (match.Groups[1].Value == "IsRectColor" && args.Length >= 4)
                {
                    res = ConvertPoint(Str.ToInt(args[2]), Str.ToInt(args[3]), oldRect, newRect, oldIsAbs);
                    args[2] = res[2].ToString();
                    args[3] = res[3].ToString();
                }
                return $"{match.Groups[1].Value}({string.Join(",", args)})";
            });
            App.Current.Dispatcher.Invoke(() =>
            {
                var picker = new Microsoft.Win32.SaveFileDialog
                {
                    Title = "选择保存路径",
                    Filter = "脚本文件|*.lua|所有文件|*.*",
                    FileName = System.IO.Path.GetFileName(fileName),
                };
                if (picker.ShowDialog() != true)
                {
                    return;
                }
                _ = ZoDream.Language.Storage.File.WriteAsync(picker.FileName, res);
            });
        }

        private async Task ConvertFile(string fileName)
        {
            await ConvertFile(fileName, CvtOldTb.GetIntArr(4), CvtNewTb.GetIntArr(4), CvtAbsInput.Value);
        }

        private void ConvertPoint()
        {
            var oldPoint = CvtOldPointTb.GetIntArr(2);
            if (CvtAbsInput.Value)
            {
                ConvertAbsPoint(oldPoint[0], oldPoint[1]);
            }
            else
            {
                ConvertPoint(oldPoint[0], oldPoint[1]);
            }
        }
        /// <summary>
        /// 转化相对坐标
        /// </summary>
        /// <param name="oldX"></param>
        /// <param name="oldY"></param>
        private void ConvertPoint(int oldX, int oldY)
        {
            var oldRect = CvtOldTb.GetIntArr(4);
            var newRect = CvtNewTb.GetIntArr(4);
            var res = ConvertPoint(oldX, oldY, oldRect, newRect);
            CvtNewPointTb.SetIntArr(res);
            CvtNewAbsTb.SetIntArr(res[0] + newRect[0], res[1] + newRect[1]);
        }

        private int[] ConvertPoint(int oldX, int oldY, int[] oldRect, int[] newRect, 
            bool oldIsAbs = false)
        {
            if (oldIsAbs)
            {
                oldX -= oldRect[0];
                oldY -= oldRect[1];
            }
            var x = oldRect[2] > 0 ? (oldX * newRect[2] / oldRect[2]) : 0;
            var y = oldRect[3] > 0 ? (oldY * newRect[3] / oldRect[3]) : 0;
            return new int[] { x, y };
        }
        /// <summary>
        /// 转化绝对坐标
        /// </summary>
        /// <param name="oldX"></param>
        /// <param name="oldY"></param>
        private void ConvertAbsPoint(int oldX, int oldY)
        {
            var oldRect = CvtOldTb.GetIntArr(4);
            ConvertPoint(oldX - oldRect[0], oldY - oldRect[1]);
        }
    }
}
