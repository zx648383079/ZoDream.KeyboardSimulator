using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Input;

namespace ZoDream.Shared.Recorder
{
    public class SystemRecorder : IKeyboardRecorder, IMouseRecorder, IDisposable
    {
        public event KeyEventHandler? KeyDown;
        public event KeyEventHandler? KeyUp;
        public event KeyEventHandler? KeyPress;
        public event MouseEventHandler? MouseMove;
        public event MouseEventHandler? MouseUp;
        public event MouseEventHandler? MouseDown;
        public event MouseEventHandler? MouseClick;
        public event MouseEventHandler? MouseWheel;
        public event MouseEventHandler? MouseHWheel;
        public event MouseEventHandler? MouseDoubleClick;
        public event MouseEventHandler? MouseDragStarted;
        public event MouseEventHandler? MouseDragFinished;

        public void Dispose()
        {
            
        }
    }
}
