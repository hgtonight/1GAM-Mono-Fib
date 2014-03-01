#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Fib
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Fib : Game
    {
        static int Ticks = 0;
        enum GameState { Paused, GameOver, Active };
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D TileSheet;
        SpriteFont Font;
        SoundEffect MoveLR, March, RotateLR, MoveDenied, RotateDenied, BlockLock, LineRemoval, Drop, Hold;
        KeyboardState CurrentKeyState, PreviousKeyState;
        Board Board;
        Tetromino Piece, GhostPiece, PreviewPiece, HeldPiece, TempPiece;
        Tetromino[] StatPieces;
        int GameSpeed, Score, Level, LinesCleared;
        bool GameOver, CanSwapHeldPiece;
        GameState State;

        public Fib()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Board = new Board(new Vector2(180, 32));
            Score = 0;
            LinesCleared = 0;
            Level = 0;
            GameSpeed = CalculateGameSpeed(Level);
            GameOver = false;
            StatPieces = Tetromino.PieceList();
            Piece = Tetromino.NextPiece();
            GhostPiece = Piece.Tracer();
            PreviewPiece = Tetromino.NextPiece();
            HeldPiece = null;
            TempPiece = null;
            CanSwapHeldPiece = true;
            State = GameState.Paused;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TileSheet = Content.Load<Texture2D>("tiles");
            Font = Content.Load<SpriteFont>("PressStart2P");

            // Load up sound effects
            MoveLR = Content.Load<SoundEffect>("move");
            March = Content.Load<SoundEffect>("march");
            RotateLR = Content.Load<SoundEffect>("rotate");
            MoveDenied = Content.Load<SoundEffect>("cantmove");
            RotateDenied = Content.Load<SoundEffect>("cantrotate");
            BlockLock = Content.Load<SoundEffect>("lockdatblock");
            LineRemoval = Content.Load<SoundEffect>("removeline");
            Drop = Content.Load<SoundEffect>("dropdatblock");
            Hold = Content.Load<SoundEffect>("hold");

            // Set up static members of tetrominos
            Tetromino.TileSheet = TileSheet;
            Tetromino.TileSize = 16;
            Tetromino.SpriteCoords = new Rectangle(16, 16, 16, 16);

            // Set up static members of the board
            Board.TileSheet = TileSheet;
            Board.WallSprite = new Rectangle(0, 0, 16, 16);
            Board.BlockSprite = new Rectangle(16, 0, 16, 16);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            CurrentKeyState = Keyboard.GetState();
            switch(State) {
                case GameState.Active:
                    HandleGameInput();
                    StepGameLogic();
                    CheckForGameEnd();
                    break;
                default:
                case GameState.GameOver:
                case GameState.Paused:
                    HandlePauseInput();
                    break;
            }
            PreviousKeyState = CurrentKeyState;
            base.Update(gameTime);
        }

        private void CheckForGameEnd()
        {
            if (Board.IsFull())
            {
                State = GameState.GameOver;
            }
        }

        private void StepGameLogic()
        {
            bool FastFall = Piece.FastFalling();
            Ticks++;

            // Gravity only happens some of the time
            if (Ticks % GameSpeed == 0 || FastFall)
            {
                Piece.March();
                Ticks = 0;

                // Reward dropping based on position
                if (FastFall)
                {
                    Score++;
                }
            }

            // If piece collides with the board
            if (Board.Collides(Piece.Blocks()))
            {
                Piece.Retreat();
                Board.Consume(Piece.Blocks());
                BlockLock.Play();
                Piece = PreviewPiece;
                PreviewPiece = Tetromino.NextPiece();
                CanSwapHeldPiece = true;
                switch (Board.RemoveCompletedLines())
                {
                    case 4:
                        Score += 1200 * (Level + 1);
                        LinesCleared += 4;
                        LineRemoval.Play();
                        break;
                    case 3:
                        Score += 300 * (Level + 1);
                        LinesCleared += 3;
                        LineRemoval.Play();
                        break;
                    case 2:
                        Score += 100 * (Level + 1);
                        LinesCleared += 2;
                        LineRemoval.Play();
                        break;
                    case 1:
                        Score += 40 * (Level + 1);
                        LinesCleared += 1;
                        LineRemoval.Play();
                        break;
                    default:
                    case 0:
                        break;
                }
            }

            // Look for change in level
            if (Math.Floor((decimal)(LinesCleared / 10)) != Level)
            {
                Level++;
                GameSpeed = CalculateGameSpeed(Level);
            }

            // Update the ghost piece
            GhostPiece = Piece.Tracer();
            while(!Board.Collides(GhostPiece.Blocks())) {
                GhostPiece.March();
            }
            GhostPiece.Retreat();
        }

        /**
         * Returns a gamespeed based on level that matches NES Tetris's apparent implementation
         */
        private int CalculateGameSpeed(int lvl)
        {
            if (lvl >= 0 && lvl <= 8)
            {
                return (48 - (lvl * 5));
            }
            else if (lvl >= 0 && lvl <= 9)
            {
                return 6;
            }
            else if (lvl >=0 && lvl <= 28)
            {
                return 5 - (int)(Math.Floor((decimal)(lvl - 10) / 3));
            }
            else
            {
                return 1;
            }
        }

        private void HandleGameInput()
        {
            if (KeyPressed(Keys.Escape) || KeyPressed(Keys.P) || KeyPressed(Keys.F1))
            {
                State = GameState.Paused;
            }

            if (KeyPressed(Keys.Left) || KeyPressed(Keys.NumPad4))
            {
                Piece.MoveLeft();
                if (Board.Collides(Piece.Blocks()))
                {
                    Piece.MoveRight();
                    MoveDenied.Play();
                }
                else
                {
                    MoveLR.Play();
                }
            }

            if (KeyPressed(Keys.Right) || KeyPressed(Keys.NumPad6))
            {
                Piece.MoveRight();
                if (Board.Collides(Piece.Blocks()))
                {
                    Piece.MoveLeft();
                    MoveDenied.Play();
                }
                else
                {
                    MoveLR.Play();
                }
            }

            if (KeyPressed(Keys.Up) || KeyPressed(Keys.X) || KeyPressed(Keys.NumPad1) || KeyPressed(Keys.NumPad5) || KeyPressed(Keys.NumPad9))
            {
                Piece.RotateCW();
                if (Board.Collides(Piece.Blocks()))
                {
                    Piece.RotateCCW();
                    RotateDenied.Play();
                }
                else
                {
                    RotateLR.Play();
                }
            }

            if (KeyPressed(Keys.LeftControl) || KeyPressed(Keys.Z) || KeyPressed(Keys.NumPad3) || KeyPressed(Keys.NumPad7))
            {
                Piece.RotateCCW();
                if (Board.Collides(Piece.Blocks()))
                {
                    Piece.RotateCW();
                    RotateDenied.Play();
                }
                else
                {
                    RotateLR.Play();
                }
            }

            if (KeyPressed(Keys.Space) || KeyPressed(Keys.NumPad8))
            {
                Piece.Drop();
                Drop.Play();
            }

            if (CurrentKeyState.IsKeyDown(Keys.Down) || CurrentKeyState.IsKeyDown(Keys.NumPad2))
            {
                Piece.SoftDrop();
            }

            if(KeyPressed(Keys.LeftShift) || KeyPressed(Keys.C) || KeyPressed(Keys.NumPad0)) {
                if(CanSwapHeldPiece) {
                    CanSwapHeldPiece = false;
                    SwapHeldPiece();
                    Hold.Play();
                }
                else {
                    MoveDenied.Play();
                }
            }
        }

        private void HandlePauseInput()
        {
            if (KeyPressed(Keys.Space))
            {
                State = GameState.Active;
            }

            if (KeyPressed(Keys.Escape))
            {
                Exit();
            }
        }

        private void SwapHeldPiece()
        {
            TempPiece = Piece;

            if (HeldPiece != null)
            {
                Piece = HeldPiece;
            }
            else
            {
                Piece = PreviewPiece;
                PreviewPiece = Tetromino.NextPiece();
            }

            HeldPiece = TempPiece;
            HeldPiece.ResetPiece();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            RenderGameState(gameTime);
            switch (State)
            {
                case GameState.Paused:
                    // overwrite board with pause message
                    break;
                case GameState.GameOver:
                    // Show game over screen
                    break;
                default:
                case GameState.Active:
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void RenderGameState(GameTime gameTime)
        {
            Board.Draw(gameTime, spriteBatch);

            GhostPiece.Draw(gameTime, spriteBatch, Board.Position());
            Piece.Draw(gameTime, spriteBatch, Board.Position());
            PreviewPiece.Draw(gameTime, spriteBatch, Board.PreviewPosition());

            if (HeldPiece != null)
            {
                HeldPiece.Draw(gameTime, spriteBatch, Board.HoldPosition());
            }

            DrawHud();
            DrawTetrominoStats(gameTime);
        }

        private void DrawHud()
        {
            spriteBatch.DrawString(Font, "Score", new Vector2(380, 32), Color.White);
            spriteBatch.DrawString(Font, Score.ToString(), new Vector2(512 - Font.MeasureString(Score.ToString()).X, 52), Color.White);

            spriteBatch.DrawString(Font, "Lines", new Vector2(380, 82), Color.White);
            spriteBatch.DrawString(Font, LinesCleared.ToString(), new Vector2(512 - Font.MeasureString(LinesCleared.ToString()).X, 102), Color.White);

            spriteBatch.DrawString(Font, "Level", new Vector2(380, 132), Color.White);
            spriteBatch.DrawString(Font, Level.ToString(), new Vector2(512 - Font.MeasureString(Level.ToString()).X, 152), Color.White);

            spriteBatch.DrawString(Font, "Next", new Vector2(380, Board.PreviewPosition().Y - 48), Color.White);
            spriteBatch.DrawString(Font, "Hold", new Vector2(380, Board.HoldPosition().Y - 48), Color.White);
        }

        private void DrawTetrominoStats(GameTime gameTime)
        {
            for(int i = 0; i < Tetromino.Stats.Length; i++) {
                spriteBatch.DrawString(Font, Tetromino.Stats[i].ToString(), new Vector2(172 - Font.MeasureString(Tetromino.Stats[i].ToString()).X, i * 50 + 32), Color.White);
                StatPieces[i].Draw(gameTime, spriteBatch, new Vector2(-32, i * 50 + 48));
            }
        }

        public bool KeyPressed(Keys Key)
        {
            if (CurrentKeyState.IsKeyDown(Key) && !PreviousKeyState.IsKeyDown(Key))
            {
                return true;
            }
            return false;
        }
    }
}
