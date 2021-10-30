using System;
using System.Collections.Generic;
using System.Text;

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
            while (source.MoveNext())
            {
                var code = source.Current;
                if (code == '\n' || code == '\r')
                {
                    continue;
                }
            }
            return items;
        }
    }
}
