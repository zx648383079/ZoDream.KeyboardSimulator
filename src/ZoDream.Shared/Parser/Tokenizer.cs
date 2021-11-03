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
            while (true)
            {
                var line = source.ReadLine();
                if (line == null)
                {
                    items.Add(new TokenStmt(Token.EndOfFile));
                    break;
                }
                line = line.Split("//")[0].Trim();
                if (string.IsNullOrEmpty(line))
                {
                    items.Add(new TokenStmt(Token.EndFn));
                    continue;
                }
                if (line.StartsWith("fn "))
                {
                    items.Add(ParseFn(line));
                    continue;
                }
                if (line == "else")
                {
                    items.Add(new TokenStmt(Token.Else));
                }
                if (line == "endif")
                {
                    items.Add(new TokenStmt(Token.EndIf));
                }
                if (line.StartsWith("if "))
                {
                    items.Add(ParseIf(line.Substring(3)));
                    continue;
                }
                if (line.StartsWith("if("))
                {
                    var j = line.IndexOf(')', 3);
                    items.Add(ParseIf(j < 0 ? line.Substring(3) : line.Substring(3, j - 3)));
                    continue;
                }
                if (Regex.IsMatch(line, @"^\d+$"))
                {
                    items.Add(new TokenStmt(Token.Delay, line));
                    continue;
                }
                ParseCall(ref items, line);
            }
            return items;
        }

        private void ParseCall(ref List<TokenStmt> items, string line)
        {
            var i = line.IndexOf('(');
            if (i == -1)
            {
                var temp = line.Split(' ', 2);
                if (temp.Length == 1)
                {
                    AddCall(ref items, line, string.Empty);
                    return;
                }
                AddCall(ref items, temp[0], temp[1]);
                return;
            }
            var j = line.IndexOf(')', i);
            if (j == -1)
            {
                AddCall(ref items, line.Substring(0, i), line.Substring(i + 1));
                return;
            }
            AddCall(ref items, line.Substring(0, i), line.Substring(i + 1, j - i - 1));
        }

        private void AddCall(ref List<TokenStmt> items, string fn, string param)
        {
            fn = fn.Trim();
            param = param.Trim();
            switch (fn.ToLower())
            {
                case "delay":
                    items.Add(new TokenStmt(Token.Delay, param));
                    return;
                case "if":
                    items.Add(ParseIf(param));
                    return;
                case "exit":
                    items.Add(new TokenStmt(Token.Exit));
                    return;
                default:
                    break;
            }
            if (string.IsNullOrWhiteSpace(param))
            {
                items.Add(new TokenStmt(Token.FnCall, fn));
                return;
            }
            var data = param.Split(',').Select(i => i.Trim()).ToArray();
            items.Add(new TokenStmt(Token.FnCall, fn, data.Length == 1 && data[0] == "" ? null : data));
        }

        private TokenStmt ParseFn(string line)
        {
            line = line.Split("//")[0];
            var i = line.IndexOf(':');
            var fn = i > 0 ? line.Substring(3, i) : line.Substring(3);
            return new TokenStmt(Token.Fn, fn.Trim());
        }

        private TokenStmt ParseIf(string line)
        {
            return new TokenStmt(Token.If, line);
        }
    }
}
