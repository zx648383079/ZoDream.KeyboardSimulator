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
         Delay,
         FnCall,
         Parameter,
         Exit,
         EndOfFile,
    }
}
