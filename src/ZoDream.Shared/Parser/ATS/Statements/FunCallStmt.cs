using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Parser.ATS.Statements
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
