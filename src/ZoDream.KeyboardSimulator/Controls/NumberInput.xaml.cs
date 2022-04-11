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
using ZoDream.Shared.Utils;

namespace ZoDream.KeyboardSimulator.Controls
{
    /// <summary>
    /// NumberInput.xaml 的交互逻辑
    /// </summary>
    public partial class NumberInput : UserControl
    {
        public NumberInput()
        {
            InitializeComponent();
        }



        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(int), typeof(NumberInput), new PropertyMetadata(0));




        public int Min
        {
            get { return (int)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Min.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(int), typeof(NumberInput), new PropertyMetadata(0));




        public uint Step
        {
            get { return (uint)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Step.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(uint), typeof(NumberInput), new PropertyMetadata((uint)1));



        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(NumberInput), new PropertyMetadata(0, OnValueChanged));

        public event ValueChangedEventHandler<int>? ValueChanged;
        private CancellationTokenSource TokenSource = new();

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NumberInput)?.UpdateValue(e.NewValue);
        }

        private void UpdateValue(object val)
        {
            var v = val.ToString();
            if (v ==null || NumberTb.Text.Trim() == v)
            {
                return;
            }
            NumberTb.Text = v;
        }

        private void MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            var val = Value - Step;
            if (val < Min)
            {
                val = Min;
            }
            NumberTb.Text = val.ToString();
            Value = Convert.ToInt32(val);
            ValueChanged?.Invoke(this, Value);
        }

        private void PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            var val = Value + Step;
            if (Max > 0 && val > Max)
            {
                val = Max;
            }
            NumberTb.Text = val.ToString();
            Value = Convert.ToInt32(val);
            ValueChanged?.Invoke(this, Value);
        }

        private void NumberTb_TextChanged(object sender, TextChangedEventArgs e)
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
                    var oldVal = Value;
                    var val = Str.ToInt((sender as TextBox)!.Text);
                    if (val < Min)
                    {
                        val = Min;
                    }
                    else if (Max > 0 && val > Max)
                    {
                        val = Max;
                    }
                    NumberTb.Text = val.ToString();
                    Value = val;
                    ValueChanged?.Invoke(this, Value);
                });
            }, TokenSource.Token);
        }
    }
}
