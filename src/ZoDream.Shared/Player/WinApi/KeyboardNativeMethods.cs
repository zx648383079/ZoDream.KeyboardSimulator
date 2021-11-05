using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ZoDream.Shared.Input;

namespace ZoDream.Shared.Player.WinApi
{
    public static class KeyboardNativeMethods
    {

        [DllImport("user32.dll")] 
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);


        public static void KeyDown(Key key)
        {
            keybd_event((byte)key, (byte)InputNativeMethods.MapVirtualKey((uint)key,
                (uint)MappingType.VK_TO_VSC), 0x0001 | 0, 0);
        }

        public static void KeyUp(Key key)
        {
            keybd_event((byte)key, (byte)InputNativeMethods.MapVirtualKey((uint)key,
                (uint)MappingType.VK_TO_VSC), 0x0001 | 0x0002, 0);
        }
    }
}
