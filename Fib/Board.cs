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
        public static Rectangle SpriteCoords;
        public static Texture2D TileSheet;

        public Board(Vector2 Position)
        {
            BlockPositions = new List<GridPosition>();
            GridSize.X = 10;
            GridSize.Y = 20;
            GridSize.Z = 16;
            Offset.X = Position.X;
            Offset.Y = Position.Y;
            GenerateWell();
        }

        public void GenerateWell()
        {
            for (int x = 0; x <= GridSize.X + 1; x++)
            {
                for (int y = 0; y <= GridSize.Y; y++)
                {
                    if (x == 0
                        || x > GridSize.X
                        || y >= GridSize.Y)
                    {
                        BlockPositions.Add(new GridPosition(x, y));
                    }
                }
            }
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
                spriteBatch.Draw(TileSheet, new Vector2((BlockPositions[i].X * GridSize.Z) + Offset.X, (BlockPositions[i].Y * GridSize.Z) + Offset.Y), SpriteCoords, BlockPositions[i].Color);
            }
        }

        // returns true if there is any overlap
        public bool Collides(List<GridPosition> Positions)
        {
            int MaxY = 0;
            foreach (GridPosition Position in Positions)
            {
                MaxY = (Position.Y > MaxY) ? Position.Y : MaxY;

                if (BlockPositions.Exists(x => x.X == Position.X && x.Y == Position.Y))
                {
                    return true;
                }
            }

            // check for piece moving beyond bounds
            if (MaxY >= GridSize.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Consume(List<GridPosition> Positions)
        {
            BlockPositions = BlockPositions.Concat(Positions).ToList();
        }

        public Vector2 Position()
        {
            return Offset;
        }

        public Vector2 PreviewPosition()
        {
            return new Vector2(Offset.X + (GridSize.X * GridSize.Z), Offset.Y + (GridSize.Y * GridSize.Z));
        }

        public void RemoveCompletedLines()
        {
            // Search the entire board looking for full lines
        }
    }
}
