using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.OS.WinApi.Helpers
{
    internal class HookResult : IDisposable
    {
        public HookResult(HookProcedureHandle handle, HookProcedure procedure)
        {
            Handle = handle;
            Procedure = procedure;
        }

        public HookProcedureHandle Handle { get; }

        public HookProcedure Procedure { get; }

        public void Dispose()
        {
            Handle.Dispose();
        }
    }
}
