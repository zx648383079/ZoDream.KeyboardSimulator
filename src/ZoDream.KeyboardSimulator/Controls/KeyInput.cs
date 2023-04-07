using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    ///     <MyNamespace:KeyInput/>
    ///
    /// </summary>
    /// 
    [TemplatePart(Name = KeyTbName, Type = typeof(TextBox))]
    [TemplatePart(Name = ClearBtnName, Type = typeof(Button))]
    public class KeyInput : Control
    {
        const string KeyTbName = "PART_KeyTb";
        const string ClearBtnName = "PART_ClearBtn";
        static KeyInput()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KeyInput), new FrameworkPropertyMetadata(typeof(KeyInput)));
        }



        public string Keys {
            get { return (string)GetValue(KeysProperty); }
            set { SetValue(KeysProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Keys.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeysProperty =
            DependencyProperty.Register("Keys", typeof(string), typeof(KeyInput), new PropertyMetadata(string.Empty));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild(KeyTbName) is TextBox tb)
            {
                tb.PreviewKeyDown += Tb_KeyDown;
            }
            if (GetTemplateChild(ClearBtnName) is Button btn)
            {
                btn.Click += Btn_Click;
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Keys = string.Empty;
        }

        private void Tb_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            var key = e.Key == Key.ImeProcessed ? e.ImeProcessedKey.ToString() : e.Key.ToString();
            if (string.IsNullOrWhiteSpace(Keys))
            {
                Keys = key;
                ((TextBox)sender).SelectionStart = Keys.Length;
                return;
            }
            if (HasKey(Keys, key))
            {
                return;
            }
            Keys = $"{Keys}+{key}";
            ((TextBox)sender).SelectionStart = Keys.Length;
        }

        private bool HasKey(string keys, string key)
        {
            var i = keys.IndexOf(key);
            if (i < 0)
            {
                return false;
            }
            var j = i + key.Length;
            if (j >= keys.Length)
            {
                return true;
            }
            if (keys[j] == '+')
            {
                return true;
            }
            return false;
        }
    }
}
