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
using System.Windows.Shapes;
using ZoDream.KeyboardSimulator.ViewModels;

namespace ZoDream.KeyboardSimulator.Pages
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow(SettingViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = viewModel;
        }

        public SettingViewModel ViewModel;

        private void BaseTb_ValueChanged(object sender, int[] _)
        {
            var point = BaseTb.GetIntArr(2);
            ViewModel.BaseX = point[0];
            ViewModel.BaseY = point[1];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BaseTb.SetIntArr(ViewModel.BaseX, ViewModel.BaseY);
        }
    }
}
