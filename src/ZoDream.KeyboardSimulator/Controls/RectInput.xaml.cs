using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using ZoDream.KeyboardSimulator.Events;

namespace ZoDream.KeyboardSimulator.Controls
{
    /// <summary>
    /// RectInput.xaml 的交互逻辑
    /// </summary>
    public partial class RectInput : UserControl
    {
        public RectInput()
        {
            InitializeComponent();
        }



        public int[] Value
        {
            get { return (int[])GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int[]), typeof(RectInput), new PropertyMetadata(Array.Empty<int>(), (d, e) =>
            {
                (d as RectInput)!.UpdateValue((int[])e.NewValue, false);
            }));

        public int Column
        {
            get { return (int)GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Column.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnProperty =
            DependencyProperty.Register("Column", typeof(int), typeof(RectInput), new PropertyMetadata(2));



        public string Separator
        {
            get { return (string)GetValue(SeparatorProperty); }
            set { SetValue(SeparatorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Separator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SeparatorProperty =
            DependencyProperty.Register("Separator", typeof(string), typeof(RectInput), new PropertyMetadata(","));




        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(RectInput), new PropertyMetadata(false));



        public event ValueChangedEventHandler<int[]>? ValueChanged;
        private CancellationTokenSource TokenSource = new();

        public void SetIntArr(params object[] items)
        {
            UpdateValue(items.Select(i =>
            {
                if (i is int)
                {
                    return (int)i;
                }
                return Convert.ToInt32(i);
            }).ToArray());
        }

        public int[] GetIntArr(int[]? items, int count)
        {
            var real = new int[count];
            if (items != null)
            {
                for (int i = 0; i < Column; i++)
                {
                    if (items.Length > i)
                    {
                        real[i] = items[i];
                    }
                }
            }
            return real;
        }

        public int[] GetIntArr(int count)
        {
            return GetIntArr(Value, count);
        }

        public int[] GetIntArr()
        {
            return GetIntArr(Value, Column);
        }

        private void UpdateValue(int[]? items)
        {
            UpdateValue(items, true);
        }

        private void UpdateValue(int[]? items, bool update)
        {
            var real = GetIntArr(items, Column);
            if (update)
            {
                Value = real;
            }
            ValueTb.Text = string.Join(Separator, real);
            ValueTb.SelectionStart = ValueTb.Text.Length;
        }


        private void ValueTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            TokenSource.Cancel();
            TokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                if (TokenSource.IsCancellationRequested)
                {
                    return;
                }
                App.Current.Dispatcher.Invoke(() =>
                {
                    var items = ValueTb.Text.Split(new string[] { Separator }, StringSplitOptions.None)
                    .Select(i => string.IsNullOrWhiteSpace(i) ? 0 : Convert.ToInt32(i.Trim())).ToArray();
                    UpdateValue(items);
                    ValueChanged?.Invoke(this, Value);
                });
            }, TokenSource.Token);
        }

        private void ValueTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                e.Handled = true;
                if (ValueTb.Text.Split(new string[] { Separator }, StringSplitOptions.None).Length >= Column)
                {
                    return;
                }
                var i = ValueTb.SelectionStart;
                if (i < 0)
                {
                    ValueTb.Text += Separator;
                    ValueTb.SelectionStart = i;
                } else
                {
                    ValueTb.Text = ValueTb.Text.Substring(0, i) + Separator + ValueTb.Text.Substring(i);
                    ValueTb.SelectionStart = i + Separator.Length;
                }
            }
        }
    }
}
