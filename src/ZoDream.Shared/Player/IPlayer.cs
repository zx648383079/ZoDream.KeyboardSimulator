﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ZoDream.Language.Loggers;
using ZoDream.Shared.Input;

namespace ZoDream.Shared.Player
{
    public interface IPlayer : IKeyboardPlayer, IMousePlayer, IDisposable
    {
        public ILogger? Logger { get; set; }

        public IntPtr FindWindow(string? clsName, string windowName);
        public void FocusWindow(IntPtr hwnd);
        public int[] GetWindowRect(IntPtr hwnd);
        public int[] GetClientRect(IntPtr hwnd);
        public ushort GetScanKey(Key key);
        public void LostFocus();

        public Color GetPixelColor(int x, int y);
    }
}
