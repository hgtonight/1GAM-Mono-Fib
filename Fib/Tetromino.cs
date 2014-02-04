using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fib
{
    class Tetromino
    {
        public static Texture2D TileSheet = null;
        public static Rectangle SpriteCoords = new Rectangle(0, 0, 16, 16);
        public static int TileSize = 16;
        private List<GridPosition> BlockPositions;
        private Vector2 BoardPosition;
        private bool FastFall, Falling;

        public Tetromino(List<GridPosition> list)
        {
            BlockPositions = list;
            BoardPosition = new Vector2(4, 0);
            FastFall = false;
            Falling = true;
        }

        public void March()
        {
            if (Falling)
            {
                BoardPosition.Y++;
            }
        }

        public void Retreat()
        {
            BoardPosition.Y--;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 Offset)
        {
            for (int i = BlockPositions.Count - 1; i >= 0; i--)
            {
                spriteBatch.Draw(TileSheet, new Vector2(
                                                (BlockPositions[i].X * TileSize) + (BoardPosition.X * TileSize) + Offset.X,
                                                (BlockPositions[i].Y * TileSize) + (BoardPosition.Y * TileSize) + Offset.Y
                                                ), SpriteCoords, BlockPositions[i].Color);
            }
        }

        public void Position(Vector2 Position)
        {
            BoardPosition.X = Position.X;
            BoardPosition.Y = Position.Y;
        }

        public void RotateCW()
        {

        }

        public void RotateCCW()
        {

        }

        public void Drop()
        {
            FastFall = true;
        }

        public List<GridPosition> Blocks()
        {
            List<GridPosition> Blocks = new List<GridPosition>();
            foreach (GridPosition Block in BlockPositions) {
                GridPosition Temp = new GridPosition(Block.X + (int)BoardPosition.X, Block.Y + (int)BoardPosition.Y, Block.Color);
                Blocks.Add(Temp);
            }

            return Blocks;
        }

        public static Tetromino NextPiece(uint random = 0)
        {
            if (random == 0)
            {
                Random r = new Random();
                random = (uint)r.Next(0, 6);
            }

            random = random % 7;

            switch (random)
            {
                default:
                case 0:
                    return Tetromino.I();
                    break;
                case 1:
                    return Tetromino.O();
                    break;
                case 2:
                    return Tetromino.T();
                    break;
                case 3:
                    return Tetromino.J();
                    break;
                case 4:
                    return Tetromino.L();
                    break;
                case 5:
                    return Tetromino.S();
                    break;
                case 6:
                    return Tetromino.Z();
                    break;
            }
            
        }

        /* The standard pieces */
        public static Tetromino I()
        {
            List<GridPosition> List = new List<GridPosition>();
            Color Color = Color.Cyan;
            List.Add(new GridPosition(0, 3, Color));
            List.Add(new GridPosition(1, 3, Color));
            List.Add(new GridPosition(2, 3, Color));
            List.Add(new GridPosition(3, 3, Color));
            return new Tetromino(List);
        }

        public static Tetromino O()
        {
            List<GridPosition> List = new List<GridPosition>();
            Color Color = Color.Yellow;
            List.Add(new GridPosition(1, 2, Color));
            List.Add(new GridPosition(1, 3, Color));
            List.Add(new GridPosition(2, 2, Color));
            List.Add(new GridPosition(2, 3, Color));
            return new Tetromino(List);
        }

        public static Tetromino T()
        {
            List<GridPosition> List = new List<GridPosition>();
            Color Color = Color.DarkMagenta;
            List.Add(new GridPosition(0, 2, Color));
            List.Add(new GridPosition(1, 2, Color));
            List.Add(new GridPosition(2, 2, Color));
            List.Add(new GridPosition(1, 3, Color));
            return new Tetromino(List);
        }

        public static Tetromino J()
        {
            List<GridPosition> List = new List<GridPosition>();
            Color Color = Color.Blue;
            List.Add(new GridPosition(0, 2, Color));
            List.Add(new GridPosition(1, 2, Color));
            List.Add(new GridPosition(2, 2, Color));
            List.Add(new GridPosition(2, 3, Color));
            return new Tetromino(List);
        }

        public static Tetromino L()
        {
            List<GridPosition> List = new List<GridPosition>();
            Color Color = Color.Orange;
            List.Add(new GridPosition(0, 2, Color));
            List.Add(new GridPosition(1, 2, Color));
            List.Add(new GridPosition(2, 2, Color));
            List.Add(new GridPosition(0, 3, Color));
            return new Tetromino(List);
        }

        public static Tetromino S()
        {
            List<GridPosition> List = new List<GridPosition>();
            Color Color = Color.Lime;
            List.Add(new GridPosition(0, 3, Color));
            List.Add(new GridPosition(1, 3, Color));
            List.Add(new GridPosition(1, 2, Color));
            List.Add(new GridPosition(2, 2, Color));
            return new Tetromino(List);
        }

        public static Tetromino Z()
        {
            List<GridPosition> List = new List<GridPosition>();
            Color Color = Color.Red;
            List.Add(new GridPosition(0, 2, Color));
            List.Add(new GridPosition(1, 2, Color));
            List.Add(new GridPosition(1, 3, Color));
            List.Add(new GridPosition(2, 3, Color));
            return new Tetromino(List);
        }
    }
}
