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
using ZoDream.Shared.Storage;

namespace ZoDream.KeyboardSimulator.Controls
{
    /// <summary>
    /// LineTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class LineTextBox : UserControl
    {
        public LineTextBox()
        {
            InitializeComponent();
        }



        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(LineTextBox), new PropertyMetadata(false));




        public new string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(string), typeof(LineTextBox), new PropertyMetadata(string.Empty));




        private void ContentTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }
            var sb = new StringBuilder();
            for (var i = 0; i < textBox.LineCount; i++)
            {
                sb.AppendLine((i + 1).ToString());
            }
            LineTb.Text = sb.ToString();
        }

        private void ContentTb_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (IsReadOnly)
            {
                return;
            }
            e.Effects = DragDropEffects.Link;
            e.Handled = true;
        }

        private void ContentTb_PreviewDrop(object sender, DragEventArgs e)
        {
            if (IsReadOnly)
            {
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var file = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                if (string.IsNullOrEmpty(file))
                {
                    return;
                }
                ContentTb.Text = Open.Read(file);
            }
        }
    }
}
