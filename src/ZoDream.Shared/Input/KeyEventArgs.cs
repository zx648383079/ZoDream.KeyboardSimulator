using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Input
{
    public class KeyEventArgs
    {
        public Key Key { get; private set; }

        public ButtonState KeyStates { get; private set; }

        public int Timestamp { get; private set; }

        public bool IsExtendedKey { get; private set; }

        public DateTime? HappenTime { get; private set; } = DateTime.Now;

        public KeyEventArgs(Key key, ButtonState keyStates)
        {
            Key = key;
            KeyStates = keyStates;
        }

        public KeyEventArgs(Key key, ButtonState keyStates, int time, bool isExtendedKey) 
            : this(key, keyStates)
        {
            Timestamp = time;
            IsExtendedKey = isExtendedKey;

        }
    }
}
