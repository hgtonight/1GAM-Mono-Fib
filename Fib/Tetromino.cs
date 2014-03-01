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
        public static int[] Stats = {0, 0, 0, 0, 0, 0, 0};
        public static int TileSize = 16;
        private List<List<GridPosition>> BlockPositions;
        private int RotationState;
        private Vector2 BoardPosition;
        private bool HardFastFall, SoftFastFall, Falling;

        public Tetromino(List<List<GridPosition>> list)
        {
            BlockPositions = list;
            BoardPosition = new Vector2(4, -3);
            HardFastFall = false;
            SoftFastFall = false;
            Falling = true;
            RotationState = 0;
        }

        public void MoveLeft()
        {
            if (!HardFastFall)
            {
                BoardPosition.X--;
            }
        }

        public void MoveRight()
        {
            if (!HardFastFall)
            {
                BoardPosition.X++;
            }
        }

        public void March()
        {
            if (Falling)
            {
                BoardPosition.Y++;
            }
            SoftFastFall = false;
        }

        public void Retreat()
        {
            BoardPosition.Y--;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 Offset)
        {
            for (int i = BlockPositions[RotationState].Count - 1; i >= 0; i--)
            {
                spriteBatch.Draw(TileSheet, new Vector2(
                                                (this.BlockPositions[RotationState][i].X * TileSize) + (BoardPosition.X * TileSize) + Offset.X,
                                                (this.BlockPositions[RotationState][i].Y * TileSize) + (BoardPosition.Y * TileSize) + Offset.Y
                                                ), SpriteCoords, BlockPositions[RotationState][i].Color);
            }
        }

        public void ResetPiece()
        {
            HardFastFall = false;
            SoftFastFall = false;
            Falling = true;
            RotationState = 0;
            RotationState = 0;
            BoardPosition = new Vector2(4, -3);
        }

        public void Position(Vector2 Position)
        {
            BoardPosition.X = Position.X;
            BoardPosition.Y = Position.Y;
        }

        public void RotateCW()
        {
            if (!HardFastFall)
            {
                RotationState++;
                if (RotationState >= BlockPositions.Count)
                {
                    RotationState = 0;
                }
            }
        }

        public void RotateCCW()
        {
            if (!HardFastFall)
            {
                RotationState--;
                if (RotationState < 0)
                {
                    RotationState = BlockPositions.Count - 1;
                }
            }
        }

        public void Drop()
        {
            HardFastFall = true;
        }

        public void SoftDrop()
        {
            SoftFastFall = true;
        }

        public bool FastFalling()
        {
            return (HardFastFall || SoftFastFall) ? true : false;
        }

        public List<GridPosition> Blocks()
        {
            List<GridPosition> Blocks = new List<GridPosition>();
            foreach (GridPosition Block in BlockPositions[RotationState]) {
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
                random = (uint)r.Next(0, 7);
            }

            random = random % 7;

            // Keep track of next piece selection
            Stats[random]++;

            switch (random)
            {
                default:
                case 0:
                    return Tetromino.I();
                case 1:
                    return Tetromino.O();
                case 2:
                    return Tetromino.T();
                case 3:
                    return Tetromino.J();
                case 4:
                    return Tetromino.L();
                case 5:
                    return Tetromino.S();
                case 6:
                    return Tetromino.Z();
            }
            
        }

        /* Create a tracer tetromino that only has the current rotation state copied into it */
        public Tetromino Tracer()
        {
            List<List<GridPosition>> BlockList = new List<List<GridPosition>>();
            List<GridPosition> Blocks = new List<GridPosition>();

            // Clone 
            foreach(GridPosition Block in this.BlockPositions[this.RotationState]) {
                Blocks.Add(new GridPosition(Block.X, Block.Y, Color.White * 0.25f));
            }

            BlockList.Add(Blocks);

            Tetromino Ghost = new Tetromino(BlockList);

            Ghost.Position(this.BoardPosition);

            return Ghost;
        }

        /* The standard pieces */
        public static Tetromino I()
        {
            List<List<GridPosition>> List = new List<List<GridPosition>>();
            List<GridPosition> Piece = new List<GridPosition>();
            Color Color = Color.Cyan;

            Piece.Add(new GridPosition(0, 2, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(2, 2, Color));
            Piece.Add(new GridPosition(3, 2, Color));
            List.Add(Piece);

            Piece = new List<GridPosition>();
            Piece.Add(new GridPosition(1, 0, Color));
            Piece.Add(new GridPosition(1, 1, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(1, 3, Color));
            List.Add(Piece); 
            
            return new Tetromino(List);
        }

        public static Tetromino O()
        {
            List<List<GridPosition>> List = new List<List<GridPosition>>();
            List<GridPosition> Piece = new List<GridPosition>();
            Color Color = Color.Yellow;

            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(1, 3, Color));
            Piece.Add(new GridPosition(2, 2, Color));
            Piece.Add(new GridPosition(2, 3, Color));
            List.Add(Piece);

            return new Tetromino(List);
        }

        public static Tetromino T()
        {
            List<List<GridPosition>> List = new List<List<GridPosition>>();
            List<GridPosition> Piece = new List<GridPosition>();
            Color Color = Color.DarkMagenta;

            Piece.Add(new GridPosition(0, 2, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(2, 2, Color));
            Piece.Add(new GridPosition(1, 3, Color));
            List.Add(Piece);

            Piece = new List<GridPosition>();
            Piece.Add(new GridPosition(0, 2, Color));
            Piece.Add(new GridPosition(1, 1, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(1, 3, Color));
            List.Add(Piece);

            Piece = new List<GridPosition>();
            Piece.Add(new GridPosition(0, 2, Color));
            Piece.Add(new GridPosition(1, 1, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(2, 2, Color));
            List.Add(Piece);

            Piece = new List<GridPosition>();
            Piece.Add(new GridPosition(1, 1, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(1, 3, Color));
            Piece.Add(new GridPosition(2, 2, Color));
            List.Add(Piece); 
            
            return new Tetromino(List);
        }

        public static Tetromino J()
        {
            List<List<GridPosition>> List = new List<List<GridPosition>>();
            List<GridPosition> Piece = new List<GridPosition>();
            Color Color = Color.Blue;
            
            Piece.Add(new GridPosition(0, 2, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(2, 2, Color));
            Piece.Add(new GridPosition(2, 3, Color));
            List.Add(Piece);

            Piece = new List<GridPosition>(); 
            Piece.Add(new GridPosition(1, 1, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(1, 3, Color));
            Piece.Add(new GridPosition(0, 3, Color));
            List.Add(Piece);

            Piece = new List<GridPosition>();
            Piece.Add(new GridPosition(0, 1, Color));
            Piece.Add(new GridPosition(0, 2, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(2, 2, Color));
            List.Add(Piece);

            Piece = new List<GridPosition>();
            Piece.Add(new GridPosition(1, 1, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(1, 3, Color));
            Piece.Add(new GridPosition(2, 1, Color));
            List.Add(Piece);

            return new Tetromino(List);
        }

        public static Tetromino L()
        {
            List<List<GridPosition>> List = new List<List<GridPosition>>();
            List<GridPosition> Piece = new List<GridPosition>();
            Color Color = Color.Orange;
            
            Piece.Add(new GridPosition(0, 2, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(2, 2, Color));
            Piece.Add(new GridPosition(0, 3, Color));
            List.Add(Piece);

            Piece = new List<GridPosition>();
            Piece.Add(new GridPosition(0, 1, Color));
            Piece.Add(new GridPosition(1, 1, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(1, 3, Color));
            List.Add(Piece);

            Piece = new List<GridPosition>();
            Piece.Add(new GridPosition(0, 2, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(2, 2, Color));
            Piece.Add(new GridPosition(2, 1, Color));
            List.Add(Piece);

            Piece = new List<GridPosition>();
            Piece.Add(new GridPosition(1, 1, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(1, 3, Color));
            Piece.Add(new GridPosition(2, 3, Color));
            List.Add(Piece);
            
            return new Tetromino(List);
        }

        public static Tetromino S()
        {
            List<List<GridPosition>> List = new List<List<GridPosition>>();
            List<GridPosition> Piece = new List<GridPosition>();
            Color Color = Color.Lime;
            
            Piece.Add(new GridPosition(0, 3, Color));
            Piece.Add(new GridPosition(1, 3, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(2, 2, Color));
            List.Add(Piece);

            Piece = new List<GridPosition>();
            Piece.Add(new GridPosition(0, 1, Color));
            Piece.Add(new GridPosition(0, 2, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(1, 3, Color));
            List.Add(Piece);

            return new Tetromino(List);
        }

        public static Tetromino Z()
        {
            List<List<GridPosition>> List = new List<List<GridPosition>>();
            List<GridPosition> Piece = new List<GridPosition>();
            Color Color = Color.Red;

            Piece.Add(new GridPosition(0, 2, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            Piece.Add(new GridPosition(1, 3, Color));
            Piece.Add(new GridPosition(2, 3, Color));
            List.Add(Piece);

            Piece = new List<GridPosition>();
            Piece.Add(new GridPosition(0, 2, Color));
            Piece.Add(new GridPosition(0, 3, Color));
            Piece.Add(new GridPosition(1, 1, Color));
            Piece.Add(new GridPosition(1, 2, Color));
            List.Add(Piece);

            return new Tetromino(List);
        }

        public static Tetromino[] PieceList()
        {
            Tetromino[] List = { null, null, null, null, null, null, null };

            List[0] = Tetromino.I();
            List[1] = Tetromino.O();
            List[2] = Tetromino.T();
            List[3] = Tetromino.J();
            List[4] = Tetromino.L();
            List[5] = Tetromino.S();
            List[6] = Tetromino.Z();

            return List;
        }

        public static void ResetStats()
        {
            for (int i = 0; i < Stats.Count(); i++)
            {
                Stats[i] = 0;
            }
        }
    }
}
