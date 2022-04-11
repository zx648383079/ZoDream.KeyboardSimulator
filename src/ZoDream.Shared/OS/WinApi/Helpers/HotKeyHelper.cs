using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Input;

namespace ZoDream.Shared.OS.WinApi.Helpers
{
    public class HotKeyHelper
    {
        public HotKeyHelper(IntPtr hwnd)
        {
            WindowHandle = hwnd;
        }

        public HotKeyHelper()
        {
            WindowHandle = IntPtr.Zero;
        }

        private IDictionary<string, int> HotKeyId = new Dictionary<string, int>();

        private IntPtr WindowHandle;

        public bool RegisterHotKey(IntPtr hwnd, string name, 
            Key key, bool hasCtrl = false, bool hasShift = false, bool hasAlt = false)
        {
            if (!HotKeyId.ContainsKey(name))
            {
                if (HotKeysNativeMethods.GlobalFindAtom(name) != 0)
                {
                    HotKeysNativeMethods.GlobalDeleteAtom(name);
                }
                HotKeyId.Add(name, HotKeysNativeMethods.GlobalAddAtom(name));
            } else
            {
                HotKeysNativeMethods.UnregisterHotKey(hwnd, HotKeyId[name]);
            }
            return RegisterHotKey(hwnd, HotKeyId[name], key, hasCtrl, hasShift, hasAlt);
        }

        public bool RegisterHotKey(string name,
            Key key, bool hasCtrl = false, bool hasShift = false, bool hasAlt = false)
        {
            if (!HotKeyId.ContainsKey(name))
            {
                if (HotKeysNativeMethods.GlobalFindAtom(name) != 0)
                {
                    HotKeysNativeMethods.GlobalDeleteAtom(name);
                }
                HotKeyId.Add(name, HotKeysNativeMethods.GlobalAddAtom(name));
            }
            else
            {
                HotKeysNativeMethods.UnregisterHotKey(WindowHandle, HotKeyId[name]);
            }
            return RegisterHotKey(WindowHandle, HotKeyId[name], key, hasCtrl, hasShift, hasAlt);
        }


        private bool RegisterHotKey(IntPtr hwnd, int id,
            Key key, bool hasCtrl, bool hasShift, bool hasAlt)
        {

            var fsModifiers = 0;
            if (hasCtrl)
            {
                fsModifiers |= (int)Key.LeftCtrl;
            }
            if (hasShift)
            {
                fsModifiers |= (int)Key.LeftShift;
            }
            if (hasAlt)
            {
                fsModifiers |= (int)Key.LeftAlt;
            }
            return HotKeysNativeMethods.RegisterHotKey(hwnd, id, fsModifiers, (int)key);
        }


        public string ParseHook(int msg, IntPtr wideParam, IntPtr longParam)
        {
            if (msg != HotKeysNativeMethods.WM_HOTKEY)
            {
                return string.Empty;
            }
            var sid = wideParam.ToInt32();
            foreach (var item in HotKeyId)
            {
                if (sid == item.Value)
                {
                    return item.Key;
                }
            }
            return string.Empty;
        }

    }
}
