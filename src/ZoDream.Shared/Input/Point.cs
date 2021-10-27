using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Input
{
    public class Point
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Point()
        {

        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
