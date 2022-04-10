using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ZoDream.KeyboardSimulator.Pages;
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
            ViewModel.Compiler.TokenChanged += Compiler_TokenChanged;
        }

        public MainViewModel ViewModel = new();
        private CancellationTokenSource _cancellationTokenSource = new();

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
                    ViewModel.Compiler.Compile(script, _cancellationTokenSource.Token);
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
            var page = new RecordWindow(ViewModel.Option);
            page.Show();
            page.OnFinish += (_, e) =>
            {
                if (MessageBox.Show("是否保存记录？将追加到脚本中？", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ContentTb.InsertLine(e);
                }
            };
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Paused = true;
            _cancellationTokenSource.Cancel();
            
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Dispose();
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
            };
        }
    }
}
