using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Parser.ATS.Declarations
{
    public class Function
    {
        public readonly string Name;

        public readonly IList<IPStmt> Body = new List<IPStmt>();

        public Function(string name, IEnumerable<IPStmt> stmts)
        {
            Name = name;
            foreach (var item in stmts)
            {
                Body.Add(item);
            }
        }
    }
}
