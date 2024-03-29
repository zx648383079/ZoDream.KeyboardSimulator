﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ZoDream.Shared.Input;

namespace ZoDream.Shared.Player
{
    public interface IMousePlayer
    {

        public void MouseMoveTo(double x, double y);
        public void MouseMoveTo(Point point);

        public void MouseDown();
        public void MouseUp();
        public void MouseClick();
        public void MouseClick(int count);
        public void MouseDoubleClick();

        public void MouseDown(MouseButton button);
        public void MouseUp(MouseButton button);
        public void MouseClick(MouseButton button);
        public void MouseDoubleClick(MouseButton button);

        public void MouseRightDown();
        public void MouseRightUp();
        public void MouseRightClick();
        public void MouseRightDoubleClick();

        public void MouseXDown(int buttonId);
        public void MouseXUp(int buttonId);
        public void MouseXClick(int buttonId);
        public void MouseXDoubleClick(int buttonId);

        public void MouseWheel(int value);
        public void MouseHWheel(int value);
    }
}
