using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Parser;

namespace ZoDream.KeyboardSimulator.ViewModels
{
    public class SettingViewModel: BindableBase
    {

        public SettingViewModel()
        {

        }

        public SettingViewModel(ParserOption option)
        {
            MaxSpace = option.MaxSpace;
            HasMove = option.HasMove;
        }

        private int maxSpace = 150;

        public int MaxSpace
        {
            get => maxSpace;
            set => Set(ref maxSpace, value);
        }

        private bool hasMove = false;

        public bool HasMove
        {
            get => hasMove;
            set => Set(ref hasMove, value);
        }


        public ParserOption ToOption()
        {
            return new ParserOption()
            {
                MaxSpace = MaxSpace,
                HasMove = HasMove
            };
        }
    }
}
