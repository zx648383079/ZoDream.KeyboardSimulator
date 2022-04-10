using Neo.IronLua;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ZoDream.Shared.Input;
using ZoDream.Shared.Player;
using ZoDream.Shared.Recorder.WinApi;
using ZoDream.Shared.Utils;

namespace ZoDream.Shared.Parser
{
    public class Compiler: IDisposable
    {
        public Compiler()
        {
            Player = new SystemPlayer();
        }

        public Compiler(IPlayer player)
        {
            Player = player;
        }

        private IPlayer Player;
        private CancellationTokenSource? tokenSource;
        private int BaseX = 0;
        private int BaseY = 0;
        public event TokenChangedEventHandler? TokenChanged;
        private IDictionary<string, Key> KeyMaps = new Dictionary<string, Key>()
        {
            {"0", Key.D0 },
            {"1", Key.D1 },
            {"2", Key.D2 },
            {"3", Key.D3 },
            {"4", Key.D4 },
            {"5", Key.D5 },
            {"6", Key.D6 },
            {"7", Key.D7 },
            {"8", Key.D8 },
            {"9", Key.D9 },
            {"-", Key.OemMinus },
            {"=", Key.OemPlus },
            {"[", Key.OemOpenBrackets },
            {"]", Key.OemCloseBrackets },
            {";", Key.OemSemicolon },
            {"'", Key.OemQuotes },
            {"\\", Key.OemBackslash },
            {",", Key.OemComma },
            {".", Key.OemPeriod },
            {"/", Key.Divide },
            {"`", Key.OemTilde },
        };

        private bool IsCancellationRequested => tokenSource != null && tokenSource.IsCancellationRequested;

        public void Compile(string code, CancellationTokenSource? cancellationTokenSource)
        {
            tokenSource = cancellationTokenSource;
            using var lua = new Lua();
            dynamic g = lua.CreateEnvironment<LuaGlobal>();
            // 注册方法 g.print = new Action<object>();
            g.FindWindow = new Func<string, string, IntPtr>(FindWindow);
            g.FocusWindow = new Action<IntPtr>(FocusWindow);
            g.GetWindowRect = new Func<IntPtr, int[]>(GetWindowRect);
            g.SetBasePosition = new Action<int, int>(SetBasePosition);
            g.MoveTo = new Action<int, int>(MoveTo);
            g.Move = new Action<int, int>(MoveTo);
            g.MoveTween = new Action<object[]>(MoveTween);
            g.Click = new Action<int>(Click);
            g.KeyPress = new Action<string>(Input);
            g.Input = new Action<string>(Input);
            g.Delay = new Action<int>(Delay);
            g.Scroll = new Action<int>(Scroll);
            g.GetPixelColor = new Func<int, int, string>(GetPixelColor);
            g.IsPixelColor = new Func<int, int, string, bool>(IsPixelColor);
            var chunk = lua.CompileChunk(code, "source.lua", new LuaCompileOptions() { DebugEngine = LuaExceptionDebugger.Default });
            try
            {
                g.dochunk(chunk); // execute the chunk
            }
            catch (Exception e)
            {
                Console.WriteLine("Expception: {0}", e.Message);
                var d = LuaExceptionData.GetData(e); // get stack trace
                Console.WriteLine("StackTrace: {0}", d.FormatStackTrace(0, false));
            }
        }

        private void Scroll(int diff)
        {
            Player.MouseWheel(diff);
        }

        private string GetPixelColor(int x, int y)
        {
            var color = Player.GetPixelColor(x, y);
            return ColorTranslator.ToHtml(color).Substring(1);
        }

        private bool IsPixelColor(int x, int y, string color)
        {
            return GetPixelColor(x, y).ToLower() == color.ToLower();
        }

        private void Click(int count = 1)
        {
            Player.MouseClick(count);
        }

        private void Delay(int time)
        {
            Thread.Sleep(time);
        }

        private void Input(string key)
        {
            Player.KeyPress(FormatKey(key));
        }

        private IntPtr FindWindow(string cls, string name)
        {
            return Player.FindWindow(cls, name);
        }

        private void FocusWindow(IntPtr hwn)
        {
            Player.FocusWindow(hwn);
        }

        private int[] GetWindowRect(IntPtr hwn)
        {
            return Player.GetWindowRect(hwn);
        }

        private void SetBasePosition(int x, int y)
        {
            BaseX = x;
            BaseY = y;
        }

        private void MoveTo(int x, int y)
        {
            Player.MouseMoveTo(x + BaseX, y + BaseY);
        }

        private void MoveTween(object[] param)
        {
            if (param.Length < 2)
            {
                return;
            }
            var x = Convert.ToDouble(param[0]);
            var y = Convert.ToDouble(param[1]);
            if (param.Length == 2)
            {
                Player.MouseMoveTo(x, y);
                return;
            }
            var point = MouseNativeMethods.GetMousePosition();
            var time = Convert.ToInt32(param[2]);
            var stepTime = 15;
            var step = time / stepTime;
            if (step < 2)
            {
                Player.MouseMoveTo(x, y);
                return;
            }
            var stepX = (x - point.X) / step;
            var stepY = (y - point.Y) / step;
            var startX = (double)point.X;
            var startY = (double)point.Y;
            while (true)
            {
                Thread.Sleep(stepTime);
                if (IsCancellationRequested)
                {
                    return;
                }
                startX += stepX;
                startY += stepY;
                if ((stepX < 0 && startX < x) || (stepX > 0 && startX > x))
                {
                    startX = x;
                }
                if ((stepY < 0 && startY < y) || (stepY > 0 && startY > y))
                {
                    startY = y;
                }
                Player.MouseMoveTo(startX, startY);
                if (startX == x && startY == y)
                {
                    break;
                }
            }
        }

        private MouseButton FormatButton(string b)
        {
            if (string.IsNullOrWhiteSpace(b))
            {
                return MouseButton.Left;
            }
            return (MouseButton)Enum.Parse(typeof(MouseButton), b);
        }

        private ushort FormatScanKey(string k)
        {
            k = k.Trim();
            if (Str.IsInt(k))
            {
                return (ushort)Str.ToInt(k);
            }
            return Player.GetScanKey((Key)Enum.Parse(typeof(Key), k));
        }

        private Key FormatKey(string k)
        {
            k = k.Trim();
            if (KeyMaps.ContainsKey(k))
            {
                return KeyMaps[k];
            }
            if (Str.IsInt(k)) {
                return (Key)Str.ToInt(k);
            }
            return (Key)Enum.Parse(typeof(Key), k);
        }

        public void Dispose()
        {
            tokenSource?.Dispose();
            Player.Dispose();
        }
    }
}
