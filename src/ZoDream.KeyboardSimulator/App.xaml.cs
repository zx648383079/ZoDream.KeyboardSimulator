using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ZoDream.KeyboardSimulator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (e.Args.Length == 0)
            {
                return;
            }
            EnterFileName = e.Args[0];
        }

        internal static string EnterFileName = string.Empty;
    }
}
