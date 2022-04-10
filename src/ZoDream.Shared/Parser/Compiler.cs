using Neo.IronLua;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ZoDream.Shared.Input;
using ZoDream.Shared.OS.WinApi;
using ZoDream.Shared.Player;
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
        private CancellationToken CancelToken = default;
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
            {"Esc", Key.Escape },
        };

        public void Compile(string code, CancellationToken token = default)
        {
            CancelToken = token;
            using var lua = new Lua();
            dynamic g = lua.CreateEnvironment<LuaGlobal>();
            // 注册方法 g.print = new Action<object>();
            g.FindWindow = new Func<string, string, IntPtr>(FindWindow);
            g.FocusWindow = new Action<IntPtr>(FocusWindow);
            g.GetWindowRect = new Func<IntPtr, int[]>(GetWindowRect);
            g.GetClientRect = new Func<IntPtr, int[]>(GetClientRect);
            g.SetBasePosition = new Action<int, int>(SetBasePosition);
            g.MoveTo = new Action<int, int>(MoveTo);
            g.Move = new Action<int, int>(MoveTo);
            g.MoveTween = new Action<object[]>(MoveTween);
            g.Click = new Action<object>(Click);
            g.MouseDown = new Action<string>(MouseDown);
            g.MouseUp = new Action<string>(MouseUp);
            g.KeyPress = new Action<string>(Input);
            g.KeyDown = new Action<string>(KeyDown);
            g.KeyUp = new Action<string>(KeyUp);
            g.Input = new Action<string>(Input);
            g.Delay = new Action<int>(Delay);
            g.Scroll = new Action<int>(Scroll);
            g.HotKey = new Action<string[]>(HotKey);
            g.GetPixelColor = new Func<int, int, string>(GetPixelColor);
            g.IsPixelColor = new Func<int, int, string, bool>(IsPixelColor);
            g.IsRectColor = new Func<int, int, int, int, string, bool>(IsRectColor);
            var chunk = lua.CompileChunk(code, "source.lua", new LuaCompileOptions() { 
                DebugEngine = new LuaCancelTokenDebug(token)
            });
            try
            {
                g.dochunk(chunk); // execute the chunk
            }
            catch (Exception e)
            {
                Console.WriteLine("Expception: {0}", e.Message);
                throw e;
            }
        }

        private bool IsRectColor(int x, int y, int endX, int endY, string color)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            return Snapshot.GetRect(x, y, endX, endY) == color;
        }

        private void Scroll(int diff)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            var j = 0;
            Tween.Invoke(0, diff, 120, (_, i) =>
            {
                if (CancelToken.IsCancellationRequested)
                {
                    throw new LuaCancelTokenException();
                }
                if (i == 0)
                {
                    return;
                }
                if (j > 0)
                {
                    Thread.Sleep(100);
                }
                Player.MouseWheel(i);
                j++;
            });
        }

        private void MouseDown(string button)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            Player.MouseDown(FormatButton(button));
        }

        private void MouseUp(string button)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            Player.MouseUp(FormatButton(button));
        }

        private void KeyDown(string key)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            Player.KeyDown(FormatKey(key));
        }

        private void KeyUp(string key)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            Player.KeyUp(FormatKey(key));
        }

        private void HotKey(string[] keys)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }   
            Player.KeyStroke(keys.Select(k => FormatKey(k)).ToArray());
        }

        private string GetPixelColor(int x, int y)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            var color = Player.GetPixelColor(x + BaseX, y + BaseY);
            return ColorTranslator.ToHtml(color).Substring(1);
        }

        private bool IsPixelColor(int x, int y, string color)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            return GetPixelColor(x, y).ToLower() == color.Replace("#", "").ToLower();
        }

        private void Click(object button)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            if (button is int)
            {
                Player.MouseClick((int)button);
                return;
            }
            var b = button.ToString();
            if (Str.IsInt(b))
            {
                Player.MouseClick(Convert.ToInt32(b));
                return;
            }
            Player.MouseClick(FormatButton(b));
        }

        private void Delay(int time)
        {
            Thread.Sleep(time);
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
        }

        private void Input(string key)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            Player.KeyPress(FormatKey(key));
        }

        private IntPtr FindWindow(string cls, string name)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            return Player.FindWindow(cls, name);
        }

        private void FocusWindow(IntPtr hwn)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            Player.FocusWindow(hwn);
        }

        private int[] GetWindowRect(IntPtr hwn)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            return Player.GetWindowRect(hwn);
        }

        private int[] GetClientRect(IntPtr hwn)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            return Player.GetClientRect(hwn);
        }

        private void SetBasePosition(int x, int y)
        {
            BaseX = x;
            BaseY = y;
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
        }

        private void MoveTo(int x, int y)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            Player.MouseMoveTo(x + BaseX, y + BaseY);
        }

        private void MoveTween(object[] param)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
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
                if (CancelToken.IsCancellationRequested)
                {
                    throw new LuaCancelTokenException();
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
            Player.Dispose();
        }
    }
}
