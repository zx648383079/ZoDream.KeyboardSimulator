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
            MaxDelay = option.MaxDelay;
            IsLogVisible = option.IsLogVisible;
            IsLogTime = option.IsLogTime;
            PlayHotKey = option.PlayHotKey;
            RecordHotKey = option.RecordHotKey;
            StopHotKey = option.StopHotKey;
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

        private int isMaxDelay;

        public int MaxDelay
        {
            get => isMaxDelay;
            set => Set(ref isMaxDelay, value);
        }

        private bool isLogVisible;

        public bool IsLogVisible
        {
            get => isLogVisible;
            set => Set(ref isLogVisible, value);
        }

        private bool isLogTime;

        public bool IsLogTime
        {
            get => isLogTime;
            set => Set(ref isLogTime, value);
        }

        private string playHotKey = string.Empty;

        public string PlayHotKey {
            get => playHotKey;
            set => Set(ref playHotKey, value);
        }


        private string recordHotKey = string.Empty;

        public string RecordHotKey {
            get => recordHotKey;
            set => Set(ref recordHotKey, value);
        }

        private string stopHotKey = string.Empty;

        public string StopHotKey {
            get => stopHotKey;
            set => Set(ref stopHotKey, value);
        }


        public ParserOption ToOption()
        {
            Source.MaxSpace = MaxSpace;
            Source.HasMove = HasMove;
            Source.BaseX = BaseX;
            Source.BaseY = BaseY;
            Source.MaxDelay = MaxDelay;
            Source.IsLogVisible = IsLogVisible;
            Source.IsLogTime = isLogTime;
            Source.PlayHotKey = playHotKey;
            Source.RecordHotKey = recordHotKey;
            Source.StopHotKey = stopHotKey;
            return Source;
        }
    }
}
