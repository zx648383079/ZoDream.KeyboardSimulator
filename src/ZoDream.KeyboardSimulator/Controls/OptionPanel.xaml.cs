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
using ZoDream.Shared.Parser;

namespace ZoDream.KeyboardSimulator.Controls
{
    /// <summary>
    /// OptionPanel.xaml 的交互逻辑
    /// </summary>
    public partial class OptionPanel : UserControl
    {
        public OptionPanel()
        {
            InitializeComponent();
            Value = new ParserOption();
        }

        public ParserOption Value
        {
            get
            {
                return new ParserOption()
                {
                    MaxSpace = SpaceTb.Value,
                    HasMove = MoveTb.Value,
                };
            }
            set
            {
                SpaceTb.Value = value.MaxSpace;
                MoveTb.Value = value.HasMove;
            }
        }
    }
}
