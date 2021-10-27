using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Input;

namespace ZoDream.Shared.Player
{
    public class SystemPlayer : IKeyboardPlayer, IMousePlayer, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void KeyDown(Key key)
        {
            throw new NotImplementedException();
        }

        public void KeyPress(Key key)
        {
            throw new NotImplementedException();
        }

        public void KeyPress(params Key[] keys)
        {
            throw new NotImplementedException();
        }

        public void KeyUp(Key key)
        {
            throw new NotImplementedException();
        }

        public void ModifiedKeyStroke(IEnumerable<Key> modifierKeys, IEnumerable<Key> keys)
        {
            throw new NotImplementedException();
        }

        public void ModifiedKeyStroke(IEnumerable<Key> modifierKeys, Key key)
        {
            throw new NotImplementedException();
        }

        public void ModifiedKeyStroke(Key modifierKey, IEnumerable<Key> keys)
        {
            throw new NotImplementedException();
        }

        public void ModifiedKeyStroke(Key modifierKey, Key key)
        {
            throw new NotImplementedException();
        }

        public void MouseClick()
        {
            throw new NotImplementedException();
        }

        public void MouseDoubleClick()
        {
            throw new NotImplementedException();
        }

        public void MouseDown()
        {
            throw new NotImplementedException();
        }

        public void MouseHWheel(int value)
        {
            throw new NotImplementedException();
        }

        public void MouseMoveTo(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void MouseMoveTo(Point point)
        {
            throw new NotImplementedException();
        }

        public void MouseRightClick()
        {
            throw new NotImplementedException();
        }

        public void MouseRightDoubleClick()
        {
            throw new NotImplementedException();
        }

        public void MouseRightDown()
        {
            throw new NotImplementedException();
        }

        public void MouseRightUp()
        {
            throw new NotImplementedException();
        }

        public void MouseUp()
        {
            throw new NotImplementedException();
        }

        public void MouseWheel(int value)
        {
            throw new NotImplementedException();
        }

        public void MouseXClick(int buttonId)
        {
            throw new NotImplementedException();
        }

        public void MouseXDoubleClick(int buttonId)
        {
            throw new NotImplementedException();
        }

        public void MouseXDown(int buttonId)
        {
            throw new NotImplementedException();
        }

        public void MouseXUp(int buttonId)
        {
            throw new NotImplementedException();
        }
    }
}
