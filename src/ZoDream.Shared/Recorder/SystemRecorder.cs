using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ZoDream.Shared.Input;
using ZoDream.Shared.Recorder.WinApi;

namespace ZoDream.Shared.Recorder
{
    public class SystemRecorder : IRecorder
    {
        private bool paused = true;
        private bool booted = false;
        private Point lastPoint = new Point(-1, -1);
        private Point lastDragPoint = new Point(-1, -1);
        /// <summary>
        /// 记录还处于按下状态的按键
        /// </summary>
        private IList<MouseButton> singleDown = new List<MouseButton>();
        private IList<MouseButton> doubleDown = new List<MouseButton>();
        private HookResult? MouseHande;
        private HookResult? KeyboardHande;

        public event KeyEventHandler? KeyDown;
        public event KeyEventHandler? KeyUp;
        public event KeyEventHandler? KeyPress;
        public event KeyEventHandler? OnKey;

        public event MouseEventHandler? MouseMove;
        public event MouseEventHandler? MouseUp;
        public event MouseEventHandler? MouseDown;
        public event MouseEventHandler? MouseClick;
        public event MouseEventHandler? MouseWheel;
        public event MouseEventHandler? MouseHWheel;
        public event MouseEventHandler? MouseDoubleClick;
        public event MouseEventHandler? MouseDragStarted;
        public event MouseEventHandler? MouseDragFinished;
        public event MouseEventHandler? OnMouse;

        public void Start()
        {
            paused = false;
            Boot();
        }

        public void Stop()
        {
            paused = true;
        }

        private void Boot()
        {
            if (booted)
            {
                return;
            }
            booted = true;
            KeyboardHande = HookHelper.HookGlobalKeyboard(KeyboardCallback);
            MouseHande = HookHelper.HookGlobalMouse(MouseCallback);
        }

        protected bool KeyboardCallback(KeyEventArgs args)
        {
            if (!paused)
            {
                OnKey?.Invoke(this, args);
                if (args.KeyStates == ButtonState.Pressed)
                {
                    KeyDown?.Invoke(this, args);
                    KeyPress?.Invoke(this, args);
                }
                if (args.KeyStates == ButtonState.Released)
                {
                    KeyUp?.Invoke(this, args);
                }
            }
            return true;
        }

        protected bool MouseCallback(MouseEventArgs args)
        {
            if (!paused)
            {
                OnMouse?.Invoke(this, args);
                if (args.WheelDelta != 0)
                {
                    if (args.IsHorizontalWheel)
                    {
                        MouseHWheel?.Invoke(this, args);
                    } else
                    {
                        MouseWheel?.Invoke(this, args);
                    }
                }
                switch (args.ButtonState)
                {
                    case ButtonState.Released:
                        MouseUp?.Invoke(this, args);
                        break;
                    case ButtonState.Pressed:
                        MouseDown?.Invoke(this, args);
                        break;
                    default:
                        break;
                }

                var isDouble = args.ClickCount > 1;
                ToggleMouseButton(args.LeftButton, MouseButton.Left, isDouble);
                ToggleMouseButton(args.RightButton, MouseButton.Right, isDouble);
                ToggleMouseButton(args.MiddleButton, MouseButton.Middle, isDouble);
                ToggleMouseButton(args.XButton1, MouseButton.XButton1, isDouble);
                ToggleMouseButton(args.XButton2, MouseButton.XButton2, isDouble);

                if (args.Point != null && args.Point != lastPoint)
                {
                    MouseMove?.Invoke(this, args);
                }

            }
            return true;
        }

        private void ToggleMouseButton(ButtonState buttonState, MouseButton button, bool isDouble = false)
        {
            switch (buttonState)
            {
                case ButtonState.Released:
                    singleDown.Remove(button);
                    doubleDown.Remove(button);
                    break;
                case ButtonState.Pressed:
                    if (isDouble)
                    {
                        doubleDown.Add(button);
                    }
                    else
                    {
                        singleDown.Add(button);
                    }
                    break;
                default:
                    break;
            }
        }

        public void Dispose()
        {
            MouseHande?.Dispose();
            KeyboardHande?.Dispose();
            HookProcedureHandle.Closing = true;
        }
    }
}
