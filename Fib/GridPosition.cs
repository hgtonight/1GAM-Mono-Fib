using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Fib
{
    class GridPosition
    {
        public int X, Y;
        public Color Color;

        public GridPosition()
        {
            Init(-1, -1, Color.White);
        }
        public GridPosition(int x, int y)
        {
            Init(x, y, Color.White);
        }
        public GridPosition(int x, int y, Color color)
        {
            Init(x, y, color);
        }

        private void Init(int x, int y, Color color)
        {
            X = x;
            Y = y;
            Color = color;
        }
    }
}
