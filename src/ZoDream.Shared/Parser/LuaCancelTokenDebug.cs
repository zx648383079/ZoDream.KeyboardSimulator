using Neo.IronLua;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

namespace ZoDream.Shared.Parser
{
    public class LuaCancelTokenDebug : ILuaDebug
    {

        public LuaCancelTokenDebug(CancellationToken token)
        {
            CancellationToken = token;
        }

        private ILuaDebug BaseDebug = LuaExceptionDebugger.Default;
        private CancellationToken CancellationToken;
        public LuaDebugLevel Level => BaseDebug.Level;

        public LuaChunk CreateChunk(Lua lua, LambdaExpression expr)
        {
            if (CancellationToken.IsCancellationRequested)
            {
                throw new LuaCancelTokenException();
            }
            return BaseDebug.CreateChunk(lua, expr);
        }
    }

    public class LuaCancelTokenException: Exception
    {
        public LuaCancelTokenException(): base("Interrupt execution")
        {
        }
    }
}
