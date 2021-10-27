using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Input;

namespace ZoDream.Shared.Player
{
    public interface IKeyboardPlayer
    {
        public void KeyDown(Key key);
        public void KeyPress(Key key);
        public void KeyPress(params Key[] keys);
        public void KeyUp(Key key);

        public void ModifiedKeyStroke(IEnumerable<Key> modifierKeys, IEnumerable<Key> keys);
        public void ModifiedKeyStroke(IEnumerable<Key> modifierKeys, Key key);
        public void ModifiedKeyStroke(Key modifierKey, IEnumerable<Key> keys);
        public void ModifiedKeyStroke(Key modifierKey, Key key);
    }
}
