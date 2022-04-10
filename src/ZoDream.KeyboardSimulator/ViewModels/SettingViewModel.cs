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
            Source = new ParserOption();
        }

        public SettingViewModel(ParserOption option)
        {
            Source = option;
            MaxSpace = option.MaxSpace;
            HasMove = option.HasMove;
            BaseX = option.BaseX;
            BaseY = option.BaseY;
        }

        private ParserOption Source;

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

        private int baseX = 0;

        public int BaseX
        {
            get => baseX;
            set => Set(ref baseX, value);
        }

        private int baseY = 0;

        public int BaseY
        {
            get => baseY;
            set => Set(ref baseY, value);
        }


        public ParserOption ToOption()
        {
            Source.MaxSpace = MaxSpace;
            Source.HasMove = HasMove;
            Source.BaseX = BaseX;
            Source.BaseY = BaseY;
            return Source;
        }
    }
}
