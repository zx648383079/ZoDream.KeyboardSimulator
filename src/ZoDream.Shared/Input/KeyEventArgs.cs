using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Input
{
    public class KeyEventArgs
    {
        public Key Key { get; private set; }

        public KeyStates KeyStates { get; private set; }

    }
}
