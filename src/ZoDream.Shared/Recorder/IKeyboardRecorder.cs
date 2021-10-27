using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Input;

namespace ZoDream.Shared.Recorder
{
    public interface IKeyboardRecorder
    {
        public event KeyEventHandler? KeyDown;

        public event KeyEventHandler? KeyUp;

        public event KeyEventHandler? KeyPress;
    }
}
