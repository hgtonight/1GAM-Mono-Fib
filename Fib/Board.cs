using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Fib
{
    class Board
    {
        private List<GridPosition> BlockPositions;
        private Vector3 GridSize;
        private Vector2 Offset;
        private Rectangle Block;
        private Texture2D Tile;

        public Board()
        {
            BlockPositions = new List<GridPosition>();
            GridSize.X = 10;
            GridSize.Y = 20;
            GridSize.Z = 16;
            Offset.X = 0;
            Offset.Y = 0;
        }

        public bool Update(GameTime gameTime)
        {
           // check for completed lines
            return true;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = BlockPositions.Count - 1; i >= 0; i--)
            {
                spriteBatch.Draw(Tile, new Vector2(BlockPositions[i].X * GridSize.Z, BlockPositions[i].Y * GridSize.Z), Block, BlockPositions[i].Color);
            }
        }

        public bool Collides(Vector2 Position)
        {
            if (BlockPositions.Exists(x => x.X == Position.X && x.Y == Position.Y))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Lock(Tetromino Piece)
        {

        }
    }
}
