using Neo.IronLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ZoDream.Language.Loggers;
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

        public ILogger? Logger { get; set; }
        private IPlayer Player;
        private CancellationToken CancelToken = default;
        private int BaseX = 0;
        private int BaseY = 0;
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
            {"/", Key.OemQuestion },
            {"`", Key.OemTilde },
            {"Esc", Key.Escape },
            {"Backspace", Key.Back },
        };
        
        public void Compile(string code, CancellationToken token = default)
        {
            CancelToken = token;
            Player.Logger = Logger;
            SetBasePosition(0, 0);
            using var lua = new Lua();
            dynamic g = lua.CreateEnvironment<LuaGlobal>();
            // 注册方法 g.print = new Action<object>();
            g.FindWindow = new Func<string, string, int>(FindWindow);
            g.FocusWindow = new Action<int>(FocusWindow);
            g.GetWindowRect = new Func<int, int[]>(GetWindowRect);
            g.GetClientRect = new Func<int, int[]>(GetClientRect);
            g.SetBasePosition = new Action<int, int>(SetBasePosition);
            g.MoveTo = new Action<int, int>(MoveTo);
            g.Move = new Action<int, int>(MoveTo);
            g.MoveTween = new Action<int, int, int>(MoveTween);
            g.Click = new Action<object, int?>(Click);
            g.MouseDown = new Action<string>(MouseDown);
            g.MouseUp = new Action<string>(MouseUp);
            g.KeyPress = new Action<string, int?>(Input);
            g.KeyDown = new Action<string>(KeyDown);
            g.KeyUp = new Action<string>(KeyUp);
            g.Input = new Action<string, int?>(Input);
            g.Delay = new Action<int>(Delay);
            g.Scroll = new Action<int>(Scroll);
            g.HotKey = new Action<string[]>(HotKey);
            g.Log = new Action<object>(ConsoleLog);
            g.GetPixelColor = new Func<int, int, string>(GetPixelColor);
            g.IsPixelColor = new Func<int, int, string, bool>(IsPixelColor);
            g.IsRectColor = new Func<int, int, int, int, string, bool>(IsRectColor);
            g.InColor = new Func<string, string?, string?, bool>(InColor);
            try
            {
                var chunk = lua.CompileChunk(code, "source.lua", new LuaCompileOptions()
                {
                    DebugEngine = new LuaCancelTokenDebug(token)
                });
                g.dochunk(chunk); // execute the chunk

            }
            catch (LuaParseException e)
            {
                var error = $"Lua Parse Expception: {e.Message}, Line:{e.Line}, Column:{e.Column}";
                Logger?.Error(error);
                throw e;
            }
            catch (LuaRuntimeException e)
            {
                var error = $"Lua Runtime Expception: {e.StackTrace}";
                Logger?.Error(error);
                throw e;
            }
            catch (LuaCancelTokenException e)
            {
                Logger?.Info("Execution has been cancelled");
                return;
            }
            catch (Exception e)
            {
                Logger?.Error($"Lua Expception: {e.Message}");
                throw e;
            }
        }

        private void ConsoleLog(object msg)
        {
            Logger?.Log(msg.ToString());
        }

        private bool InColor(string color, string? min, string? max)
        {
            var c = ColorHelper.From(color);
            if (!string.IsNullOrWhiteSpace(min) && 
                ColorHelper.Compare(c, ColorHelper.From(min!)) < 0)
            {
                return false;
            }
            if (!string.IsNullOrWhiteSpace(max) &&
                ColorHelper.Compare(c, ColorHelper.From(max!, Color.White)) > 0)
            {
                return false;
            }
            return true;
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
            return ColorHelper.To(Player.GetPixelColor(x + BaseX, y + BaseY));
        }

        private bool IsPixelColor(int x, int y, string color)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            return GetPixelColor(x, y) == ColorHelper.Format(color);
        }

        private void Click(object? button, int? count)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            if (button == null)
            {
                Player.MouseClick();
                return;
            }
            if (button is int)
            {
                MouseClick(MouseButton.Left, (int)button);
                return;
            }
            var b = button.ToString();
            if (Str.IsInt(b))
            {
                MouseClick(MouseButton.Left, Convert.ToInt32(b));
                return;
            }
            MouseClick(FormatButton(b), count == null || count < 1 ? 1 : (int)count);
        }

        private void MouseClick(MouseButton button, int count)
        {
            Loop(() =>
            {
                Player.MouseClick(button);
            }, count);
        }

        private void Loop(Action func, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                {
                    Delay(50);
                }
                if (CancelToken.IsCancellationRequested)
                {
                    throw new LuaCancelTokenException();
                }
                func();
            }
        }

        private void Delay(int time)
        {
            Thread.Sleep(time);
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
        }

        private void Input(string key, int? count)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            var k = FormatKey(key);
            var c = count == null || count < 1 ? 1 : (int)count;
            Loop(() =>
            {
                Player.KeyPress(k);
            }, c);
        }

        private int FindWindow(string cls, string name)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            return (int)Player.FindWindow(cls, name);
        }

        private void FocusWindow(int hwn)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            Player.FocusWindow(new IntPtr(hwn));
        }

        private int[] GetWindowRect(int hwn)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            return Player.GetWindowRect(new IntPtr(hwn));
        }

        private int[] GetClientRect(int hwn)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            return Player.GetClientRect(new IntPtr(hwn));
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

        private void MoveTween(int x, int y, int time = 0)
        {
            if (CancelToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            if (time <= 0)
            {
                MoveTo(x, y);
                return;
            }
            var point = MouseNativeMethods.GetMousePosition();
            var stepTime = 15;
            var step = time / stepTime;
            if (step < 2)
            {
                MoveTo(x, y);
                return;
            }
            var realX = x + BaseX;
            var realY = y + BaseY;
            var stepX = (realX - point.X) / step;
            var stepY = (realY - point.Y) / step;
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
                if ((stepX < 0 && startX < realX) || (stepX > 0 && startX > realX))
                {
                    startX = realX;
                }
                if ((stepY < 0 && startY < realY) || (stepY > 0 && startY > realY))
                {
                    startY = realY;
                }
                Player.MouseMoveTo(startX, startY);
                if (startX == realX && startY == realY)
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
            return (Key)Enum.Parse(typeof(Key), k.Length == 1 ? k.ToUpper() : k);
        }

        public void Dispose()
        {
            Player.Dispose();
        }
    }
}
