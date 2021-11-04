using System;
using System.Collections.Generic;
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
        const string MainEntery = "main";

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

        private bool IsCancellationRequested => tokenSource != null && tokenSource.IsCancellationRequested;

        public void Compile(string code, CancellationTokenSource? cancellationTokenSource)
        {
            tokenSource = cancellationTokenSource;
            Compile(new Tokenizer().Parse(code));
        }

        public void Compile(IEnumerable<TokenStmt> tokens)
        {
            var fnItems = RenderFn(tokens);
            if (!fnItems.ContainsKey(MainEntery))
            {
                return;
            }
            CompileFn(fnItems[MainEntery], ref fnItems);
        }

        private bool CompileFn(IList<TokenStmt> tokens, ref IDictionary<string, IList<TokenStmt>> fnItems)
        {
            var i = -1;
            bool res;
            while (i < tokens.Count - 1)
            {
                i++;
                var item = tokens[i];
                if (IsCancellationRequested)
                {
                    return false;
                }
                switch (item.Type)
                {
                    case Token.Delay:
                        Thread.Sleep(Convert.ToInt32(item.Content));
                        break;
                    case Token.FnCall:
                        if (fnItems.ContainsKey(item.Content))
                        {
                            res = CompileFn(fnItems[item.Content], ref fnItems);
                            if (res == false)
                            {
                                return res;
                            }
                            break;
                        }
                        CompileFn(item);
                        break;
                    case Token.Exit:
                        return false;
                    case Token.If:
                        res = CompileIf(ref i, ref tokens, ref fnItems);
                        if (res == false)
                        {
                            return res;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        private bool CompileIf(ref int i, ref IList<TokenStmt> tokens, ref IDictionary<string, IList<TokenStmt>> fnItems)
        {
            var item = tokens[i];
            var elseStart = -1;
            var ifEnd = -1;
            for (int j = i + 1; j < tokens.Count; j++)
            {
                var t = tokens[j].Type;
                if (t == Token.Else) {
                    elseStart = j;
                    continue;
                }
                if (t == Token.EndIf || t == Token.EndFn || t == Token.EndOfFile)
                {
                    ifEnd = j;
                    break;
                }
            }
            if (ifEnd < 0)
            {
                ifEnd = tokens.Count;
            }
            var start = i + 1;
            var end = elseStart - 1;
            if (!CompileCondition(item.Parameters, item.Content))
            {
                start = elseStart + 1;
                end = ifEnd - 1;
            }
            if (start < 0)
            {
                i = end;
                return true;
            }
            for (; start < end; start++)
            {
                item = tokens[start];
                if (IsCancellationRequested)
                {
                    i = end;
                    return false;
                }
                switch (item.Type)
                {
                    case Token.Delay:
                        Thread.Sleep(Convert.ToInt32(item.Content));
                        break;
                    case Token.FnCall:
                        if (fnItems.ContainsKey(item.Content))
                        {
                            var res = CompileFn(fnItems[item.Content], ref fnItems);
                            if (res == false)
                            {
                                i = end;
                                return res;
                            }
                            break;
                        }
                        CompileFn(item);
                        break;
                    case Token.Exit:
                        i = end;
                        return false;
                    default:
                        break;
                }
            }
            i = end;
            return true;
        }

        private bool CompileCondition(string[] point, string content)
        {
            if (point.Length != 4)
            {
                return false;
            }
            var val = Snapshot.GetRect(Convert.ToInt32(point[0]),
                Convert.ToInt32(point[1]),
                Convert.ToInt32(point[2]), Convert.ToInt32(point[3]));
            return !string.IsNullOrEmpty(val) && content == val;
        }

        private void CompileFn(TokenStmt item)
        {
            switch (item.Content)
            {
                case "Click":
                    Player.MouseClick(FormatButton(item.Parameters[0]));
                    break;
                case "DoubleClick":
                    Player.MouseDoubleClick(FormatButton(item.Parameters[0]));
                    break;
                case "MouseDown":
                    Player.MouseDown(FormatButton(item.Parameters[0]));
                    break;
                case "MouseUp":
                    Player.MouseUp(FormatButton(item.Parameters[0]));
                    break;
                case "Move":
                    MoveTween(item.Parameters);
                    break;
                case "Input":
                    Player.KeyStroke(FormatKey(item.Parameters[0]));
                    break;
                case "HotKey":
                    Player.KeyStroke(item.Parameters.Select(i => FormatKey(i)).ToArray());
                    break;
                case "KeyDown":
                    Player.KeyDown(FormatKey(item.Parameters[0]));
                    break;
                case "KeyUp":
                    Player.KeyUp(FormatKey(item.Parameters[0]));
                    break;
                case "Scroll":
                    Player.MouseWheel(Convert.ToInt32(item.Parameters[0]));
                    break;
                default:
                    break;
            }
        }

        private void MoveTween(string[] param)
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

        private Key FormatKey(string k)
        {
            k = k.Trim();
            if (Str.IsInt(k)) {
                return (Key)Str.ToInt(k);
            }
            return (Key)Enum.Parse(typeof(Key), k);
        }

        private IDictionary<string, IList<TokenStmt>> RenderFn(IEnumerable<TokenStmt> tokens)
        {
            var fn = MainEntery;
            var items = new Dictionary<string, IList<TokenStmt>>();
            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case Token.Fn:
                        fn = token.Content;
                        break;
                    case Token.EndFn:
                        fn = MainEntery;
                        break;
                    default:
                        if (!items.ContainsKey(fn))
                        {
                            items.Add(fn, new List<TokenStmt>());
                        }
                        items[fn].Add(token);
                        break;
                }
            }
            return items;
        }

        public void Dispose()
        {
            tokenSource?.Dispose();
            Player.Dispose();
        }
    }
}
