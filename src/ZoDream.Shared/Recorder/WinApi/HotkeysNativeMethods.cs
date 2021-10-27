﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ZoDream.Shared.Recorder.WinApi
{
    internal static class HotkeysNativeMethods
    {
        /// <summary>
        ///     Defines a system-wide hot key.
        /// </summary>
        /// <param name="hwnd">
        ///     A handle to the window that will receive WM_HOTKEY messages generated by the hot key. If this parameter is NULL,
        ///     WM_HOTKEY messages are posted to the message queue of the calling thread and must be processed in the message loop.
        /// </param>
        /// <param name="id">
        ///     The identifier of the hot key. If the hWnd parameter is NULL, then the hot key is associated with the current
        ///     thread rather than with a particular window. If a hot key already exists with the same hWnd and id parameters, see
        ///     Remarks for the action taken.
        /// </param>
        /// <param name="fsModifiers">
        ///     The keys that must be pressed in combination with the key specified by the uVirtKey parameter in order to generate
        ///     the WM_HOTKEY message. The fsModifiers parameter can be a combination of the following values.
        /// </param>
        /// <param name="vk">
        ///     The virtual-key code of the hot key. See Virtual Key Codes.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern int RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int vk);

        /// <summary>
        ///     Frees a hot key previously registered by the calling thread.
        /// </summary>
        /// <param name="hwnd">
        ///     A handle to the window associated with the hot key to be freed. This parameter should be NULL if the hot key is not
        ///     associated with a window.
        /// </param>
        /// <param name="id">
        ///     The identifier of the hot key to be freed.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hwnd, int id);
    }
}
