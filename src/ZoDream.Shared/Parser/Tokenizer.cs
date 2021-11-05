using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ZoDream.Shared.Parser
{
    public class Tokenizer
    {

        public IList<TokenStmt> Parse(string content)
        {
            return Parse(new CharReader(content));
        }

        public IList<TokenStmt> Parse(CharReader source)
        {
            var items = new List<TokenStmt>();
            var lineNo = 0;
            while (true)
            {
                var line = source.ReadLine();
                lineNo ++;
                if (line == null)
                {
                    items.Add(new TokenStmt(Token.EndOfFile) { Line = lineNo });
                    break;
                }
                line = line.Split(new string[] { "//" }, StringSplitOptions.None)[0].Trim();
                if (string.IsNullOrEmpty(line))
                {
                    items.Add(new TokenStmt(Token.EndFn) { Line = lineNo });
                    continue;
                }
                if (line.StartsWith("fn "))
                {
                    items.Add(ParseFn(line, lineNo));
                    continue;
                }
                if (line == "else")
                {
                    items.Add(new TokenStmt(Token.Else) { Line = lineNo });
                }
                if (line == "endif")
                {
                    items.Add(new TokenStmt(Token.EndIf) { Line = lineNo });
                }
                if (line.StartsWith("if "))
                {
                    items.Add(ParseIf(line.Substring(3), lineNo));
                    continue;
                }
                if (line.StartsWith("if("))
                {
                    var j = line.IndexOf(')', 3);
                    items.Add(ParseIf(j < 0 ? line.Substring(3) : line.Substring(3, j - 3), lineNo));
                    continue;
                }
                if (Regex.IsMatch(line, @"^\d+$"))
                {
                    items.Add(new TokenStmt(Token.Delay, line) { Line = lineNo });
                    continue;
                }
                ParseCall(ref items, line, lineNo);
            }
            return items;
        }

        private void ParseCall(ref List<TokenStmt> items, string line, int lineNo)
        {
            var i = line.IndexOf('(');
            if (i == -1)
            {
                var temp = line.Split(new char[] { ' ' }, 2);
                if (temp.Length == 1)
                {
                    AddCall(ref items, line, string.Empty, lineNo);
                    return;
                }
                AddCall(ref items, temp[0], temp[1], lineNo);
                return;
            }
            var j = line.IndexOf(')', i);
            if (j == -1)
            {
                AddCall(ref items, line.Substring(0, i), line.Substring(i + 1), lineNo);
                return;
            }
            AddCall(ref items, line.Substring(0, i), line.Substring(i + 1, j - i - 1), lineNo);
        }

        private void AddCall(ref List<TokenStmt> items, string fn, string param, int lineNo)
        {
            fn = fn.Trim();
            param = param.Trim();
            switch (fn.ToLower())
            {
                case "delay":
                    items.Add(new TokenStmt(Token.Delay, param) { Line = lineNo});
                    return;
                case "if":
                    items.Add(ParseIf(param, lineNo));
                    return;
                case "exit":
                    items.Add(new TokenStmt(Token.Exit) { Line = lineNo });
                    return;
                default:
                    break;
            }
            if (string.IsNullOrWhiteSpace(param))
            {
                items.Add(new TokenStmt(Token.FnCall, fn) { Line = lineNo });
                return;
            }
            var data = param.Split(',').Select(i => i.Trim()).ToArray();
            items.Add(new TokenStmt(Token.FnCall, fn, data.Length == 1 && data[0] == "" ? null : data) { Line = lineNo });
        }

        private TokenStmt ParseFn(string line, int lineNo)
        {
            line = line.Split(new string[] { "//" }, StringSplitOptions.None)[0];
            var i = line.IndexOf(':');
            var fn = i > 0 ? line.Substring(3, i) : line.Substring(3);
            return new TokenStmt(Token.Fn, fn.Trim()) { Line = lineNo};
        }

        private TokenStmt ParseIf(string line, int lineNo)
        {
            var args = line.Split(new char[] { '=' }, 2);
            return new TokenStmt(Token.If, args.Length > 1 ? args[1].Trim() : string.Empty, 
                args[0].Replace('(', ' ').Replace(')', ' ').Split(',').Select(i => i.Trim()).ToArray())
            { Line = lineNo};
        }
    }
}
