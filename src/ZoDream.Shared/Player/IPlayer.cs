using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Player
{
    public interface IPlayer: IKeyboardPlayer, IMousePlayer, IDisposable
    {
    }
}
