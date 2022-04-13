using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using ZoDream.KeyboardSimulator.ViewModels;
using ZoDream.Shared.Parser;
using ZoDream.Shared.Recorder;

namespace ZoDream.KeyboardSimulator.Pages
{
    /// <summary>
    /// PlayWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PlayWindow : Window
    {
        public PlayWindow(IPlayViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public IPlayViewModel ViewModel;
        
        private PlayStatus status = PlayStatus.Stop;

        public PlayStatus Status
        {
            get { return status; }
            set {
                status = value;
                PlayBtn.Visibility = value != PlayStatus.Play ? Visibility.Visible : Visibility.Collapsed;
                PlayBtn.Label = value == PlayStatus.Pause ? "继续" : "开始";
                PauseBtn.Visibility = value != PlayStatus.Play ? Visibility.Collapsed : Visibility.Visible;
                StopBtn.Visibility = value == PlayStatus.Stop ? Visibility.Collapsed : Visibility.Visible;
            }
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Dispose();
            Status = PlayStatus.Stop;
            Close();
        }

        private async void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (await ViewModel.StopAsync())
            {
                Status = PlayStatus.Stop;
            }
        }

        private async void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (await ViewModel.PauseAsync())
            {
                Status = PlayStatus.Pause;
            }
        }

        private async void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (await ViewModel.PlayAsync())
            {
                Status = PlayStatus.Play;
            }
        }
    }

    public enum PlayStatus
    {
        Stop,
        Pause,
        Play,
    }
}
