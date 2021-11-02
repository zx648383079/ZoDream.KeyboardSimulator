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

        public void Compile(IEnumerable<Token> tokens)
        {
            var fnItems = RenderFn(tokens);
            if (!fnItems.ContainsKey(MainEntery))
            {
                return;
            }
            CompileFn(fnItems[MainEntery], ref fnItems);
        }

        private bool CompileFn(IList<Token> tokens, ref IDictionary<string, IList<Token>> fnItems)
        {
            foreach (var item in tokens)
            {
                if (tokenSource != null && tokenSource.IsCancellationRequested)
                {
                    return false;
                }
                switch (item.Type)
                {
                    case TokenType.Delay:
                        Thread.Sleep(Convert.ToInt32(item.Content));
                        break;
                    case TokenType.Call:
                        if (fnItems.ContainsKey(item.Content))
                        {
                            var res = CompileFn(fnItems[item.Content], ref fnItems);
                            if (res == false)
                            {
                                return res;
                            }
                            break;
                        }
                        CompileFn(item);
                        break;
                    case TokenType.Exit:
                        return false;
                    default:
                        break;
                }
            }
            return true;
        }

        private void CompileFn(Token item)
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

        private IDictionary<string, IList<Token>> RenderFn(IEnumerable<Token> tokens)
        {
            var fn = MainEntery;
            var items = new Dictionary<string, IList<Token>>();
            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Fn:
                        fn = token.Content;
                        break;
                    case TokenType.End:
                        fn = MainEntery;
                        break;
                    default:
                        if (!items.ContainsKey(fn))
                        {
                            items.Add(fn, new List<Token>());
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
