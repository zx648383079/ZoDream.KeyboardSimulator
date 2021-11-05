﻿using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZoDream.KeyboardSimulator.Pages;
using ZoDream.KeyboardSimulator.ViewModels;
using ZoDream.Shared.Storage;

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
            ViewModel.Recorder.OnKey += Recorder_OnKey;
            ViewModel.Recorder.OnMouse += Recorder_OnMouse;
            ViewModel.Compiler.TokenChanged += Compiler_TokenChanged;
        }

        public MainViewModel ViewModel = new MainViewModel();
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private void Recorder_OnMouse(object sender, Shared.Input.MouseEventArgs e)
        {
            ViewModel.Generator.Add(e);
            AsyncOuput();
        }

        /// <summary>
        /// 延迟显示
        /// </summary>
        private void AsyncOuput()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Factory.StartNew(() => {
                Thread.Sleep(500);
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }
                this.Dispatcher.Invoke(() =>
                {
                    ContentTb.Content = ViewModel.Generator.ToString();
                });
            }, _cancellationTokenSource.Token);
            
        }

        private void Recorder_OnKey(object sender, Shared.Input.KeyEventArgs e)
        {
            ViewModel.Generator.Add(e);
            AsyncOuput();
        }

        private void Compiler_TokenChanged(object sender, Shared.Parser.TokenStmt value)
        {
            ViewModel.ShowMessage($"Run Line: {value.Line}");
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Paused = false;
            _cancellationTokenSource = new CancellationTokenSource();
            var script = ContentTb.Content;
            Task.Factory.StartNew(() => {
                try
                {
                    ViewModel.Compiler.Compile(script, _cancellationTokenSource);
                    ViewModel.Paused = true;
                }
                catch (System.Exception ex)
                {
                    ViewModel.ShowMessage(ex.Message);
                }
            }, _cancellationTokenSource.Token);
        }


        private void RecordBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Paused = false;
            ViewModel.Generator.Option = OptionTb.Value;
            ViewModel.Generator.Reset();
            ViewModel.Generator.Add(ContentTb.Content);
            ViewModel.Recorder.Start();
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Paused = true;
            ViewModel.Recorder.Stop();
            _cancellationTokenSource.Cancel();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Dispose();
        }

        private void SpotBtn_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Thread.Sleep(500);
            var page = new SpotWindow();
            page.Spot += (_, x, y, endX, endY, e) =>
            {
                page.Close();
                Show();
                ContentTb.InsertLine(ViewModel.Generator.Render(
                    ViewModel.Generator.RenderIfToken(x, y, endX, endY, e)));
            };
            page.Show();
            page.Activate();
        }

        private void OptionBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OptionVisible = !ViewModel.OptionVisible;
        }
    }
}
