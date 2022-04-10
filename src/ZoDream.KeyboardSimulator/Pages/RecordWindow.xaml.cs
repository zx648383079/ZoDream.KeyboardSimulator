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
using System.Windows.Shapes;
using ZoDream.Shared.Parser;
using ZoDream.Shared.Recorder;

namespace ZoDream.KeyboardSimulator.Pages
{
    /// <summary>
    /// RecordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RecordWindow : Window
    {
        public RecordWindow(ParserOption option)
        {
            InitializeComponent();
            Generator.Option = option;
        }

        public SystemRecorder? Recorder;
        public Generator Generator = new();
        private bool isPaused = true;

        public bool IsPaused
        {
            get { return isPaused; }
            set {
                isPaused = value;
                PlayBtn.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                StopBtn.Visibility = PauseBtn.Visibility 
                    = value ? Visibility.Collapsed : Visibility.Visible;
            }
        }


        public event FinishEventHandler? OnFinish;

        private void InitRecorder()
        {
            if (Recorder != null)
            {
                return;
            }
            Recorder = new SystemRecorder();
            Recorder.OnKey += Recorder_OnKey;
            Recorder.OnMouse += Recorder_OnMouse;
        }

        private void Recorder_OnKey(object sender, Shared.Input.KeyEventArgs e)
        {
            if (IsPaused)
            {
                return;
            }
            Generator.Add(e);
        }

        private void Recorder_OnMouse(object sender, Shared.Input.MouseEventArgs e)
        {
            if (IsPaused)
            {
                return;
            }
            Generator.Add(e);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Recorder?.Dispose();
            Generator.Dispose();
            IsPaused = true;
            Close();
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            IsPaused = true;
            OnFinish?.Invoke(this, Generator.ToString());
            Generator.Reset();
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            IsPaused = true;
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            IsPaused = false;
            Generator.ResetTime();
        }
    }

    public delegate void FinishEventHandler(object sender, string e);
}
