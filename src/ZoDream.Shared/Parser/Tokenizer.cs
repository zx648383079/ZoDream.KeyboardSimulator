using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ZoDream.Shared.Parser
{
    public class Tokenizer
    {

        public IList<Token> Parse(string content)
        {
            return Parse(new CharIterator(content));
        }

        public IList<Token> Parse(CharIterator source)
        {
            var items = new List<Token>();
            while (true)
            {
                var line = source.ReadLine();
                if (line == null)
                {
                    items.Add(new Token(TokenType.End));
                    break;
                }
                line = line.Split("//")[0].Trim();
                if (string.IsNullOrEmpty(line))
                {
                    items.Add(new Token(TokenType.End));
                    continue;
                }
                if (line.StartsWith("fn "))
                {
                    items.Add(ParseFn(line));
                    continue;
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
                    items.Add(new Token(TokenType.Delay, line));
                    continue;
                }
                ParseCall(ref items, line);
            }
            return items;
        }

        private void ParseCall(ref List<Token> items, string line)
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

        private void AddCall(ref List<Token> items, string fn, string param)
        {
            fn = fn.Trim();
            param = param.Trim();
            switch (fn.ToLower())
            {
                case "delay":
                    items.Add(new Token(TokenType.Delay, param));
                    return;
                case "if":
                    items.Add(ParseIf(param));
                    return;
                case "exit":
                    items.Add(new Token(TokenType.Exit));
                    return;
                default:
                    break;
            }
            if (string.IsNullOrWhiteSpace(param))
            {
                items.Add(new Token(TokenType.Call, fn));
                return;
            }
            var data = param.Split(',').Select(i => i.Trim()).ToArray();
            items.Add(new Token(TokenType.Call, fn, data.Length == 1 && data[0] == "" ? null : data));
        }

        private Token ParseFn(string line)
        {
            line = line.Split("//")[0];
            var i = line.IndexOf(':');
            var fn = i > 0 ? line.Substring(3, i) : line.Substring(3);
            return new Token(TokenType.Fn, fn.Trim());
        }

        private Token ParseIf(string line)
        {
            return new Token(TokenType.If, line);
        }
    }
}
