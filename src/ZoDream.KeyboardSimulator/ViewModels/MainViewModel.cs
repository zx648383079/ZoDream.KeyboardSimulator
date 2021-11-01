using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Parser;
using ZoDream.Shared.Player;
using ZoDream.Shared.Recorder;

namespace ZoDream.KeyboardSimulator.ViewModels
{
    public class MainViewModel: BindableBase, IDisposable
    {

        public SystemRecorder Recorder = new SystemRecorder();
        public SystemPlayer Player = new SystemPlayer();
        public Generator Generator = new Generator();

        private bool paused = true;

        public bool Paused
        {
            get => paused;
            set => Set(ref paused, value);
        }

        public void Dispose()
        {
            Recorder.Dispose();
            Player.Dispose();
        }
    }
}
