using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using ZoDream.Shared.Input;
using ZoDream.Shared.Player.WinApi;
using System.Diagnostics;
using System.Threading;
using System.Drawing;

namespace ZoDream.Shared.Player
{
    public class SystemPlayer : IPlayer
    {

        private IntPtr windowHandle = IntPtr.Zero;

        public void Dispose()
        {
        }

        public void KeyDown(Key key)
        {
            //if (windowHandle != IntPtr.Zero)
            //{
            //    WindowNativeMethods.PostMessage(windowHandle, WindowNativeMethods.WM_KEYDOWN, (uint)key, 0);
            //    return;
            //}
            var inputList = new InputBuilder().AddKeyDown(key).ToArray();
            SendSimulatedInput(inputList);
        }

        public void KeyPress(Key key)
        {
            //if (windowHandle != IntPtr.Zero)
            //{
            //    KeyDown(key);
            //    KeyUp(key);
            //    return;
            //}
            var inputList = new InputBuilder().AddKeyPress(key).ToArray();
            SendSimulatedInput(inputList);
            //var builder = new InputBuilder();
            //var inputList = builder.AddKeyDown(key).ToArray();
            //SendSimulatedInput(inputList);
            //SendSimulatedInput(builder.RenderKeyUp(inputList));

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
            //if (windowHandle != IntPtr.Zero)
            //{
            //    WindowNativeMethods.PostMessage(windowHandle, WindowNativeMethods.WM_KEYUP, (uint)key, 0);
            //    return;
            //}
            var inputList = new InputBuilder().AddKeyUp(key).ToArray();
            SendSimulatedInput(inputList);
        }

        public void KeyDown(ushort key)
        {
            var inputList = new InputBuilder().AddKeyDown(key).ToArray();
            SendSimulatedInput(inputList);
        }
        public void KeyPress(ushort key)
        {
            var inputList = new InputBuilder().AddKeyDown(key).AddKeyUp(key).ToArray();
            SendSimulatedInput(inputList);

        }
        public void KeyUp(ushort key)
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

        public void KeyStroke(params Key[] keys)
        {
            var builder = new InputBuilder();
            foreach (var item in keys)
            {
                builder.AddKeyDown(item);
            }
            foreach (var item in keys.Reverse())
            {
                builder.AddKeyUp(item);
            }
            SendSimulatedInput(builder.ToArray());
        }

        public void MouseClick()
        {
            MouseClick(MouseButton.Left);
        }

        public void MouseDoubleClick()
        {
            MouseDoubleClick(MouseButton.Left);
        }

        public void MouseDown()
        {
            MouseDown(MouseButton.Left);
        }

        public void MouseDown(MouseButton button)
        {
            var inputList = new InputBuilder().AddMouseButtonDown(button).ToArray();
            SendSimulatedInput(inputList);
        }
        public void MouseUp(MouseButton button)
        {
            var inputList = new InputBuilder().AddMouseButtonUp(button).ToArray();
            SendSimulatedInput(inputList);
        }
        public void MouseClick(MouseButton button)
        {
            var inputList = new InputBuilder().AddMouseButtonClick(button).ToArray();
            SendSimulatedInput(inputList);
        }
        public void MouseDoubleClick(MouseButton button)
        {
            var inputList = new InputBuilder().AddMouseButtonDoubleClick(button).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseHWheel(int value)
        {
            var inputList = new InputBuilder().AddMouseHorizontalWheelScroll(value).ToArray();
            SendSimulatedInput(inputList);
        }

        public void MouseMoveTo(double x, double y)
        {
            var rc = WindowNativeMethods.VirtualScreen;
            var fx = x * (0xFFFF / rc.Width);
            var fy = y * (0xFFFF / rc.Height);
            var inputList = new InputBuilder().AddAbsoluteMouseMovement((int)fx, (int)fy).ToArray();
            SendSimulatedInput(inputList);
            // MouseNativeMethods.MoveTo((int)x, (int)y);
        }

        public void MouseMoveTo(Point point)
        {
            MouseMoveTo(point.X, point.Y);
        }

        public void MouseRightClick()
        {
            MouseClick(MouseButton.Right);
        }

        public void MouseRightDoubleClick()
        {
            MouseDoubleClick(MouseButton.Right);
        }

        public void MouseRightDown()
        {
            MouseDown(MouseButton.Right);
        }

        public void MouseRightUp()
        {
            MouseUp(MouseButton.Right);
        }

        public void MouseUp()
        {
            MouseUp(MouseButton.Left);
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
            var successful = InputNativeMethods.SendInput(inputs);
            if (successful != inputs.Length)
            {
                // 执行没有完全成功
            }
        }

        public void Focus(string windowName)
        {
            windowHandle = WindowNativeMethods.FindWindow(null, windowName);
            if (windowHandle == IntPtr.Zero)
            {
                throw new Exception($"Can not get Window: {windowName}");
            }
            WindowNativeMethods.SetForegroundWindow(windowHandle);
        }

        public ushort GetScanKey(Key key)
        {
            return (ushort)InputNativeMethods.MapVirtualKey((uint)key, (uint)MappingType.VK_TO_VSC);
        }

        public void LostFocus()
        {
            windowHandle = IntPtr.Zero;
        }
    }
}
