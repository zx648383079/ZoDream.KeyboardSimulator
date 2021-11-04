﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZoDream.Shared.Parser;

namespace ZoDream.KeyboardSimulator.Pages
{
    /// <summary>
    /// SpotWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SpotWindow : Window
    {
        public SpotWindow()
        {
            InitializeComponent();
            screenSnapshot = Snapshot.GetScreenSnapshot();
            var bmp = ToBitmapSource(screenSnapshot);
            bmp.Freeze();
            this.MainBox.Background = new ImageBrush(bmp);
        }

        private System.Drawing.Bitmap screenSnapshot;

        public event SpotEventHandler Spot;

        /// <summary>
        /// 转化
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public BitmapSource ToBitmapSource(System.Drawing.Bitmap bmp)
        {
            if (bmp == null)
            {
                return null;
            }
            BitmapSource returnSource;
            try
            {
                returnSource = Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            catch
            {
                returnSource = null;
            }
            return returnSource;

        }

        internal System.Drawing.Bitmap CopyFromScreenSnapshot(Rect region)
        {
            // 根据缩放比例还原选中区域
            var scale = Snapshot.VirtualScreen.Width / MainBox.ActualWidth;
            var sourceRect = new System.Drawing.Rectangle((int)(region.X * scale), (int)(region.Y * scale),
                (int)(region.Width * scale), (int)(region.Height * scale));
            var destRect = new System.Drawing.Rectangle(0, 0, sourceRect.Width, sourceRect.Height);

            if (screenSnapshot != null)
            {
                var bitmap = new System.Drawing.Bitmap(destRect.Width, destRect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (var g = System.Drawing.Graphics.FromImage(bitmap))
                {
                    g.DrawImage(screenSnapshot, destRect, sourceRect, System.Drawing.GraphicsUnit.Pixel);
                }

                return bitmap;
            }
            return null;
        }

        internal Rect GetSourceRect(Rect region)
        {
            // 根据缩放比例还原选中区域
            var scale = Snapshot.VirtualScreen.Width / MainBox.ActualWidth;
            return new Rect((int)(region.X * scale), (int)(region.Y * scale),
                (int)(region.Width * scale), (int)(region.Height * scale));
        }

        private void MainBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var rect = new Rectangle();
            rect.StrokeThickness = 2;
            rect.Stroke = new SolidColorBrush(Colors.Red);
            var start = e.GetPosition(MainBox);
            Canvas.SetTop(rect, start.Y);
            Canvas.SetLeft(rect, start.X);
            MainBox.Children.Add(rect);
            this.On(p =>
            {
                var bound = ComputedBound(start, p);
                Canvas.SetTop(rect, bound.Y);
                Canvas.SetLeft(rect, bound.X);
                rect.Width = bound.Width;
                rect.Height = bound.Height;
            }, p =>
            {
                var bound = ComputedBound(start, p);
                Canvas.SetTop(rect, bound.Y);
                Canvas.SetLeft(rect, bound.X);
                rect.Width = bound.Width;
                rect.Height = bound.Height;
                if (bound.Width < 1 || bound.Height < 1)
                {
                    return;
                }
                //var img = new Image();
                //img.Source = ToBitmapSource(CopyFromScreenSnapshot(bound));
                //Canvas.SetTop(img, 200);
                //Canvas.SetLeft(img, 200);
                //MainBox.Children.Add(img);
                var sourceRect = GetSourceRect(bound);
                var x = (int)sourceRect.X;
                var y = (int)sourceRect.Y;
                var endX = (int)(sourceRect.X + sourceRect.Width);
                var endY = (int)(sourceRect.Y + sourceRect.Height);
                Spot.Invoke(this, x, y, endX, endY, Snapshot.GetRect(screenSnapshot, x, y, endX, endY));
            });
        }
        /// <summary>
        /// 根据两个坐标计算区域
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private Rect ComputedBound(Point start, Point end)
        {
            var bound = new Rect();
            if (start.X < end.X)
            {
                bound.X = start.X;
                bound.Width = end.X - start.X;
            }
            else
            {
                bound.X = end.X;
                bound.Width = start.X - end.X;
            }
            if (start.Y < end.Y)
            {
                bound.Y = start.Y;
                bound.Height = end.Y - start.Y;
            }
            else
            {
                bound.Y = end.Y;
                bound.Height = start.Y - end.Y;
            }
            return bound;
        }

        private void MainBox_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (onMoveFn == null)
            {
                return;
            }
            onMoveFn.Invoke(e.GetPosition(MainBox));
        }

        private void MainBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (onUpFn != null)
            {
                onUpFn.Invoke(e.GetPosition(MainBox));
            }
            onUpFn = null;
            onMoveFn = null;
        }

        private Action<Point> onMoveFn;
        private Action<Point> onUpFn;

        private void On(Action<Point> moveFn, Action<Point> upFn)
        {
            onMoveFn = moveFn;
            onUpFn = upFn;
        }
    }

    public delegate void SpotEventHandler(object sender, int x, int y, int endX, int endY, string e);
}
