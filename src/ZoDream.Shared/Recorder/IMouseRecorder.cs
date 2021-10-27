using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Input;

namespace ZoDream.Shared.Recorder
{
    public interface IMouseRecorder
    {
        public event MouseEventHandler? MouseMove;
        public event MouseEventHandler? MouseUp;
        public event MouseEventHandler? MouseDown;
        public event MouseEventHandler? MouseClick;
        public event MouseEventHandler? MouseWheel;
        public event MouseEventHandler? MouseHWheel;
        public event MouseEventHandler? MouseDoubleClick;
        public event MouseEventHandler? MouseDragStarted;
        public event MouseEventHandler? MouseDragFinished;

    }
}
