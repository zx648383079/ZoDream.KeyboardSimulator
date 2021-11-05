using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Input;

namespace ZoDream.Shared.Player
{
    public interface IPlayer : IKeyboardPlayer, IMousePlayer, IDisposable
    {
        public void Focus(string windowName);
        public ushort GetScanKey(Key key);
        public void LostFocus();
    }
}
