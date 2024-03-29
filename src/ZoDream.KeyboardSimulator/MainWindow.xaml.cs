﻿using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Xml;
using ZoDream.KeyboardSimulator.Pages;
using ZoDream.KeyboardSimulator.ViewModels;
using ZoDream.Language.Loggers;
using ZoDream.Shared.OS.WinApi.Helpers;

namespace ZoDream.KeyboardSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        
        public MainViewModel ViewModel = new();
        private CancellationTokenSource _cancellationTokenSource = new();

        #region 注册快捷键
        const string PLAY_TAG = "zre_play";
        const string STOP_TAG = "zre_stop";
        private HotKeyHelper? HotKey;
        private void BindHotKey()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            HotKey = new HotKeyHelper(hwnd);
            var hWndSource = HwndSource.FromHwnd(hwnd);
            // 添加处理程序
            hWndSource?.AddHook(WndProc);
            HotKey?.RegisterHotKey(PLAY_TAG, Shared.Input.Key.F10);
            HotKey?.RegisterHotKey(STOP_TAG, Shared.Input.Key.F11);
        }

        private void UnbindHotKey()
        {
            HotKey?.UnRegisterHotKey(PLAY_TAG);
            HotKey?.UnRegisterHotKey(STOP_TAG);
        }
        /// <summary>
        /// 窗体回调函数，接收所有窗体消息的事件处理函数
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="msg">消息</param>
        /// <param name="wideParam">附加参数1</param>
        /// <param name="longParam">附加参数2</param>
        /// <param name="handled">是否处理</param>
        /// <returns>返回句柄</returns>
        private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wideParam, IntPtr longParam, ref bool handled)
        {
            var name = HotKey?.ParseHook(msg, wideParam, longParam);
            if (name != null && string.IsNullOrEmpty(name))
            {
                switch (name)
                {
                    case PLAY_TAG:
                        TapPlay();
                        break;
                    case STOP_TAG:
                        Stop();
                        break;
                }
                handled = true;
            }
            return IntPtr.Zero;
        }

        #endregion

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFile(App.EnterFileName);
            // BindHotKey();
            await ViewModel.LoadOptionAsync();
            await ViewModel.Server.LoadAsync();
            LoadCode();
            UpdateLogBox();
            var logger = new EventLogger();
            logger.OnLog += (s, e) =>
            {
                ViewModel.ShowMessage(s);
                App.Current.Dispatcher.Invoke(() =>
                {
                    LogTb.AppendLine(s, ViewModel.Option.IsLogTime);
                });
            };
            ViewModel.Compiler.Logger = logger;
        }

        #region 代码提示
        private CompletionWindow? completionWindow;

        private void LoadCode()
        {
            CodeEditor.TextArea.TextEntered += TextArea_TextEntered;
            CodeEditor.TextArea.TextEntering += TextArea_TextEntering;
            CodeEditor.TextChanged += TextArea_TextChanged;
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lua.xshd");
            if (!File.Exists(fileName))
            {
                return;
            }
            using var reader = XmlReader.Create(fileName);
            var xshd = HighlightingLoader.LoadXshd(reader);
            CodeEditor.SyntaxHighlighting = HighlightingLoader.Load(xshd, HighlightingManager.Instance);
        }


        private void TextArea_TextChanged(object? sender, EventArgs e)
        {
            ViewModel.Server.LoadTextAsync(CodeEditor.Text);
        }
        private void TextArea_TextEntering(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
        }

        private void TextArea_TextEntered(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var items = ViewModel.Server.Find(e.Text);
            if (items.Count < 1)
            {
                return;
            }
            completionWindow = new CompletionWindow(CodeEditor.TextArea);
            // provide AvalonEdit with the data:
            var data = completionWindow.CompletionList.CompletionData;
            foreach (var item in items)
            {
                data.Add(item);
            }
            completionWindow.Show();
            completionWindow.Closed += delegate {
                completionWindow = null;
            };
        }
        #endregion


        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            TapPlay();
        }

        private void TapPlay()
        {
            if (string.IsNullOrWhiteSpace(CodeEditor.Text))
            {
                ViewModel.ShowMessage("脚本内容为空");
                return;
            }
            ViewModel.Paused = false;
            CountdownBtn.Play(ViewModel.Option.MaxDelay);
        }

        private void Play()
        {
            var script = CodeEditor.Text;
            if (string.IsNullOrWhiteSpace(script))
            {
                ViewModel.ShowMessage("脚本内容为空");
                return;
            }
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => {
                try
                {
                    ViewModel.Compiler.Compile(script, _cancellationTokenSource.Token);
                }
                catch (System.Exception)
                {
                    // ViewModel.ShowMessage(ex.Message);
                } finally
                {
                    ViewModel.Paused = true;
                }
            }, _cancellationTokenSource.Token);
        }


        private void RecordBtn_Click(object sender, RoutedEventArgs e)
        {
            var model = new RecordViewModel(ViewModel.Option);
            var page = new PlayWindow(model);
            page.Show();
            model.OnFinish += (_, e) =>
            {
                if (MessageBox.Show("是否保存记录？将追加到脚本中？", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    CodeEditor.Document.Insert(CodeEditor.TextArea.Caret.Offset, e);
                }
            };
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            ViewModel.Paused = true;
            _cancellationTokenSource.Cancel();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Dispose();
            // UnbindHotKey();
        }

        private void FindBtn_Click(object sender, RoutedEventArgs e)
        {
            var page = new FindWindow();
            page.Show();
        }

        private void OptionBtn_Click(object sender, RoutedEventArgs e)
        {
            var model = new SettingViewModel(ViewModel.Option);
            var page = new SettingWindow(model);
            page.Show();
            model.PropertyChanged += (_, e) =>
            {
                ViewModel.Option = model.ToOption();
                UpdateLogBox();
                _= ViewModel.SaveOptionAsync();
            };
        }

        private void UpdateLogBox()
        {
            LogTb.Visibility = ViewModel.Option.IsLogVisible ? Visibility.Visible : Visibility.Collapsed;
            if (ViewModel.Option.IsLogVisible)
            {
                LogTb.Height = ActualHeight / 3;
            }
        }

        private void CountdownBtn_Ended(object sender, RoutedEventArgs e)
        {
            CountdownBtn.Visibility = Visibility.Collapsed;
            if (ViewModel.Paused)
            {
                return;
            }
            Play();
        }

        private void CommandBinding_SaveFile(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var picker = new Microsoft.Win32.SaveFileDialog
            {
                Title = "选择保存路径",
                Filter = "脚本文件|*.lua|所有文件|*.*",
                FileName = string.IsNullOrEmpty(ViewModel.FileName) ? "未知脚本" : Path.GetFileName(ViewModel.FileName),
            };
            if (picker.ShowDialog() != true)
            {
                return;
            }
            SaveFile(picker.FileName);
        }

        private void CommandBinding_OpenFile(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (!ViewModel.Paused)
            {
                return;
            }
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

        private void CodeEditor_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (ViewModel.Paused)
            {
                e.Effects = DragDropEffects.Link;
                e.Handled = true;
            }
        }

        private void CodeEditor_PreviewDrop(object sender, DragEventArgs e)
        {
            if (!ViewModel.Paused)
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

        public void OpenFile(string fileName)
        {
            ViewModel.FileName = fileName;
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }
            CodeEditor.Load(fileName);
        }
        public void SaveFile(string fileName)
        {
            ViewModel.FileName = fileName;
            CodeEditor.Save(fileName);
        }
    }
}
