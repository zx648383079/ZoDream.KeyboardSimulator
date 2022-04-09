using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Language.ATS.Statements
{
    public class IfStmt: IPStmt
    {
        public IfStmt(IPExpr condition, IList<IPStmt> thenBranch): this(condition, thenBranch, null)
        {

        }

        public IfStmt(IPExpr condition, IList<IPStmt> thenBranch, IList<IPStmt>? elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }

        public IPExpr Condition { get; }
        public IList<IPStmt> ThenBranch { get; }
        public IList<IPStmt>? ElseBranch { get; }
    }
}
