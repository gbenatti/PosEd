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
		Texture2D leftRightRoom;
		Texture2D leftRightBottomRoom;
		Texture2D leftRightTopRoom;

		Keys[] oldKeyDowns;

		#endregion

		#region Initialization

		LevelDescription level;

		const float ROOM_SCALE = 2.0f;

		const int TILE_WIDTH = (int)(80.0f * ROOM_SCALE);
		const int TILE_HEIGHT = (int)(45.0f * ROOM_SCALE);

		const int MAP_WIDTH	= 5;
		const int MAP_HEIGHT = 5;

		const int WORLD_WIDTH = MAP_WIDTH * TILE_WIDTH;
		const int WORLD_HEIGHT = MAP_HEIGHT * TILE_HEIGHT;

		int slowCount = 0;
		bool slow = false;
		int delta = 500;

		public Game1 ()
		{

			graphics = new GraphicsDeviceManager (this);
			
			Content.RootDirectory = "Content";

			graphics.IsFullScreen = false;
			graphics.PreferredBackBufferWidth = WORLD_WIDTH;
			graphics.PreferredBackBufferHeight = WORLD_HEIGHT;

			GenerateLevel ();
		}

		void GenerateLevel ()
		{
			slow = false;
			level = LevelGenerator.Generate(MAP_WIDTH, MAP_HEIGHT);
		}

		void SlowMotion ()
		{
			slow = !slow;
			if (slow)
				slowCount = 0;
			else
				level = new LevelDescription(MAP_WIDTH, MAP_HEIGHT, LevelGenerator.Context.LastSnapshot);
		}


		/// <summary>
		/// Overridden from the base Game.Initialize. Once the GraphicsDevice is setup,
		/// we'll use the viewport to initialize some values.
		/// </summary>
		protected override void Initialize ()
		{
			base.Initialize ();
			oldKeyDowns = Keyboard.GetState().GetPressedKeys();
		}

		/// <summary>
		/// Load your graphics content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be use to draw textures.
			spriteBatch = new SpriteBatch (graphics.GraphicsDevice);

			leftRightRoom = Content.Load<Texture2D> ("square-tb");
			leftRightBottomRoom = Content.Load<Texture2D> ("square-t");
			leftRightTopRoom = Content.Load<Texture2D> ("square-b");
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
			UpdateKeys ();
			UpdateSlow (gameTime);
			base.Update (gameTime);
		}

		void UpdateKeys ()
		{
			Keys[] newKeyDowns = Keyboard.GetState ().GetPressedKeys ();
			IEnumerable<Keys> keyStillDowns = newKeyDowns.Intersect (oldKeyDowns);
			//			IEnumerable<Keys> newKeyPresses = newKeyDowns.Except (keyStillDowns);
			IEnumerable<Keys> newKeyReleases = oldKeyDowns.Except (keyStillDowns);

			if (newKeyReleases.Contains(Keys.R)) {
				GenerateLevel ();
			}

			if (newKeyReleases.Contains (Keys.S)) {
				SlowMotion ();
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

			level = new LevelDescription(MAP_WIDTH, MAP_HEIGHT, LevelGenerator.Context.Snapshots[slowCount]);
		}

		/// <summary>
		/// This is called when the game should draw itself. 
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			// Clear the backbuffer
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);

			spriteBatch.Begin ();

			for (int l = 0; l < level.Height; l++) {
				for (int c = 0; c < level.Width; c++) {
					var rectangle = new Rectangle (c * TILE_WIDTH, l * TILE_HEIGHT, TILE_WIDTH, TILE_HEIGHT);
					var texture = SelectRoomTexture (l, c);
					var color = SelectRoomColor (l, c);
					if (texture != null)
						spriteBatch.Draw (texture, rectangle, null, color);
				}
			}

			spriteBatch.End ();

			//TODO: Add your drawing code here
			base.Draw (gameTime);
		}
			
		Texture2D SelectRoomTexture (int l, int c)
		{
			Texture2D texture = null;
			var targetTileType = level.Tiles [c + l * MAP_WIDTH];

			if (targetTileType == TileTypes.LeftRight || targetTileType == TileTypes.LeftRightLost) {
				texture = leftRightRoom;
			}
			if (targetTileType == TileTypes.LeftRightBottom || targetTileType == TileTypes.LeftRightBottomLost) {
				texture = leftRightBottomRoom;
			}
			if (targetTileType == TileTypes.LeftRightTop || targetTileType == TileTypes.LeftRightTopLost) {
				texture = leftRightTopRoom;
			}
			return texture;
		}

		Color SelectRoomColor (int l, int c)
		{
			var targetTileType = level.Tiles [c + l * MAP_WIDTH];
			var index = (int)targetTileType;
			var colors = new List<Color> {Color.Black, Color.LightGreen, Color.LightGreen, Color.LightGreen, Color.Gold, Color.Gold, Color.Gold };

			return colors[index];
		}

		#endregion
	}
}
