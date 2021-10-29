using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Input
{
    public class MouseEventArgs
    {
        public MouseEventArgs(MouseButton button, ButtonState buttonState, int clickCount, Point point,
            short mouseDelta, int timestamp,
            bool isHorizontalWheel)
        {
            ClickCount = clickCount;
            Timestamp = timestamp;
            IsHorizontalWheel = isHorizontalWheel;
            WheelDelta = mouseDelta;
            Change(button, buttonState);
        }

        public ButtonState LeftButton { get; private set; }

        public ButtonState MiddleButton { get; private set; }

        public ButtonState RightButton { get; private set; }

        public ButtonState XButton1 { get; private set; }

        public ButtonState XButton2 { get; private set; }

        public int ClickCount { get; private set; } = 1;

        public Point? Point { get; private set; }

        public int Timestamp { get; private set; }

        public bool IsHorizontalWheel { get; private set; } = false;

        public short WheelDelta { get; private set; }

        public ButtonState ButtonState
        {
            get
            {
                if (LeftButton != ButtonState.None)
                {
                    return LeftButton;
                }
                if (MiddleButton != ButtonState.None)
                {
                    return MiddleButton;
                }
                if  (RightButton != ButtonState.None)
                {
                    return RightButton;
                }
                if (XButton1 != ButtonState.None)
                {
                    return XButton1;
                }
                if (XButton2 != ButtonState.None)
                {
                    return XButton2;
                }
                return ButtonState.None;
            }
        }

        public void Change(MouseButton button, ButtonState buttonState)
        {
            switch (button)
            {
                case MouseButton.Left:
                    LeftButton = buttonState;
                    break;
                case MouseButton.Middle:
                    MiddleButton = buttonState;
                    break;
                case MouseButton.Right:
                    RightButton = buttonState;
                    break;
                case MouseButton.XButton1:
                    XButton1 = buttonState;
                    break;
                case MouseButton.XButton2:
                    XButton2 = buttonState;
                    break;
                default:
                    break;
            }
        }
    }
}
