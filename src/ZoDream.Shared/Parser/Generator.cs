using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ZoDream.Shared.Input;

namespace ZoDream.Shared.Parser
{
    public class Generator
    {

        public IList<Token> TokenItems { get; set; } = new List<Token>();

        private DateTime lastTime = DateTime.MinValue;

        public void Reset()
        {
            TokenItems.Clear();
            lastTime = DateTime.MinValue;
        }

        public void Add(KeyEventArgs e)
        {
            var now = DateTime.Now;
            if (lastTime == DateTime.MinValue)
            {
                lastTime = now;
            }
            else
            {
                var diff = now - lastTime;
                if (diff.TotalMilliseconds > 100)
                {
                    Add(new Token(TokenType.Delay, diff.TotalMilliseconds.ToString()));
                }
                lastTime = now;
            }
            Add(EventToToken(e));
        }

        public void Add(MouseEventArgs e)
        {
            if (e.IsMove)
            {
                return;
            }
            var now = DateTime.Now;
            if (lastTime == DateTime.MinValue)
            {
                lastTime = now;
            }
            else
            {
                var diff = now - lastTime;
                if (diff.TotalMilliseconds > 100)
                {
                    Add(new Token(TokenType.Delay, diff.TotalMilliseconds.ToString()));
                }
                lastTime = now;
            }
            Add(EventToToken(e));
        }

        public void Add(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }
            Add(new Tokenizer().Parse(line));
        }

        public void Add(Token token)
        {
            if (TokenItems.Count < 1 || token.Type != TokenType.Call)
            {
                TokenItems.Add(token);
                return;
            }
            if (token.Parameters == null)
            {
                TokenItems.Add(token);
                return;
            }
            if (token.Content == "Move")
            {
                AddIfMove(token);
                return;
            }
            if (token.Content.EndsWith("Up"))
            {
                AddIfClick(token);
                return;
            }
            TokenItems.Add(token);
        }

        private void AddIfMove(Token token)
        {
            for (int i = TokenItems.Count - 1; i >= 0; i--)
            {
                var item = TokenItems[i];
                if (item.Type != token.Type || item.Content != token.Content)
                {
                    continue;
                }
                if (Enumerable.SequenceEqual(token.Parameters, item.Parameters))
                {
                    return;
                }
            }
            TokenItems.Add(token);
        }

        private void AddIfClick(Token token)
        {
            var end = TokenItems[TokenItems.Count - 1];
            if (end.Type != TokenType.Call || end.Parameters == null)
            {
                TokenItems.Add(token);
                return;
            }
            var fn = token.Content.Substring(0, token.Content.Length - 2);
            if (end.Content != fn + "Down")
            {
                TokenItems.Add(token);
                return;
            }
            if (token.Parameters.Length != end.Parameters.Length
                || end.Parameters.Length != 1 || token.Parameters[0] != end.Parameters[0])
            {
                TokenItems.Add(token);
                return;
            }
            switch (fn)
            {
                case "Mouse":
                    end.Content = "Click";
                    return;
                case "Key":
                    end.Content = "Input";
                    return;
                default:
                    break;
            }
            if (fn.Contains("Mouse"))
            {
                end.Content = fn + "Click";
            }
            else if (fn.Contains("Key"))
            {
                end.Content = fn + "Input";
            }
        }

        public void Add(IEnumerable<Token> tokens)
        {
            foreach (var item in tokens)
            {
                Add(item);
            }
        }


        public string Render(Token token)
        {
            switch (token.Type)
            {
                case TokenType.Fn:
                    return $"fn {token.Content}";
                case TokenType.If:
                    return $"if {token.Content}";
                case TokenType.IfElse:
                    return $"else";
                case TokenType.Delay:
                    return $"Delay({token.Content})";
                case TokenType.Call:
                    return $"{token.Content}({string.Join(',', token.Parameters)})";
                case TokenType.Parameter:
                    return token.Content;
                case TokenType.End:
                    return string.Empty;
                case TokenType.Exit:
                    return "exit";
                default:
                    return string.Empty;
            }
        }

        public string Render(IEnumerable<Token> tokens)
        {
            var sb = new StringBuilder();
            foreach (var item in tokens)
            {
                sb.AppendLine(Render(item));
            }
            return sb.ToString();
        }


        private IList<Token> EventToToken(KeyEventArgs e)
        {
            var fn = RenderKeyFn(e.KeyStates);
            if (string.IsNullOrEmpty(fn))
            {
                return new List<Token>() { new Token(TokenType.Call, fn)};
            }
            return new List<Token>() { new Token(TokenType.Call, fn, new string[] { e.Key.ToString() }) };
        }

        private IList<Token> EventToToken(MouseEventArgs e)
        {
            var items = new List<Token>();
            if (e.Point != null)
            {
                items.Add(Token.Call("Move", e.Point.X, e.Point.Y));
            }
            for (int i = 0; i < e.ClickCount; i++)
            {
                if (e.LeftButton != ButtonState.None)
                {
                    items.Add(RenderMouseFn(e.LeftButton, MouseButton.Left));
                }
                if (e.RightButton != ButtonState.None)
                {
                    items.Add(RenderMouseFn(e.RightButton, MouseButton.Right));
                }
                if (e.MiddleButton != ButtonState.None)
                {
                    items.Add(RenderMouseFn(e.MiddleButton, MouseButton.Middle));
                }
                if (e.XButton1 != ButtonState.None)
                {
                    items.Add(RenderMouseFn(e.XButton1, MouseButton.XButton1));
                }
                if (e.XButton2 != ButtonState.None)
                {
                    items.Add(RenderMouseFn(e.XButton2, MouseButton.XButton1));
                }
            }
            if (e.HasScroll)
            {
                items.Add(Token.Call("Scroll", e.WheelDelta));
            }
            return items;
        }

        public string Render(KeyEventArgs e)
        {
            return Render(EventToToken(e));
        }

        private string RenderKeyFn(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Released:
                    return "KeyUp";
                case ButtonState.Pressed:
                    return "KeyDown";
                default:
                    break;
            }
            return string.Empty;
        }

        public string Render(MouseEventArgs e)
        {
            return Render(EventToToken(e));
        }

        private Token RenderMouseFn(ButtonState state, MouseButton button)
        {
            if (state == ButtonState.Pressed)
            {
                return Token.Call("MouseDown", button);
            }
            if (state == ButtonState.Released)
            {
                return Token.Call("MouseUp", button);
            }
            return new Token();
        }

        public override string ToString()
        {
            return Render(TokenItems);
        }
    }
}
