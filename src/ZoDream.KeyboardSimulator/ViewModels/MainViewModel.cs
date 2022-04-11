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

        public Compiler Compiler = new();

        public ParserOption Option = new();

        private bool paused = true;

        public bool Paused
        {
            get => paused;
            set {
                Set(ref paused, value);
            }
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
                Thread.Sleep(10000);
                if (token.IsCancellationRequested)
                {
                    return;
                }
                Message = string.Empty;
            }, token);

        }

        public void Dispose()
        {
            Compiler.Dispose();
        }
    }
}
