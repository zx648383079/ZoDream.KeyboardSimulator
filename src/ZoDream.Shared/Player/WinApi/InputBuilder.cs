using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Input;

namespace ZoDream.Shared.Player.WinApi
{
    internal class InputBuilder: IEnumerable<Input>
    {
        /// <summary>
        /// The public list of <see cref="Input"/> messages being built by this instance.
        /// </summary>
        private readonly List<Input> inputItems = new List<Input>();

        /// <summary>
        /// Returns the list of <see cref="Input"/> messages as a <see cref="System.Array"/> of <see cref="Input"/> messages.
        /// </summary>
        /// <returns>The <see cref="System.Array"/> of <see cref="Input"/> messages.</returns>
        public Input[] ToArray()
        {
            return inputItems.ToArray();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the list of <see cref="Input"/> messages.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the list of <see cref="Input"/> messages.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Input> GetEnumerator()
        {
            return inputItems.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the list of <see cref="Input"/> messages.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the list of <see cref="Input"/> messages.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator<Input> IEnumerable<Input>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the <see cref="Input"/> at the specified position.
        /// </summary>
        /// <value>The <see cref="Input"/> message at the specified position.</value>
        public Input this[int position]
        {
            get
            {
                return inputItems[position];
            }
        }

        /// <summary>
        /// Determines if the <see cref="Key"/> is an ExtendedKey
        /// </summary>
        /// <param name="keyCode">The key code.</param>
        /// <returns>true if the key code is an extended key; otherwise, false.</returns>
        /// <remarks>
        /// The extended keys consist of the ALT and CTRL keys on the right-hand side of the keyboard; the INS, DEL, HOME, END, PAGE UP, PAGE DOWN, and arrow keys in the clusters to the left of the numeric keypad; the NUM LOCK key; the BREAK (CTRL+PAUSE) key; the PRINT SCRN key; and the divide (/) and ENTER keys in the numeric keypad.
        /// 
        /// See http://msdn.microsoft.com/en-us/library/ms646267(v=vs.85).aspx Section "Extended-Key Flag"
        /// </remarks>
        public static bool IsExtendedKey(Key keyCode)
        {
            if (keyCode == Key.LeftAlt ||
                keyCode == Key.RightAlt ||
                keyCode == Key.LeftCtrl ||
                keyCode == Key.RightCtrl ||
                keyCode == Key.Insert ||
                keyCode == Key.Delete ||
                keyCode == Key.Home ||
                keyCode == Key.End ||
                keyCode == Key.Print ||
                keyCode == Key.Next ||
                keyCode == Key.Right ||
                keyCode == Key.Up ||
                keyCode == Key.Left ||
                keyCode == Key.Down ||
                keyCode == Key.NumLock ||
                keyCode == Key.Cancel ||
                keyCode == Key.Snapshot ||
                keyCode == Key.Divide)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a key down to the list of <see cref="Input"/> messages.
        /// </summary>
        /// <param name="keyCode">The <see cref="Key"/>.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddKeyDown(Key keyCode)
        {
            var ki = RenderKey(keyCode, false);
            var down = new Input
            {
                Type = (uint)InputType.Keyboard,
                Keyboard = ki
            };
            inputItems.Add(down);
            return this;
        }

        /// <summary>
        /// Adds a key up to the list of <see cref="Input"/> messages.
        /// </summary>
        /// <param name="keyCode">The <see cref="Key"/>.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddKeyUp(Key keyCode)
        {
            var ki = RenderKeyUp(RenderKey(keyCode, false), false);
            var up = new Input
                {
                    Type = (uint)InputType.Keyboard,
                    Keyboard = ki,
                };
            inputItems.Add(up);
            return this;
        }

        public InputBuilder AddKeyUp(ushort key)
        {
            var down = new Input
            {
                Type = (uint)InputType.Keyboard,
                Keyboard = new KeyboardInput
                {
                    KeyCode = 0,
                    Scan = (ushort)(key & 0xff),
                    Flags = (uint)(KeyboardFlag.KeyUp | KeyboardFlag.ScanCode),
                    Time = 0,
                    ExtraInfo = InputNativeMethods.GetMessageExtraInfo()
                }
            };
            inputItems.Add(down);
            return this;
        }

        public InputBuilder AddKeyDown(ushort key)
        {
            var down = new Input
            {
                Type = (uint)InputType.Keyboard,
                Keyboard = new KeyboardInput
                {
                    KeyCode = 0,
                    Scan = (ushort)(key & 0xff),
                    Flags = (uint)(KeyboardFlag.KeyDown | KeyboardFlag.ScanCode),
                    Time = 0,
                    ExtraInfo = InputNativeMethods.GetMessageExtraInfo()
                }
            };
            inputItems.Add(down);
            return this;
        }

        public KeyboardInput RenderKey(Key key, bool useScanCode = false)
        {
            var flag = (uint)(IsExtendedKey(key) ? KeyboardFlag.ExtendedKey : 0);
            if (useScanCode)
            {
                flag |= (uint)KeyboardFlag.ScanCode;
            }
            return new KeyboardInput
            {
                KeyCode = (ushort)(useScanCode ? 0 : key),
                Scan = (ushort)(useScanCode ? InputNativeMethods.MapVirtualKey((uint)key, 
                (uint)MappingType.VK_TO_VSC) & 0xFFU : 0),
                Flags = flag,
                Time = 0,
                ExtraInfo = IntPtr.Zero
            };
        }

        public KeyboardInput RenderKeyUp(KeyboardInput input, bool useScanCode = true)
        {
            input.Flags = (uint)KeyboardFlag.KeyUp | (uint)(useScanCode ? KeyboardFlag.ScanCode : 0);
            return input;
        }

        public Input RenderKeyUp(Input input)
        {
            input.Keyboard.Flags |= (uint)KeyboardFlag.KeyUp;
            return input;
        }

        public Input[] RenderKeyUp(Input[] input)
        {
            foreach (var item in input)
            {
                RenderKeyUp(item);
            }
            return input;
        }

        /// <summary>
        /// Adds a key press to the list of <see cref="Input"/> messages which is equivalent to a key down followed by a key up.
        /// </summary>
        /// <param name="keyCode">The <see cref="Key"/>.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddKeyPress(Key keyCode)
        {
            AddKeyDown(keyCode);
            AddKeyUp(keyCode);
            return this;
        }

        /// <summary>
        /// Adds the character to the list of <see cref="Input"/> messages.
        /// </summary>
        /// <param name="character">The <see cref="System.Char"/> to be added to the list of <see cref="Input"/> messages.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddCharacter(char character)
        {
            ushort scanCode = character;

            var down = new Input
            {
                Type = (uint)InputType.Keyboard,
                Keyboard = new KeyboardInput
                {
                    KeyCode = 0,
                    Scan = scanCode,
                    Flags = (uint)KeyboardFlag.Unicode,
                    Time = 0,
                    ExtraInfo = IntPtr.Zero
                }
            };

            var up = new Input
            {
                Type = (uint)InputType.Keyboard,
                Keyboard = new KeyboardInput
                {
                    KeyCode = 0,
                    Scan = scanCode,
                    Flags =
                            (uint)(KeyboardFlag.KeyUp | KeyboardFlag.Unicode),
                    Time = 0,
                    ExtraInfo = IntPtr.Zero
                }
            };

            // Handle extended keys:
            // If the scan code is preceded by a prefix byte that has the value 0xE0 (224),
            // we need to include the KEYEVENTF_EXTENDEDKEY flag in the Flags property. 
            if ((scanCode & 0xFF00) == 0xE000)
            {
                down.Keyboard.Flags |= (uint)KeyboardFlag.ExtendedKey;
                up.Keyboard.Flags |= (uint)KeyboardFlag.ExtendedKey;
            }

            inputItems.Add(down);
            inputItems.Add(up);
            return this;
        }

        /// <summary>
        /// Adds all of the characters in the specified <see cref="IEnumerable{T}"/> of <see cref="char"/>.
        /// </summary>
        /// <param name="characters">The characters to add.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddCharacters(IEnumerable<char> characters)
        {
            foreach (var character in characters)
            {
                AddCharacter(character);
            }
            return this;
        }

        /// <summary>
        /// Adds the characters in the specified <see cref="string"/>.
        /// </summary>
        /// <param name="characters">The string of <see cref="char"/> to add.</param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddCharacters(string characters)
        {
            return AddCharacters(characters.ToCharArray());
        }

        /// <summary>
        /// Moves the mouse relative to its current position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddRelativeMouseMovement(int x, int y)
        {
            var movement = new Input { Type = (uint)InputType.Mouse };
            movement.Mouse.Flags = (uint)MouseFlag.Move;
            movement.Mouse.X = x;
            movement.Mouse.Y = y;

            inputItems.Add(movement);

            return this;
        }

        /// <summary>
        /// Move the mouse to an absolute position.
        /// </summary>
        /// <param name="absoluteX"></param>
        /// <param name="absoluteY"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddAbsoluteMouseMovement(int absoluteX, int absoluteY)
        {
            var movement = new Input { Type = (uint)InputType.Mouse };
            movement.Mouse.Flags = (uint)(MouseFlag.Move | MouseFlag.Absolute);
            movement.Mouse.X = absoluteX;
            movement.Mouse.Y = absoluteY;

            inputItems.Add(movement);

            return this;
        }

        /// <summary>
        /// Move the mouse to the absolute position on the virtual desktop.
        /// </summary>
        /// <param name="absoluteX"></param>
        /// <param name="absoluteY"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddAbsoluteMouseMovementOnVirtualDesktop(int absoluteX, int absoluteY)
        {
            var movement = new Input { Type = (uint)InputType.Mouse };
            movement.Mouse.Flags = (uint)(MouseFlag.Move | MouseFlag.Absolute | MouseFlag.VirtualDesk);
            movement.Mouse.X = absoluteX;
            movement.Mouse.Y = absoluteY;

            inputItems.Add(movement);

            return this;
        }

        /// <summary>
        /// Adds a mouse button down for the specified button.
        /// </summary>
        /// <param name="button"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseButtonDown(MouseButton button)
        {
            var buttonDown = new Input { Type = (uint)InputType.Mouse };
            buttonDown.Mouse.Flags = (uint)ToMouseButtonDownFlag(button);

            inputItems.Add(buttonDown);

            return this;
        }

        /// <summary>
        /// Adds a mouse button down for the specified button.
        /// </summary>
        /// <param name="xButtonId"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseXButtonDown(int xButtonId)
        {
            var buttonDown = new Input { Type = (uint)InputType.Mouse };
            buttonDown.Mouse.Flags = (uint)MouseFlag.XDown;
            buttonDown.Mouse.MouseData = (uint)xButtonId;
            inputItems.Add(buttonDown);

            return this;
        }

        /// <summary>
        /// Adds a mouse button up for the specified button.
        /// </summary>
        /// <param name="button"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseButtonUp(MouseButton button)
        {
            var buttonUp = new Input { Type = (uint)InputType.Mouse };
            buttonUp.Mouse.Flags = (uint)ToMouseButtonUpFlag(button);
            inputItems.Add(buttonUp);

            return this;
        }

        /// <summary>
        /// Adds a mouse button up for the specified button.
        /// </summary>
        /// <param name="xButtonId"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseXButtonUp(int xButtonId)
        {
            var buttonUp = new Input { Type = (uint)InputType.Mouse };
            buttonUp.Mouse.Flags = (uint)MouseFlag.XUp;
            buttonUp.Mouse.MouseData = (uint)xButtonId;
            inputItems.Add(buttonUp);

            return this;
        }

        /// <summary>
        /// Adds a single click of the specified button.
        /// </summary>
        /// <param name="button"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseButtonClick(MouseButton button)
        {
            return AddMouseButtonDown(button).AddMouseButtonUp(button);
        }

        /// <summary>
        /// Adds a single click of the specified button.
        /// </summary>
        /// <param name="xButtonId"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseXButtonClick(int xButtonId)
        {
            return AddMouseXButtonDown(xButtonId).AddMouseXButtonUp(xButtonId);
        }

        /// <summary>
        /// Adds a double click of the specified button.
        /// </summary>
        /// <param name="button"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseButtonDoubleClick(MouseButton button)
        {
            return AddMouseButtonClick(button).AddMouseButtonClick(button);
        }

        /// <summary>
        /// Adds a double click of the specified button.
        /// </summary>
        /// <param name="xButtonId"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseXButtonDoubleClick(int xButtonId)
        {
            return AddMouseXButtonClick(xButtonId).AddMouseXButtonClick(xButtonId);
        }

        /// <summary>
        /// Scroll the vertical mouse wheel by the specified amount.
        /// </summary>
        /// <param name="scrollAmount"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseVerticalWheelScroll(int scrollAmount)
        {
            var scroll = new Input { Type = (uint)InputType.Mouse };
            scroll.Mouse.Flags = (uint)MouseFlag.VerticalWheel;
            scroll.Mouse.MouseData = (uint)scrollAmount;

            inputItems.Add(scroll);

            return this;
        }

        /// <summary>
        /// Scroll the horizontal mouse wheel by the specified amount.
        /// </summary>
        /// <param name="scrollAmount"></param>
        /// <returns>This <see cref="InputBuilder"/> instance.</returns>
        public InputBuilder AddMouseHorizontalWheelScroll(int scrollAmount)
        {
            var scroll = new Input { Type = (uint)InputType.Mouse };
            scroll.Mouse.Flags = (uint)MouseFlag.HorizontalWheel;
            scroll.Mouse.MouseData = (uint)scrollAmount;

            inputItems.Add(scroll);

            return this;
        }

        private static MouseFlag ToMouseButtonDownFlag(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return MouseFlag.LeftDown;

                case MouseButton.Middle:
                    return MouseFlag.MiddleDown;

                case MouseButton.Right:
                    return MouseFlag.RightDown;

                default:
                    return MouseFlag.LeftDown;
            }
        }

        private static MouseFlag ToMouseButtonUpFlag(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return MouseFlag.LeftUp;

                case MouseButton.Middle:
                    return MouseFlag.MiddleUp;

                case MouseButton.Right:
                    return MouseFlag.RightUp;

                default:
                    return MouseFlag.LeftUp;
            }
        }

        
    }
}
