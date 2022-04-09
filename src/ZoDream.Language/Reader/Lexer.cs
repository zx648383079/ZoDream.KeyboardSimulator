using System;

namespace ZoDream.Language.Reader
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
            if (Reader == null)
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
                    case ':':
                    case ';':
                        break;
                    case '(':
                        return Token.Lparen;
                    case ')':
                        return Token.Rparen;
                    case ',':
                        return Token.Comma;
                    case '=':
                        if (Reader.NextIs('='))
                        {
                            return Token.Equal;
                        }
                        return Token.Comma;
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
                    case '0':
                        if (Reader.NextIs('x'))
                        {
                            return Read16NumberLiteral();
                        }
                        if (Reader.NextIs('b'))
                        {
                            return Read2NumberLiteral();
                        }
                        return Read8NumberLiteral();
                    case '\'':
                        return ReadStringLiteral('\'');
                    case '"':
                        return ReadStringLiteral('"');
                    default:
                        if (IsIdStart(code))
                        {
                            return ReadId();
                        }
                        break;
                }
            }
            return Token.EndOfFile;
        }

        private Token ReadId()
        {
            var start = Reader.Position;
            while (Reader.MoveNext())
            {
                var code = Reader.Current;
                if (!IsIdContinue(code))
                {
                    Reader.Position--;
                    break;
                }
            }
            CurrentValue = Reader.ReadRangeSeek(start, Reader.Position);
            switch (CurrentValue)
            {
                case "fn":
                    return Token.Fn;
                case "if":
                    return Token.Fn;
                case "else":
                    return Token.Else;
                case "endif":
                    return Token.EndIf;
                case "true":
                case "false":
                    return Token.Bool;
                default:
                    return Token.Id;
            }
        }

        private bool IsIdStart(char code)
        {
            return (code >= 'A' && code <= 'Z') ||
                (code >= 'a' && code <= 'z') ||
                code == '_' || code == '$';
        }

        private bool IsIdContinue(char code)
        {
            return (code >= 'A' && code <= 'Z') ||
                (code >= 'a' && code <= 'z') ||
                (code >= '0' && code <= '9') ||
                code == '_' || code == '$';
        }

        private Token ReadStringLiteral(char endTag)
        {
            var start = Reader.Position + 1;
            var count = 0;
            var isFind = false;
            while (Reader.MoveNext())
            {
                var code = Reader.Current;
                if (code == '\\')
                {
                    count++;
                    break;
                }
                if (code == endTag && count % 2 == 0)
                {
                    isFind = true;
                    break;
                }
                count = 0;
            }
            var end = isFind ? Reader.Position - 1 : Reader.Position;
            CurrentValue = Reader.ReadRangeSeek(start, end);
            return Token.InlineComment;
        }

        private Token Read8NumberLiteral()
        {
            var start = Reader.Position + 1;
            var hasDot = false;
            var is8 = true;
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
                        break;
                    case '8':
                    case '9':
                        is8 = false;
                        break;
                    case '.':
                        hasDot = true;
                        break;
                    default:
                        Reader.Position--;
                        break;
                }
            }
            var value = Reader.ReadRangeSeek(start, Reader.Position);
            CurrentValue = hasDot ? Convert.ToDouble(value) : Convert.ToInt32(value, is8 ? 8 : 10);
            return Token.Literal;
        }

        private Token Read2NumberLiteral()
        {
            Reader.MoveNext();
            var start = Reader.Position + 1;
            while (Reader.MoveNext())
            {
                var code = Reader.Current;
                switch (code)
                {
                    case '0':
                    case '1':
                        break;
                    default:
                        Reader.Position--;
                        break;
                }
            }
            var value = Reader.ReadRangeSeek(start, Reader.Position);
            CurrentValue = Convert.ToInt32(value, 2);
            return Token.Literal;
        }

        private Token Read16NumberLiteral()
        {
            Reader.MoveNext();
            var start = Reader.Position + 1;
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
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                        break;
                    default:
                        Reader.Position--;
                        break;
                }
            }
            var value = Reader.ReadRangeSeek(start, Reader.Position);
            CurrentValue = Convert.ToInt32(value, 16);
            return Token.Literal;
        }

        private Token ReadNumberLiteral()
        {
            var start = Reader.Position;
            var hasDot = false;
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
                        break;
                    case '.':
                        hasDot = true;
                        break;
                    default:
                        Reader.Position--;
                        break;
                }
            }
            var value = Reader.ReadRangeSeek(start, Reader.Position);
            CurrentValue = hasDot ? Convert.ToDouble(value) : Convert.ToInt32(value);
            return Token.Literal;

        }


        private Token ReadBlockComment()
        {
            Reader.MoveNext();
            var start = Reader.Position + 1;
            var isEnd = false;
            while (Reader.MoveNext())
            {
                if (Reader.Current == '*' && Reader.NextIs('/'))
                {
                    Reader.MoveNext();
                    break;
                }
            }
            var end = isEnd ? Reader.Position : Reader.Position - 2;
            CurrentValue = Reader.ReadRangeSeek(start, end);
            return Token.BlockComment;
        }

        private Token ReadLineComment()
        {
            Reader.MoveNext();
            var start = Reader.Position + 1;
            while (Reader.MoveNext())
            {
                var code = Reader.Current;
                if (code == '\n' || code == '\r')
                {
                    Reader.Position--;
                    break;
                }
            }
            var end = Reader.Position;
            CurrentValue = Reader.ReadRangeSeek(start, end);
            return Token.InlineComment;
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
