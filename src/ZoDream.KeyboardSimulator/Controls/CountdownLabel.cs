using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ZoDream.KeyboardSimulator.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ZoDream.KeyboardSimulator.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ZoDream.KeyboardSimulator.Controls;assembly=ZoDream.KeyboardSimulator.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CountdownLabel/>
    ///
    /// </summary>
    public class CountdownLabel : Control
    {
        static CountdownLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CountdownLabel), new FrameworkPropertyMetadata(typeof(CountdownLabel)));
        }

        private DispatcherTimer? _timer;

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CountdownLabel), new PropertyMetadata(new CornerRadius(0)));



        public int Current
        {
            get { return (int)GetValue(CurrentProperty); }
            set { SetValue(CurrentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Current.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentProperty =
            DependencyProperty.Register("Current", typeof(int), typeof(CountdownLabel), new PropertyMetadata(0));




        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(int), typeof(CountdownLabel), new PropertyMetadata(0));

        /// <summary>
        /// 进度完成最大值事件
        /// </summary>
        public event RoutedEventHandler? Ended;

        public void Play()
        {
            Play(Max);
        }

        public void Play(int max)
        {
            if (max < 1)
            {
                Ended?.Invoke(this, new RoutedEventArgs());
                return;
            }
            Visibility = Visibility.Visible;
            UpdateValue(max);
            if (_timer != null)
            {
                _timer.Start();
                return;
            }
            _timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
            _timer.Tick += (sender, e) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (IsEnd())
                    {
                        Stop();
                        return;
                    }
                    UpdateValue();
                });
            };
            _timer.Start();
        }

        /// <summary>
        /// 停止计时
        /// </summary>
        public void Stop()
        {
            if (_timer == null)
            {
                return;
            }
            _timer.Stop();
            if (IsEnd())
            {
                Ended?.Invoke(this, new RoutedEventArgs());
            }
        }

        private bool IsEnd()
        {
            return Current <= 1;
        }

        private void UpdateValue()
        {
            UpdateValue(Current - 1);
        }

        private void UpdateValue(int v)
        {
            Current = v;
            VisualStateManager.GoToState(this, "Normal", false);
            VisualStateManager.GoToState(this, "Transfer", true);
        }
    }
}
