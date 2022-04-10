using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoDream.Shared.Parser
{
    public class TokenStmt
    {

        public Token Type { get; set; }
        public string Content { get; set; } = string.Empty;

        public object[]? Parameters { get; set; }

        public int Line { get; set; } = -1;

        public TokenStmt() : this(Token.EndFn)
        {

        }

        public TokenStmt(Token t)
        {
            Type = t;
        }

        public TokenStmt(Token t, string val) : this(t)
        {
            Content = val;
        }

        public TokenStmt(Token t, string val, object[]? param) : this(t, val)
        {
            Parameters = param;
        }

        public static TokenStmt Call(string fn)
        {
            return new TokenStmt(Token.FnCall, fn);
        }
        public static TokenStmt Call(string fn, params object[] param)
        {
            return new TokenStmt(Token.FnCall, fn, param);
        }
    }
}
