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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZoDream.KeyboardSimulator.Controls
{
    /// <summary>
    /// ConsoleContainer.xaml 的交互逻辑
    /// </summary>
    public partial class ConsoleContainer : UserControl
    {
        public ConsoleContainer()
        {
            InitializeComponent();
        }

        private int lastLineStart = 0;

        public void AppendLine(string line)
        {
            var val = ContentTb.Text;
            if (val.Length > 1000)
            {
                var i = val.IndexOf('\n', val.Length - 600);
                if (i < 0)
                {
                    val = "";
                }
                else
                {
                    val = val.Substring(i + 1);
                }
            }
            lastLineStart = val.Length;
            ContentTb.Text = val + line + "\n";
            ScrollToEnd();
        }

        public void AppendLine(string line, bool hasTime)
        {
            if (hasTime)
            {
                var now = DateTime.Now;
                AppendLine($"[{now.Hour}:{now.Minute}:{now.Second}]{line}");
            } else
            {
                AppendLine(line);
            }
        }

        public void ReplaceLine(string line)
        {
            ContentTb.Text = ContentTb.Text.Substring(0, lastLineStart) + line + "\n";
            ScrollToEnd();
        }

        public void ScrollToEnd()
        {
            ContentTb.ScrollToEnd();
        }

        public void Clear()
        {
            ContentTb.Text = string.Empty;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as MenuItem).Header)
            {
                case "清空":
                    Clear();
                    break;
                default:
                    break;
            }
        }
    }
}
