using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoDream.Shared.Parser
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Content { get; set; } = string.Empty;

        public string[]? Parameters { get; set; }

        public Token(): this(TokenType.End)
        {

        }

        public Token(TokenType t)
        {
            Type = t;
        }

        public Token(TokenType t, string val): this(t)
        {
            Content = val;
        }

        public Token(TokenType t, string val, string[]? param) : this(t, val)
        {
            Parameters = param;
        }

        public static Token Call(string fn)
        {
            return new Token(TokenType.Call, fn);
        }
        public static Token Call(string fn, params object[] param)
        {
            return new Token(TokenType.Call, fn, param.Select(i => i.ToString()).ToArray());
        }
    }

    public enum TokenType
    {
         Fn,
         If,
         IfElse,
         Delay,
         Call,
         Parameter,
         End,
         Exit,
    }
}
