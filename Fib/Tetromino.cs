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
        private List<GridPosition> BlockPositions;
        private int TileSize;
        private Vector2 Position;
        private bool FastFall;
        private Texture2D TileSheet;
        private Rectangle SpriteCoords;

        public Tetromino()
        {
            BlockPositions = new List<GridPosition>();
            Position = new Vector2(4,0);
            FastFall = false;
            TileSize = 16;
        }

        public Tetromino(List<GridPosition> list)
        {
            BlockPositions = list;
            Position = new Vector2(4,0);
            FastFall = false;
            TileSize = 16;
        }

        public void LoadContent(Texture2D tileSheet)
        {
            TileSheet = tileSheet;
            SpriteCoords = new Rectangle(0, 16, 16, 16);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = BlockPositions.Count - 1; i >= 0; i--)
            {
                spriteBatch.Draw(TileSheet, new Vector2(BlockPositions[i].X * TileSize, BlockPositions[i].Y * TileSize), SpriteCoords, BlockPositions[i].Color);
            }
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
