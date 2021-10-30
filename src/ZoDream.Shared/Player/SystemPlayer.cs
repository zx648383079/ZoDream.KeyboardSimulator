using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using ZoDream.Shared.Input;
using ZoDream.Shared.Player.WinApi;

namespace ZoDream.Shared.Player
{
    public class SystemPlayer : IKeyboardPlayer, IMousePlayer, IDisposable
    {
        public void Dispose()
        {
            
        }

        public void KeyDown(Key key)
        {
            var inputList = new InputBuilder().AddKeyDown(key).ToArray();
            SendSimulatedInput(inputList);
        }

        public void KeyPress(Key key)
        {
            var inputList = new InputBuilder().AddKeyPress(key).ToArray();
            SendSimulatedInput(inputList);
        }

        public void KeyPress(params Key[] keys)
        {
            var builder = new InputBuilder();
            foreach (var item in keys)
            {
                builder.AddKeyPress(item);
            }
            SendSimulatedInput(builder.ToArray());
        }

        public void KeyUp(Key key)
        {
            var inputList = new InputBuilder().AddKeyUp(key).ToArray();
            SendSimulatedInput(inputList);
        }

        public void ModifiedKeyStroke(IEnumerable<Key> modifierKeys, IEnumerable<Key> keys)
        {
            var builder = new InputBuilder();
            foreach (var item in modifierKeys)
            {
                builder.AddKeyDown(item);
            }
            foreach (var item in keys)
            {
                builder.AddKeyPress(item);
            }
            foreach (var item in modifierKeys.Reverse())
            {
                builder.AddKeyUp(item);
            }

            SendSimulatedInput(builder.ToArray());
        }

        public void ModifiedKeyStroke(IEnumerable<Key> modifierKeys, Key key)
        {
            ModifiedKeyStroke(modifierKeys, new[] { key });
        }

        public void ModifiedKeyStroke(Key modifierKey, IEnumerable<Key> keys)
        {
            ModifiedKeyStroke(new[] { modifierKey }, keys);
        }

        public void ModifiedKeyStroke(Key modifierKey, Key key)
        {
            ModifiedKeyStroke(new[] { modifierKey }, new[] { key });
        }

        public void MouseClick()
        {
            var inputList = new InputBuilder().AddMouseButtonClick(MouseButton.Left).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseDoubleClick()
        {
            var inputList = new InputBuilder().AddMouseButtonDoubleClick(MouseButton.Left).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseDown()
        {
            var inputList = new InputBuilder().AddMouseButtonDown(MouseButton.Left).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseHWheel(int value)
        {
            var inputList = new InputBuilder().AddMouseHorizontalWheelScroll(value).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseMoveTo(double x, double y)
        {
            var inputList = new InputBuilder().AddAbsoluteMouseMovement((int)Math.Truncate(x), (int)Math.Truncate(y)).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseMoveTo(Point point)
        {
            MouseMoveTo(point.X, point.Y);
        }

        public void MouseRightClick()
        {
            var inputList = new InputBuilder().AddMouseButtonClick(MouseButton.Right).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseRightDoubleClick()
        {
            var inputList = new InputBuilder().AddMouseButtonDoubleClick(MouseButton.Right).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseRightDown()
        {
            var inputList = new InputBuilder().AddMouseButtonDown(MouseButton.Right).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseRightUp()
        {
            var inputList = new InputBuilder().AddMouseButtonUp(MouseButton.Right).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseUp()
        {
            var inputList = new InputBuilder().AddMouseButtonUp(MouseButton.Left).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseWheel(int value)
        {
            var inputList = new InputBuilder().AddMouseVerticalWheelScroll(value).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseXClick(int buttonId)
        {
            var inputList = new InputBuilder().AddMouseXButtonClick(buttonId).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseXDoubleClick(int buttonId)
        {
            var inputList = new InputBuilder().AddMouseXButtonDoubleClick(buttonId).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseXDown(int buttonId)
        {
            var inputList = new InputBuilder().AddMouseXButtonDown(buttonId).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseXUp(int buttonId)
        {
            var inputList = new InputBuilder().AddMouseXButtonUp(buttonId).ToArray();
            SendSimulatedInput(inputList);
        }

        private void SendSimulatedInput(WinApi.Input[] inputs)
        {
            if (inputs.Length < 1)
            {
                return;
            }
            var successful = WinApi.InputNativeMethods.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(WinApi.Input)));
            if (successful != inputs.Length)
            {
                // 执行没有完全成功
            }
        }
    }
}
