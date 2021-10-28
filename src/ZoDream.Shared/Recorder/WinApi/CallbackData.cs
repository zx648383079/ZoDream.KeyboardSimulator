using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Recorder.WinApi
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
}
