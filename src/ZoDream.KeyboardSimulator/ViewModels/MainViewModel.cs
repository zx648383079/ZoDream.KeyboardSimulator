using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZoDream.Shared.Parser;
using ZoDream.Shared.Player;
using ZoDream.Shared.Recorder;

namespace ZoDream.KeyboardSimulator.ViewModels
{
    public class MainViewModel: BindableBase, IDisposable
    {

        public SystemRecorder Recorder = new();
        public Compiler Compiler = new();
        public Generator Generator = new();

        private bool paused = true;

        public bool Paused
        {
            get => paused;
            set {
                Set(ref paused, value);
                if (!value)
                {
                    OptionVisible = false;
                }
            }
        }

        private bool optionVisible = false;

        public bool OptionVisible
        {
            get => optionVisible;
            set => Set(ref optionVisible, value);
        }



        private CancellationTokenSource messageToken = new();
        private string message = string.Empty;

        public string Message
        {
            get => message;
            set => Set(ref message, value);
        }

        public void ShowMessage(string message)
        {
            messageToken.Cancel();
            messageToken = new CancellationTokenSource();
            var token = messageToken.Token;
            Message = message;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
                if (token.IsCancellationRequested)
                {
                    return;
                }
                Message = string.Empty;
            }, token);

        }

        public void Dispose()
        {
            Recorder.Dispose();
            Compiler.Dispose();
            Generator.Dispose();
        }
    }
}
