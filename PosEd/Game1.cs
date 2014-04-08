#region File Description
//-----------------------------------------------------------------------------
// PosEdGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;


#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

using MapGen;

#endregion
namespace PosEd
{

	/// <summary>
	/// Default Project Template
	/// </summary>
	public class Game1 : Game
	{
		#region Fields

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		LevelDescription levelDescription;

		LevelEntity levelEntity;
		BathysphereEntity bathysphereEntity;

		Camera camera;

		Keys[] oldKeyDowns;

		#endregion

		#region Initialization

		const int MAP_WIDTH	= 6;
		const int MAP_HEIGHT = 6;

		const int SCREEN_WIDTH = MAP_WIDTH * 160;
		const int SCREEN_HEIGHT = MAP_HEIGHT * 90;

		bool slow = false;
		int slowCount = 0;
		int delta = 100;

		bool game = false;

		public Game1 ()
		{

			graphics = new GraphicsDeviceManager (this);
			
			Content.RootDirectory = "Content";

			graphics.IsFullScreen = false;
			graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
			graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
		}

		void LoadEntities ()
		{
			lock (this) {
				levelDescription = LevelGenerator.Generate(MAP_WIDTH, MAP_HEIGHT);
				levelEntity = new LevelEntity(levelDescription, MAP_WIDTH, MAP_HEIGHT);
				levelEntity.Load(Content);

				bathysphereEntity = new BathysphereEntity ();
				bathysphereEntity.Load (Content);
				SetStartXY ();

				camera = new Camera (SCREEN_WIDTH, SCREEN_HEIGHT, levelEntity.Width, levelEntity.Height);
				CameraFollowBathysphere ();
			}
		}

		void SlowMotion ()
		{
			slow = !slow;
			if (slow)
				slowCount = 0;
			else
				levelDescription = new LevelDescription(MAP_WIDTH, MAP_HEIGHT, LevelGenerator.Context.LastSnapshot);
		}

		void GameMode()
		{
			game = !game;
			slow = false;

			if (game)
				camera.SetViewPort (levelEntity.Width/MAP_WIDTH, levelEntity.Height/MAP_HEIGHT);
			else
				camera.SetViewPort (levelEntity.Width, levelEntity.Height);
		}

		void SetStartXY ()
		{
			var startIndex = levelDescription.Tiles.FindIndex (tile => tile.Start);
			var c = (startIndex % levelDescription.Width) + 0.5f;
			var l = (startIndex / levelDescription.Width) + 0.5f;

			float startX = (c * (float)LevelEntity.TILE_WIDTH);
			float startY = (l * (float)LevelEntity.TILE_HEIGHT);

			bathysphereEntity.SetPosition (startX, startY);
		}	

		void CameraFollowBathysphere ()
		{
			var viewport = camera.GetViewPort ();
			var halfViewportWidth = viewport.Item1 / 2;
			camera.TargetX = ((bathysphereEntity.Posx - halfViewportWidth) >= 0) ? ((bathysphereEntity.Posx + halfViewportWidth > levelEntity.Width) ? levelEntity.Width - halfViewportWidth : bathysphereEntity.Posx) : halfViewportWidth; 

			var halfViewportHeight = viewport.Item2 / 2;
			camera.TargetY = ((bathysphereEntity.Posy - halfViewportHeight) >= 0) ? ((bathysphereEntity.Posy + halfViewportHeight > levelEntity.Height) ? levelEntity.Height - halfViewportHeight : bathysphereEntity.Posy) : halfViewportHeight;
		}

		/// <summary>
		/// Overridden from the base Game.Initialize. Once the GraphicsDevice is setup,
		/// we'll use the viewport to initialize some values.
		/// </summary>
		protected override void Initialize ()
		{
			base.Initialize ();
			SetupInitialValues ();
		}

		void SetupInitialValues ()
		{
			oldKeyDowns = Keyboard.GetState().GetPressedKeys();
			game = false;
			slow = false;
		}

		/// <summary>
		/// Load your graphics content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be use to draw textures.
			spriteBatch = new SpriteBatch (graphics.GraphicsDevice);
			LoadEntities ();
		}
			
		#endregion

		#region Update and Draw

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			lock (this) {
				UpdateKeys (gameTime);
				UpdateSlow (gameTime);
				UpdateGame (gameTime);
				base.Update (gameTime);
			}
		}

		void UpdateKeys (GameTime gameTime)
		{
			Keys[] newKeyDowns = Keyboard.GetState ().GetPressedKeys ();
			IEnumerable<Keys> keyStillDowns = newKeyDowns.Intersect (oldKeyDowns);
			IEnumerable<Keys> newKeyPresses = newKeyDowns.Except (keyStillDowns);
			IEnumerable<Keys> newKeyReleases = oldKeyDowns.Except (keyStillDowns);

			if (newKeyReleases.Contains(Keys.R)) {
				LoadEntities ();
				SetupInitialValues ();
			}

			if (newKeyReleases.Contains (Keys.S)) {
				SlowMotion ();
			}

			if (newKeyReleases.Contains (Keys.G)) {
				GameMode ();
			}

			if (keyStillDowns.Contains (Keys.Left)) {
				bathysphereEntity.ApplyForce (-2, 0);
			}

			if (keyStillDowns.Contains (Keys.Right)) {
				bathysphereEntity.ApplyForce (2, 0);
			}

			if (keyStillDowns.Contains (Keys.Up)) {
				bathysphereEntity.ApplyForce (0, -1);
			}

			if (keyStillDowns.Contains (Keys.Down)) {
				bathysphereEntity.ApplyForce (0, 1);
			}

			oldKeyDowns = newKeyDowns;
		}

		void UpdateSlow(GameTime gameTime)
		{
			if (!slow)
				return;

			delta -= gameTime.ElapsedGameTime.Milliseconds;
			if (delta < 0) {
				delta = 500;

				slowCount += 1;
				if (slowCount >= LevelGenerator.Context.Snapshots.Count) {
					slowCount = 0;
				}
			}

			levelDescription = new LevelDescription(MAP_WIDTH, MAP_HEIGHT, LevelGenerator.Context.Snapshots[slowCount]);
		}

		void UpdateGame(GameTime gameTime)
		{
			bathysphereEntity.Update (gameTime);
			CollisionBathysphereWorld (bathysphereEntity, levelEntity);
			CameraFollowBathysphere ();
		}

		/// <summary>
		/// This is called when the game should draw itself. 
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			lock (this) {
				// Clear the backbuffer
				graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
				
				spriteBatch.Begin ();
				
				RenderMap ();
				
				spriteBatch.End ();
				
				base.Draw (gameTime);
			}
		}

		void RenderMap ()
		{
			levelEntity.Render (camera, spriteBatch, game);
			bathysphereEntity.Render (camera, spriteBatch);
		}
			
		void CollisionBathysphereWorld (BathysphereEntity bathysphereEntity, LevelEntity levelEntity)
		{
			float px = bathysphereEntity.Posx;
			float py = bathysphereEntity.Posy;

			if (levelEntity.Collision (px, py)) {
				bathysphereEntity.Rewind ();
			}
		}
		#endregion
	}
}
