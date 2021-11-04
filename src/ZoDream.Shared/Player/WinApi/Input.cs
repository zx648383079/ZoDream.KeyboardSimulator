using System;
using System.Runtime.InteropServices;

namespace ZoDream.Shared.Player.WinApi
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Input32
    {
        [FieldOffset(0)]
        public uint Type;

        /// <summary>
        /// The <see cref="MOUSEINPUT"/> definition.
        /// </summary>
        [FieldOffset(4)]
        public MouseInput Mouse;

        /// <summary>
        /// The <see cref="KEYBDINPUT"/> definition.
        /// </summary>
        [FieldOffset(4)]
        public KeyboardInput Keyboard;

        /// <summary>
        /// The <see cref="HARDWAREINPUT"/> definition.
        /// </summary>
        [FieldOffset(4)]
        public HardwareInput Hardware;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Input
    {
        [FieldOffset(0)]
        public uint Type;

        /// <summary>
        /// The <see cref="MOUSEINPUT"/> definition.
        /// </summary>
        [FieldOffset(8)]
        public MouseInput Mouse;

        /// <summary>
        /// The <see cref="KEYBDINPUT"/> definition.
        /// </summary>
        [FieldOffset(8)]
        public KeyboardInput Keyboard;

        /// <summary>
        /// The <see cref="HARDWAREINPUT"/> definition.
        /// </summary>
        [FieldOffset(8)]
        public HardwareInput Hardware;
    }
}
