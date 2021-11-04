﻿using System.Runtime.InteropServices;

namespace ZoDream.Shared.Player.WinApi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HardwareInput
    {
        /// <summary>
        /// Value specifying the message generated by the input hardware. 
        /// </summary>
        public uint Msg;

        /// <summary>
        /// Specifies the low-order word of the lParam parameter for uMsg. 
        /// </summary>
        public ushort ParamL;

        /// <summary>
        /// Specifies the high-order word of the lParam parameter for uMsg. 
        /// </summary>
        public ushort ParamH;
    }
}