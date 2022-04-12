using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ZoDream.Shared.Input;
using ZoDream.Shared.Utils;

namespace ZoDream.Shared.Parser
{
    public class Generator: IDisposable
    {

        public IList<TokenStmt> TokenItems { get; set; } = new List<TokenStmt>();

        private DateTime lastTime = DateTime.MinValue;

        private readonly Queue<object> waitItems = new Queue<object>();
        private bool isLoading = false;

        public ParserOption Option { get; set; } = new();

        public void Reset()
        {
            TokenItems.Clear();
            ResetTime();
        }

        public void ResetTime()
        {
            lastTime = DateTime.Now;
        }

        public void Add(KeyEventArgs e)
        {
            waitItems.Enqueue(e);
            if (isLoading)
            {
                return;
            }
            AddAsync();
        }

        private void AddAsync()
        {
            if (isLoading)
            {
                return;
            }
            isLoading = true;
            while (waitItems.Count > 0)
            {
                var e = waitItems.Dequeue();
                if (e == null)
                {
                    break;
                }
                if (e is KeyEventArgs)
                {
                    AddAsync(e as KeyEventArgs);
                }
                if (e is MouseEventArgs)
                {
                    AddAsync(e as MouseEventArgs);
                }
            }
            isLoading = false;
        }

        private void AddAsync(KeyEventArgs? e)
        {
            if (e == null)
            {
                return;
            }
            AddDelay(e.HappenTime);
            Add(EventToToken(e), true);
        }

        private void AddDelay(DateTime? date)
        {
            var now = date == null ? DateTime.Now : (DateTime)date;
            if (lastTime == DateTime.MinValue)
            {
                lastTime = now;
            }
            else
            {
                var diff = now - lastTime;
                if (diff.TotalMilliseconds > Option.MaxSpace)
                {
                    Add(new TokenStmt(Token.Delay, ((int)diff.TotalMilliseconds).ToString()));
                }
                lastTime = now;
            }
        }

        private void AddAsync(MouseEventArgs? e)
        {
            if (e == null)
            {
                return;
            }
            AddDelay(e.HappenTime);
            Add(EventToToken(e), true);
        }

        public void Add(MouseEventArgs e)
        {
            if (e.IsMove && !Option.HasMove)
            {
                return;
            }
            waitItems.Enqueue(e);
            if (isLoading)
            {
                return;
            }
            AddAsync();
        }

        public void Add(TokenStmt token)
        {
            if (TokenItems.Count < 1 || token.Type != Token.FnCall)
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

        public void AddIfStmt(double x, double y, double endX, double endY, string hash)
        {
            Add(RenderIfToken(x, y, endX, endY, hash), true);
        }

        private void AddIfMove(TokenStmt token)
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

        private void AddIfClick(TokenStmt token)
        {
            var end = TokenItems[TokenItems.Count - 1];
            if (end.Type != Token.FnCall || end.Parameters == null)
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
            if (!IsSameParam(token.Parameters, end.Parameters))
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

        private bool IsSameParam(object[]? a, object[]? b)
        {
            if (a == b)
            {
                return true;
            }
            var len = a == null ? 0 : a.Length;
            var len2 = b == null ? 0 : b.Length;
            if (len != len2)
            {
                return false;
            }
            for (int i = 0; i < len; i++)
            {
                if (a![i].ToString() != b![i].ToString())
                {
                    return false;
                }
            }
            return true;
        }

        public void Add(IEnumerable<TokenStmt> tokens)
        {
            Add(tokens, false);
        }

        private void Add(IEnumerable<TokenStmt> tokens, bool filterEmpty)
        {
            foreach (var item in tokens)
            {
                if (filterEmpty && item.Type == Token.EndFn 
                    && string.IsNullOrEmpty(item.Content) && item.Parameters == null)
                {
                    continue;
                }
                Add(item);
            }
        }


        public IEnumerable<TokenStmt> RenderIfToken(double x, double y, double endX, double endY, string hash)
        {
            return new TokenStmt[]
            {
                new TokenStmt(Token.If, hash, new string[] { x.ToString(), y.ToString(),
                endX.ToString(), endY.ToString() }),
                new TokenStmt(Token.EndIf),
            };
        }

        public string Render(TokenStmt token)
        {
            switch (token.Type)
            {
                case Token.Fn:
                    return $"function {token.Content}()";
                case Token.If:
                    return $"if({RenderParameter(token.Parameters)}={token.Content})\nthen";
                case Token.Else:
                    return $"else";
                case Token.EndIf:
                    return $"end";
                case Token.Delay:
                    return $"Delay({token.Content})";
                case Token.FnCall:

                    return $"{token.Content}({RenderParameter(token.Parameters)})";
                case Token.Parameter:
                    return token.Content;
                case Token.EndFn:
                case Token.EndOfFile:
                    return string.Empty;
                case Token.Exit:
                    return "exit";
                default:
                    return string.Empty;
            }
        }

        public string RenderParameter(params object[]? items)
        {
            if (items == null)
            {
                return string.Empty;
            }
            return string.Join(",", items.Select(i => i is int ? i : $"\"{i}\""));
        }

        public string Render(IEnumerable<TokenStmt> tokens)
        {
            var sb = new StringBuilder();
            foreach (var item in tokens)
            {
                sb.AppendLine(Render(item));
            }
            return sb.ToString();
        }


        private IList<TokenStmt> EventToToken(KeyEventArgs e)
        {
            var fn = RenderKeyFn(e.KeyStates);
            if (string.IsNullOrEmpty(fn))
            {
                return new List<TokenStmt>() { new TokenStmt(Token.FnCall, fn)};
            }
            return new List<TokenStmt>() { new TokenStmt(Token.FnCall, fn, new string[] { e.Key.ToString() }) };
        }

        private IList<TokenStmt> EventToToken(MouseEventArgs e)
        {
            var items = new List<TokenStmt>();
            if (e.Point != null)
            {
                items.Add(TokenStmt.Call("Move", e.Point.X - Option.BaseX, e.Point.Y - Option.BaseY));
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
                items.Add(TokenStmt.Call("Scroll", e.WheelDelta));
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

        private TokenStmt RenderMouseFn(ButtonState state, MouseButton button)
        {
            if (state == ButtonState.Pressed)
            {
                return TokenStmt.Call("MouseDown", button);
            }
            if (state == ButtonState.Released)
            {
                return TokenStmt.Call("MouseUp", button);
            }
            return new TokenStmt();
        }

        public override string ToString()
        {
            return Render(TokenItems);
        }

        public void Dispose()
        {
            waitItems.Clear();
            TokenItems.Clear();
        }
    }
}
