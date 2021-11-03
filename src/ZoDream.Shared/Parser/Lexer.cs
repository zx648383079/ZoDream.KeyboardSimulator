using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Parser
{
    public class Lexer
    {
        protected CharReader? Reader;
        protected int Line;
        protected Token? CurrentToken;
        protected object? CurrentValue;

        public void Open(string content)
        {
            Open(new CharReader(content));
        }

        public void Open(CharReader reader)
        {
            Reader = reader;
            Line = 1;
            NextToken();
        }



        public Token? NextToken()
        {
            var token = CurrentToken;
            CurrentToken = ReadToken();
            return token;
        }

        private Token ReadToken()
        {
            if (Reader == null || !Reader.MoveNext())
            {
                return Token.EndOfFile;
            }
            while (Reader.MoveNext())
            {
                var code = Reader.Current;
                switch (code)
                {
                    case '\t':
                    case ' ':
                        continue;
                    case '\n':
                        return Token.EndOfLine;
                    case '\r':
                        if (Reader.NextIs('\n'))
                        {
                            Reader.MoveNext();
                        }
                        return Token.EndOfLine;
                    case '/':
                        if (Reader.NextIs('/'))
                        {
                            return ReadLineComment();
                        }
                        if (Reader.NextIs('*'))
                        {
                            return ReadBlockComment();
                        }
                        break;
                    case 'f':
                        if (Reader.NextIs("n "))
                        {
                            return Token.Fn;
                        }
                        break;
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        return ReadNumberLiteral();

                }
            }
            return Token.EndOfFile;
        }


        private Token ReadNumberLiteral()
        {
            var sb = new StringBuilder();
            sb.Append(Reader.Current);
            while (Reader.MoveNext())
            {
                var code = Reader.Current;
                switch (code)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '.':
                        sb.Append(code);
                        break;
                    default:
                        Reader.Position--;
                        break;
                }
            }
            CurrentValue = Convert.ToDouble(sb.ToString());
            return Token.Literal;

        }


        private Token ReadBlockComment()
        {
            while (Reader.MoveNext())
            {
                if (Reader.Current == '*' && Reader.NextIs('/')) {
                    Reader.MoveNext();
                    return Token.EndOfLine;
                }
            }
            return Token.EndOfFile;
        }

        private Token ReadLineComment()
        {
            while (Reader.MoveNext())
            {
                var code = Reader.Current;
                if (code == '\n')
                {
                    return Token.EndOfLine;
                }
                if (code == '\r')
                {
                    if (Reader.NextIs('\n'))
                    {
                        Reader.MoveNext();
                    }
                    return Token.EndOfLine;
                }
            }
            return Token.EndOfFile;
        }

        public bool See(Token token)
        {
            return CurrentToken == token;
        }

        protected bool Eat(Token token)
        {
            if (See(token))
            {
                NextToken();
                return true;
            }
            return false;
        }

        protected void Check(Token expected)
        {
            if (!See(expected))
            {
                throw new Exception($"Expected {expected}, got {CurrentToken}");
            }
        }

        protected void Expect(Token expected)
        {
            Check(expected);
            NextToken();
        }
    }
}
