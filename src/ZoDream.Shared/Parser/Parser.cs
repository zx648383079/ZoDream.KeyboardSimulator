using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Parser.ATS;
using ZoDream.Shared.Parser.ATS.Declarations;

namespace ZoDream.Shared.Parser
{
    public class Parser: Lexer
    {
        protected Program? Program;

        public void Parse(string content)
        {
            Parse(new CharReader(content));
        }

        public void Parse(CharReader reader)
        {
            Program = new Program();
            Open(reader);
            var mainBody = new List<IPStmt>();
            while (!See(Token.EndOfFile))
            {
                switch (CurrentToken)
                {
                    case Token.Fn:
                        Program.FuncItems.Add(ParseFunc());
                        break;
                    case Token.EndFn:
                        break;

                    default:
                        break;
                }
            }
        }

        public Function ParseFunc()
        {
            Expect(Token.Fn);
            var id = CurrentValue as string;
            Expect(Token.Id);
            var body = new List<IPStmt>();
            while (!See(Token.EndFn) && !See(Token.EndOfFile))
            {

            }
            return new Function(id, body);
        }
    }
}
