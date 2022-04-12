using System;
using System.Collections.Generic;
using System.IO;
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

        public readonly string OptionFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setting.xml");

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

        public Task LoadOptionAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                if (!File.Exists(OptionFileName))
                {
                    return;
                }
                try
                {
                    using var reader = Language.Storage.File.Reader(OptionFileName);
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ParserOption));
                    var res = serializer.Deserialize(reader);
                    if (res != null)
                    {
                        Option = (ParserOption)res;
                    }
                }
                catch (Exception)
                {
                    ShowMessage("setting.xml load failure");
                }
            });
        }

        private CancellationTokenSource SaveToken = new();

        public Task SaveOptionAsync()
        {
            SaveToken.Cancel();
            SaveToken = new CancellationTokenSource();
            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                if (SaveToken.IsCancellationRequested)
                {
                    return;
                }
                try
                {
                    using var writer = Language.Storage.File.Writer(OptionFileName);
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ParserOption));
                    serializer.Serialize(writer, Option);
                }
                catch (Exception)
                {
                    ShowMessage("setting.xml save failure");
                }
            }, SaveToken.Token);
           
        }
    }
}
