using System;
using System.Runtime.InteropServices;

namespace ZoDream.Shared.OS.WinApi.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public struct InputStruct32
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
    public struct InputStruct
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
