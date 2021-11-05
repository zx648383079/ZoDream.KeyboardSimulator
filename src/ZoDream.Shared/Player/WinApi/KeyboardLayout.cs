using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Player.WinApi
{
    
    public class KeyboardLayout: IDisposable
    {
        internal const uint KLF_NOTELLSHELL = 0x00000080;

        public readonly IntPtr Handle;

        public KeyboardLayout(IntPtr handle)
        {
            Handle = handle;
        }

        public KeyboardLayout(string keyboardLayoutID)
            : this(InputNativeMethods.LoadKeyboardLayout(keyboardLayoutID, KLF_NOTELLSHELL))
        {
        }

        public bool IsDisposed
        {
            get;
            private set;
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            InputNativeMethods.UnloadKeyboardLayout(Handle);
            IsDisposed = true;
        }

        public static KeyboardLayout US_English = new KeyboardLayout("00000409");

        public static KeyboardLayout Active
        {
            get
            {
                return new KeyboardLayout(InputNativeMethods.GetKeyboardLayout(IntPtr.Zero));
            }
        }
    }
}
