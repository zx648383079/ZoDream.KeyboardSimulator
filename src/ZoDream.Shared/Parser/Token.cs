using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoDream.Shared.Parser
{

    public enum Token
    {
        Fn,
        Id,
        EndOfLine,
        EndFn,
        If,
        Else,
        EndIf,
        Literal,

        String,
        Bool,
        Int,
        Float,
        Double,

        Delay,
        FnCall,
        Parameter,
        InlineComment,
        BlockComment,
        Exit,
        EndOfFile,

        // Operators
        Plus,
        Minus,
        Bang,
        Asterisk,
        Slash,

        Equal,
        NotEqual,
        LessThan,
        LessThanEqual,
        GreaterThan,
        GreaterThanEqual,

        // Delimiters
        Comma,
        Colon,
        Semicolon,
        Lparen,
        Rparen,
        Lbrace,
        Rbrace,
        Lbracket,
        Rbracket,
    }
}
