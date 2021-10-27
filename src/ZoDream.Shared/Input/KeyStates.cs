using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Input
{
    public enum KeyStates : byte
    {
        //
        // 摘要:
        //     The key is not pressed.
        None = 0x0,
        //
        // 摘要:
        //     The key is pressed.
        Down = 0x1,
        //
        // 摘要:
        //     The key is toggled.
        Toggled = 0x2
    }
}
