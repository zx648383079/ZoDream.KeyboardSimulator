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
using ZoDream.Language.Storage;

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
            DependencyProperty.Register("Content", typeof(string), typeof(LineTextBox), new PropertyMetadata(string.Empty, (d, e) =>
            {
                (d as LineTextBox).SafeAsync((string)e.NewValue);
            }));

        private void SafeAsync(string val)
        {
            if (ContentTb.Text == val)
            {
                return;
            }
            ContentTb.Text = val;
        }


        public void InsertLine(string line)
        {
            var i = ContentTb.SelectionStart;
            var text = ContentTb.Text;
            if (i < 0)
            {
                i = text.Length;
            } else if (i > text.Length)
            {
                i = text.Length;
            }
            if (i > 0 && text[i - 1] != '\n' && text[i-1] != '\r')
            {
                line = '\n' + line;
            }
            Insert(line);
        }

        public void Insert(string val)
        {
            var i = ContentTb.SelectionStart;
            var text = ContentTb.Text;
            if (i > 0)
            {
                ContentTb.Text = text.Substring(0, i) + val + text.Substring(i);
                ContentTb.SelectionStart = i + val.Length;
            } else if (i == 0)
            {
                ContentTb.Text = val + text;
                ContentTb.SelectionStart = i + val.Length;
            } else
            {
                ContentTb.Text = text + val;
                ContentTb.SelectionStart = ContentTb.Text.Length;
            }
            Content = ContentTb.Text;
        }

        private void ContentTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }
            var sb = new StringBuilder();
            var count = textBox.LineCount;
            for (var i = 0; i < count; i++)
            {
                sb.AppendLine((i + 1).ToString());
            }
            LineTb.Text = sb.ToString();
            Content = ContentTb.Text;
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

        private async void ContentTb_PreviewDrop(object sender, DragEventArgs e)
        {
            if (IsReadOnly)
            {
                return;
            }
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }
            var items = (IEnumerable<string>)e.Data.GetData(DataFormats.FileDrop);
            if (items == null)
            {
                return;
            }
            OpenFile(items.First());
        }

        private void ContentTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                Insert("    ");
                return;
            }
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl)
                || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
            {
                if (e.KeyboardDevice.IsKeyDown(Key.S))
                {
                    e.Handled = true;
                    SaveAs();
                } else if (e.KeyboardDevice.IsKeyDown(Key.O))
                {
                    e.Handled = true;
                    OpenFile();
                }
            }
        }

        private void OpenFile()
        {
            var picker = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = true,
                Filter = "脚本文件|*.lua|所有文件|*.*",
                Title = "选择文件"
            };
            if (picker.ShowDialog() != true)
            {
                return;
            }
            OpenFile(picker.FileName);
        }

        public async void OpenFile(string fileName)
        {
            Content = await ZoDream.Language.Storage.File.ReadAsync(fileName);
        }

        private void SaveAs()
        {
            var picker = new Microsoft.Win32.SaveFileDialog
            {
                Title = "选择保存路径",
                Filter = "脚本文件|*.lua|所有文件|*.*",
                FileName = "未知脚本",
            };
            if (picker.ShowDialog() != true)
            {
                return;
            }
            SaveFile(picker.FileName);
        }

        public async void SaveFile(string fileName)
        {
            await ZoDream.Language.Storage.File.WriteAsync(fileName, ContentTb.Text);
        }
    }
}
