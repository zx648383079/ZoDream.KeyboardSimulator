using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Parser;
using ZoDream.Shared.Recorder;

namespace ZoDream.KeyboardSimulator.ViewModels
{
    public class RecordViewModel : IPlayViewModel
    {
        public RecordViewModel(ParserOption option)
        {
            Generator.Option = option;
        }

        public SystemRecorder? Recorder;
        public Generator Generator = new();

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
            Generator.Add(e);
        }

        private void Recorder_OnMouse(object sender, Shared.Input.MouseEventArgs e)
        {
            Generator.Add(e);
        }

        public void Dispose()
        {
            Recorder?.Dispose();
            Generator.Dispose();
        }

        public Task<bool> PauseAsync()
        {
            Recorder?.Stop();
            return Task.FromResult(true);
        }

        public Task<bool> PlayAsync()
        {
            InitRecorder();
            Generator.ResetTime();
            Recorder?.Start();
            return Task.FromResult(true);
        }

        public Task<bool> StopAsync()
        {
            Recorder?.Stop();
            OnFinish?.Invoke(this, Generator.ToString());
            Generator.Reset();
            return Task.FromResult(true);
        }

        public delegate void FinishEventHandler(object sender, string e);
    }
}
