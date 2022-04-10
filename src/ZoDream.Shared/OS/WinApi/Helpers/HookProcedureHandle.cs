using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.OS.WinApi.Helpers
{
    internal class HookProcedureHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal static bool Closing = false;

        public HookProcedureHandle()
            : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            //NOTE Calling Unhook during processexit causes deley
            if (Closing) return true;
            return HookNativeMethods.UnhookWindowsHookEx(handle) != 0;
        }
    }
}
