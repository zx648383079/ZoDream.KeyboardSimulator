using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Language.ATS.Statements
{
    public class FunCallStmt: IPStmt
    {

        public FunCallStmt(string func, IReadOnlyList<IPExpr> args)
        {
            Function = func;
            ArgsItems = args;
        }

        public string Function { get; }
        public IReadOnlyList<IPExpr> ArgsItems { get; }
    }
}
