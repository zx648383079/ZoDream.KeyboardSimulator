using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using ZoDream.Shared.Input;
using ZoDream.Shared.OS.WinApi.Models;

namespace ZoDream.Shared.OS.WinApi.Helpers
{
    internal static class DataFormat
    {
        public static MouseEventArgs FormatMouse(CallbackData data)
        {
            var wParam = data.WParam;
            var lParam = data.LParam;

            var marshalledMouseStruct = (MouseStruct)Marshal.PtrToStructure(lParam, typeof(MouseStruct));
            return FormatMouseUniversal(wParam, marshalledMouseStruct);
        }

        private static MouseEventArgs FormatMouseUniversal(IntPtr wParam, MouseStruct mouseInfo)
        {
            var button = MouseButton.None;
            short mouseDelta = 0;
            var clickCount = 0;

            var buttonState = ButtonState.None;
            var isHorizontalWheel = false;


            switch ((long)wParam)
            {
                case Messages.WM_LBUTTONDOWN:
                    buttonState = ButtonState.Pressed;
                    button = MouseButton.Left;
                    clickCount = 1;
                    break;
                case Messages.WM_LBUTTONUP:
                    buttonState = ButtonState.Released;
                    button = MouseButton.Left;
                    clickCount = 1;
                    break;
                case Messages.WM_LBUTTONDBLCLK:
                    buttonState = ButtonState.Pressed;
                    button = MouseButton.Left;
                    clickCount = 2;
                    break;
                case Messages.WM_RBUTTONDOWN:
                    buttonState = ButtonState.Pressed;
                    button = MouseButton.Right;
                    clickCount = 1;
                    break;
                case Messages.WM_RBUTTONUP:
                    buttonState = ButtonState.Released;
                    button = MouseButton.Right;
                    clickCount = 1;
                    break;
                case Messages.WM_RBUTTONDBLCLK:
                    buttonState = ButtonState.Pressed;
                    button = MouseButton.Right;
                    clickCount = 2;
                    break;
                case Messages.WM_MBUTTONDOWN:
                    buttonState = ButtonState.Pressed;
                    button = MouseButton.Middle;
                    clickCount = 1;
                    break;
                case Messages.WM_MBUTTONUP:
                    buttonState = ButtonState.Released;
                    button = MouseButton.Middle;
                    clickCount = 1;
                    break;
                case Messages.WM_MBUTTONDBLCLK:
                    buttonState = ButtonState.Pressed;
                    button = MouseButton.Middle;
                    clickCount = 2;
                    break;
                case Messages.WM_MOUSEWHEEL:
                    isHorizontalWheel = false;
                    mouseDelta = mouseInfo.MouseData;
                    break;
                case Messages.WM_MOUSEHWHEEL:
                    isHorizontalWheel = true;
                    mouseDelta = mouseInfo.MouseData;
                    break;
                case Messages.WM_XBUTTONDOWN:
                    button = mouseInfo.MouseData == 1
                        ? MouseButton.XButton1
                        : MouseButton.XButton2;
                    buttonState = ButtonState.Pressed;
                    clickCount = 1;
                    break;
                case Messages.WM_XBUTTONUP:
                    button = mouseInfo.MouseData == 1
                        ? MouseButton.XButton1
                        : MouseButton.XButton2;
                    buttonState = ButtonState.Released;
                    clickCount = 1;
                    break;
                case Messages.WM_XBUTTONDBLCLK:
                    buttonState = ButtonState.Pressed;
                    button = mouseInfo.MouseData == 1
                        ? MouseButton.XButton1
                        : MouseButton.XButton2;
                    clickCount = 2;
                    break;
            }

            var e = new MouseEventArgs(
                button,
                buttonState,
                clickCount,
                new Point(mouseInfo.Point.X, mouseInfo.Point.Y),
                mouseDelta,
                mouseInfo.Timestamp,
                isHorizontalWheel);

            return e;
        }

        public static KeyEventArgs FormatKeybord(CallbackData data)
        {
            var wParam = data.WParam;
            var lParam = data.LParam;
            var keyboardHookStruct =
                (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

            var keyData = AppendModifierStates((Key)keyboardHookStruct.VirtualKeyCode);

            var keyCode = (int)wParam;
            var state = ButtonState.None;

            switch (keyCode)
            {
                case Messages.WM_KEYDOWN:
                case Messages.WM_SYSKEYDOWN:
                    state = ButtonState.Pressed;
                    break;
                case Messages.WM_KEYUP:
                case Messages.WM_SYSKEYUP:
                    state = ButtonState.Released;
                    break;
            }

            const uint maskExtendedKey = 0x1;
            var isExtendedKey = (keyboardHookStruct.Flags & maskExtendedKey) > 0;

            return new KeyEventArgs(keyData, state, keyboardHookStruct.Time, isExtendedKey);
        }

        // # It is not possible to distinguish Keys.LControlKey and Keys.RControlKey when they are modifiers
        // Check for Keys.Control instead
        // Same for Shift and Alt(Menu)
        // See more at http://www.tech-archive.net/Archive/DotNet/microsoft.public.dotnet.framework.windowsforms/2008-04/msg00127.html #

        // A shortcut to make life easier
        private static bool CheckModifier(int vKey)
        {
            return (KeyboardNativeMethods.GetKeyState(vKey) & 0x8000) > 0;
        }

        private static Key AppendModifierStates(Key keyData)
        {
            // Is Control being held down?
            var control = CheckModifier(KeyboardNativeMethods.VK_CONTROL);
            // Is Shift being held down?
            var shift = CheckModifier(KeyboardNativeMethods.VK_SHIFT);
            // Is Alt being held down?
            var alt = CheckModifier(KeyboardNativeMethods.VK_MENU);

            // Windows keys
            // # combine LWin and RWin key with other keys will potentially corrupt the data
            // notable F5 | Keys.LWin == F12, see https://globalmousekeyhook.codeplex.com/workitem/1188
            // and the KeyEventArgs.KeyData don't recognize combined data either

            // Function (Fn) key
            // # CANNOT determine state due to conversion inside keyboard
            // See http://en.wikipedia.org/wiki/Fn_key#Technical_details #

            return keyData |
                   (control ? Key.LeftCtrl : Key.None) |
                   (shift ? Key.LeftShift : Key.None) |
                   (alt ? Key.LeftAlt : Key.None);
        }

    }
}
