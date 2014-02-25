﻿#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D TileSheet;
        KeyboardState PreviousKeyState;
        Board Board;
        Tetromino Piece, GhostPiece, PreviewPiece;
        int GameSpeed;
        bool GameOver;

        public Fib()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Board = new Board(new Vector2(100, 20));
            GameSpeed = 60;
            GameOver = false;
            Piece = Tetromino.NextPiece();
            GhostPiece = Piece.Tracer();
            PreviewPiece = Tetromino.NextPiece();
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
            if (GameOver)
            {
                HandleMenuInput();
            }
            else
            {
                HandleUserInput();
                UpdateGameState();
                CheckForGameEnd();
            }

            base.Update(gameTime);
        }

        private void CheckForGameEnd()
        {
            if (Board.IsFull())
            {
                GameOver = true;
            }
        }

        private void UpdateGameState()
        {
            Ticks++;
            // Gravity only happens some of the time
            if (Ticks % GameSpeed == 0 || Piece.FastFalling())
            {
                Piece.March();
                Ticks = 0;
            }

            // If piece collides with the board
            if (Board.Collides(Piece.Blocks()))
            {
                Piece.Retreat();
                Board.Consume(Piece.Blocks());
                Piece = PreviewPiece;
                PreviewPiece = Tetromino.NextPiece();
                Board.RemoveCompletedLines();
            }

            // Update the ghost piece
            GhostPiece = Piece.Tracer();
            while(!Board.Collides(GhostPiece.Blocks())) {
                GhostPiece.March();
            }
            GhostPiece.Retreat();
        }

        private void HandleMenuInput()
        {
            throw new NotImplementedException();
        }

        private void HandleUserInput()
        {
            KeyboardState CurrentKeyState = Keyboard.GetState();

            if (CurrentKeyState.IsKeyDown(Keys.Escape) && !PreviousKeyState.IsKeyDown(Keys.Escape))
            {
                // Exit in the future
            }

            if (CurrentKeyState.IsKeyDown(Keys.Left) && !PreviousKeyState.IsKeyDown(Keys.Left))
            {
                Piece.MoveLeft();
                if (Board.Collides(Piece.Blocks()))
                {
                    Piece.MoveRight();
                }
            }

            if (CurrentKeyState.IsKeyDown(Keys.Right) && !PreviousKeyState.IsKeyDown(Keys.Right))
            {
                Piece.MoveRight();
                if (Board.Collides(Piece.Blocks()))
                {
                    Piece.MoveLeft();
                }
            }

            if (CurrentKeyState.IsKeyDown(Keys.LeftControl) && !PreviousKeyState.IsKeyDown(Keys.LeftControl))
            {
                Piece.RotateCW();
                if (Board.Collides(Piece.Blocks()))
                {
                    Piece.RotateCCW();
                }
            }

            if (CurrentKeyState.IsKeyDown(Keys.LeftAlt) && !PreviousKeyState.IsKeyDown(Keys.LeftAlt))
            {
                Piece.RotateCCW();
                if (Board.Collides(Piece.Blocks()))
                {
                    Piece.RotateCW();
                }
            }

            if (CurrentKeyState.IsKeyDown(Keys.Space) && !PreviousKeyState.IsKeyDown(Keys.Space))
            {
                Piece.Drop();
            }

            if (CurrentKeyState.IsKeyDown(Keys.Down))
            {
                Piece.SoftDrop();
            }

            PreviousKeyState = CurrentKeyState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            Board.Draw(gameTime, spriteBatch);

            Piece.Draw(gameTime, spriteBatch, Board.Position());
            GhostPiece.Draw(gameTime, spriteBatch, Board.Position());
            PreviewPiece.Draw(gameTime, spriteBatch, Board.PreviewPosition());

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
