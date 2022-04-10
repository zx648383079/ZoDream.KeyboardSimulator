using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Input;

namespace ZoDream.Shared.OS.WinApi.Helpers
{
    internal class CallbackData
    {
        public CallbackData(IntPtr wParam, IntPtr lParam)
        {
            WParam = wParam;
            LParam = lParam;
        }

        public IntPtr WParam { get; }

        public IntPtr LParam { get; }
    }

    internal delegate bool Callback(CallbackData data);

    internal delegate bool MouseCallback(MouseEventArgs args);
    internal delegate bool KeyboardCallback(KeyEventArgs args);
}
