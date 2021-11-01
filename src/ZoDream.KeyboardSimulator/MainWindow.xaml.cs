﻿using System;
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
using ZoDream.KeyboardSimulator.ViewModels;

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

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Paused = false;
        }

        private void RecordBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Paused = false;
            ViewModel.Generator.Reset();
            ViewModel.Generator.Add(ContentTb.Content);
            ViewModel.Recorder.Start();
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Paused = true;
            ViewModel.Recorder.Stop();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Dispose();
        }
    }
}
