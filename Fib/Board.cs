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
        private List<GridPosition> BorderPositions, BlockPositions;
        private Vector3 GridSize;
        private Vector2 Offset;
        public static Rectangle WallSprite, BlockSprite;
        public static Texture2D TileSheet;

        public Board(Vector2 Position)
        {
            BorderPositions = new List<GridPosition>();
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
                        BorderPositions.Add(new GridPosition(x, y));
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = BorderPositions.Count - 1; i >= 0; i--)
            {
                spriteBatch.Draw(TileSheet, new Vector2((BorderPositions[i].X * GridSize.Z) + Offset.X, (BorderPositions[i].Y * GridSize.Z) + Offset.Y), WallSprite, BorderPositions[i].Color);
            }

            for (int i = BlockPositions.Count - 1; i >= 0; i--)
            {
                spriteBatch.Draw(TileSheet, new Vector2((BlockPositions[i].X * GridSize.Z) + Offset.X, (BlockPositions[i].Y * GridSize.Z) + Offset.Y), BlockSprite, BlockPositions[i].Color);
            }
        }

        // returns true if there is any overlap
        public bool Collides(List<GridPosition> Positions)
        {
            foreach (GridPosition Position in Positions)
            {
                if (Position.X <= 0 || Position.X > GridSize.X)
                {
                    return true;
                }

                if (BlockPositions.Exists(x => x.X == Position.X && x.Y == Position.Y))
                {
                    return true;
                }
                
                if (BorderPositions.Exists(x => x.X == Position.X && x.Y == Position.Y))
                {
                    return true;
                }
            }

            return false;
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

        public int RemoveCompletedLines()
        {
            List<int> LinesToRemove = new List<int>();

            // Search the entire board looking for full lines
            for (int y = 1; y < GridSize.Y; y++)
            {
                for (int x = 1; x <= GridSize.X; x++)
                {
                    if (!BlockPositions.Exists(i => i.X == x && i.Y == y))
                    {
                        break;
                    }

                    if (x == GridSize.X)
                    {
                        LinesToRemove.Add(y);
                    }
                }
            }

            if (LinesToRemove.Count > 0)
            {
                // Do Stuff
                foreach (int Y in LinesToRemove)
                {
                    // Remove all blocks with that Y
                    BlockPositions.RemoveAll(i => i.Y == Y);

                    // Increase the Y position of all blocks < that Y
                    BlockPositions.ForEach(delegate(GridPosition Position)
                    {
                        if (Position.Y < Y)
                        {
                            Position.Y++;
                        }
                    });
                }
            }

            return LinesToRemove.Count;
        }

        public bool IsFull()
        {
            if (BlockPositions.Exists(i => i.Y == 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
