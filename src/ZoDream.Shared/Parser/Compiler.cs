using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ZoDream.Shared.Input;
using ZoDream.Shared.Player;

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
                if (tokenSource != null && tokenSource.IsCancellationRequested)
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
                switch (tokens[j].Type)
                {
                    case Token.Else:
                        elseStart = j;
                        break;
                    case Token.EndIf:
                    case Token.EndFn:
                    case Token.EndOfFile:
                        ifEnd = j;
                        break;
                    default:
                        break;
                }
            }
            var start = i + 1;
            var end = elseStart - 1;
            if (!CompileCondition(item.Content))
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
                if (tokenSource != null && tokenSource.IsCancellationRequested)
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

        private bool CompileCondition(string content)
        {
            return true;
        }

        private void CompileFn(TokenStmt item)
        {
            switch (item.Content)
            {
                case "Click":
                    Player.MouseClick((MouseButton)Enum.Parse(typeof(MouseButton), item.Parameters[0]));
                    break;
                case "DoubleClick":
                    Player.MouseDoubleClick((MouseButton)Enum.Parse(typeof(MouseButton), item.Parameters[0]));
                    break;
                case "MouseDown":
                    Player.MouseDown((MouseButton)Enum.Parse(typeof(MouseButton), item.Parameters[0]));
                    break;
                case "MouseUp":
                    Player.MouseUp((MouseButton)Enum.Parse(typeof(MouseButton), item.Parameters[0]));
                    break;
                case "Move":
                    Player.MouseMoveTo(Convert.ToDouble(item.Parameters[0]), Convert.ToDouble(item.Parameters[1]));
                    break;
                case "Input":
                    Player.KeyStroke((Key)Enum.Parse(typeof(Key), item.Parameters[0]));
                    break;
                case "KeyDown":
                    Player.KeyDown((Key)Enum.Parse(typeof(Key), item.Parameters[0]));
                    break;
                case "KeyUp":
                    Player.KeyUp((Key)Enum.Parse(typeof(Key), item.Parameters[0]));
                    break;
                case "Scroll":
                    Player.MouseWheel(Convert.ToInt32(item.Parameters[0]));
                    break;
                default:
                    break;
            }
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
