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
		class LevelEntity
		{
			Texture2D wallsTB;
			Texture2D wallsRL;
			Texture2D wallsT;
			Texture2D wallsB;
			Texture2D wallsR;
			Texture2D wallsL;
			Texture2D noWalls;
			Texture2D wallsBL;
			Texture2D wallsTL;
			Texture2D wallsBR;
			Texture2D wallsTR;
			Texture2D wallsLTB;
			Texture2D wallsRTB;
			Texture2D wallsLTRB;

			Texture2D wallsLBR;

			Texture2D wallsLTR;

			int mapWidth;

			int mapHeight;

			LevelDescription level;

			public LevelEntity (LevelDescription level, int mapWidth, int mapHeight)
			{
				this.level = level;
				this.mapWidth = mapWidth;
				this.mapHeight = mapHeight;
			}

			public void Load(ContentManager content)
			{
				wallsTB = content.Load<Texture2D> ("square-tb");
				wallsRL = content.Load<Texture2D> ("square-rl");
				wallsT = content.Load<Texture2D> ("square-t");
				wallsB = content.Load<Texture2D> ("square-b");
				wallsR = content.Load<Texture2D> ("square-r");
				wallsL = content.Load<Texture2D> ("square-l");

				noWalls = content.Load<Texture2D> ("square");

				wallsBL = content.Load<Texture2D> ("square-bl");
				wallsTL = content.Load<Texture2D> ("square-lt");
				wallsBR = content.Load<Texture2D> ("square-rb");
				wallsTR = content.Load<Texture2D> ("square-tr");

				wallsLTB = content.Load<Texture2D> ("square-ltb");
				wallsRTB = content.Load<Texture2D> ("square-rtb");
				wallsLTRB = content.Load<Texture2D> ("square-ltrb");

				wallsLBR = content.Load<Texture2D> ("square-lbr");
				wallsLTR = content.Load<Texture2D> ("square-ltr");
			}

			public void Render(LevelDescription level, SpriteBatch spriteBatch)
			{
				for (int l = 0; l < level.Height; l++) {
					for (int c = 0; c < level.Width; c++) {
						var rectangle = new Rectangle (c * TILE_WIDTH, l * TILE_HEIGHT, TILE_WIDTH, TILE_HEIGHT);
						var texture = SelectRoomTexture (l, c);
						var color = SelectRoomColor (l, c);
						if (!IsEmptyRoom (l, c)) {
							if (texture != null) {
								spriteBatch.Draw (texture, rectangle, null, color);
							}
							else {
								DumpTileInfo (l, c);
							}
						}
					}
				}
			}

			Texture2D SelectRoomTexture (int l, int c)
			{
				Texture2D texture = null;
				var targetTileType = level.Tiles [c + l * mapWidth];

				if ((targetTileType.Walls ^ (WallTypes.Top|WallTypes.Bottom)) == 0) {
					texture = wallsTB;
				}
				if ((targetTileType.Walls ^ (WallTypes.Right|WallTypes.Left)) == 0) {
					texture = wallsRL;
				}

				if ((targetTileType.Walls ^ (WallTypes.Top)) == 0) {
					texture = wallsT;
				}
				if ((targetTileType.Walls ^ (WallTypes.Bottom)) == 0) {
					texture = wallsB;
				}
				if ((targetTileType.Walls ^ (WallTypes.Right)) == 0) {
					texture = wallsR;
				}
				if ((targetTileType.Walls ^ (WallTypes.Left)) == 0) {
					texture = wallsL;
				}

				if ((targetTileType.Walls ^ (WallTypes.None)) == 0) {
					texture = noWalls;
				}

				if ((targetTileType.Walls ^ (WallTypes.Left|WallTypes.Bottom)) == 0) {
					texture = wallsBL;
				}

				if ((targetTileType.Walls ^ (WallTypes.Top|WallTypes.Left)) == 0) {
					texture = wallsTL;
				}

				if ((targetTileType.Walls ^ (WallTypes.Right|WallTypes.Bottom)) == 0) {
					texture = wallsBR;
				}

				if ((targetTileType.Walls ^ (WallTypes.Top|WallTypes.Right)) == 0) {
					texture = wallsTR;
				}

				//

				if ((targetTileType.Walls ^ (WallTypes.Left|WallTypes.Top|WallTypes.Bottom)) == 0) {
					texture = wallsLTB;
				}

				if ((targetTileType.Walls ^ (WallTypes.Right|WallTypes.Top|WallTypes.Bottom)) == 0) {
					texture = wallsRTB;
				}

				if ((targetTileType.Walls ^ (WallTypes.Left|WallTypes.Right|WallTypes.Top|WallTypes.Bottom)) == 0) {
					texture = wallsLTRB;
				}

				//

				if ((targetTileType.Walls ^ (WallTypes.Right|WallTypes.Left|WallTypes.Bottom)) == 0) {
					texture = wallsLBR;
				}

				if ((targetTileType.Walls ^ (WallTypes.Left|WallTypes.Right|WallTypes.Top|WallTypes.Left)) == 0) {
					texture = wallsLTR;
				}


				return texture;
			}

			Color SelectRoomColor (int l, int c)
			{
				var targetTileType = level.Tiles [c + l * mapWidth];

				if (!targetTileType.MainPath)
					return Color.LightYellow;
				else if (targetTileType.Start)
					return Color.LightGreen;
				else if (targetTileType.Finish)
					return Color.LightSalmon;
				else
					return Color.LightCyan;
			}

			bool IsEmptyRoom (int l, int c)
			{
				var targetTileType = level.Tiles [c + l * mapWidth];
				return targetTileType.Empty;
			}

			void DumpTileInfo (int l, int c)
			{
				var targetTileType = level.Tiles [c + l * mapWidth];
				Console.WriteLine (targetTileType.Walls);
			}
		}

		class BathysphereEntity
		{
			Texture2D bathyspehere;
			float posx = 0;
			float posy = 0;

			public void Load(ContentManager content)
			{
				bathyspehere = content.Load<Texture2D> ("bathysphere-small");
			}

			public void Render (SpriteBatch spriteBatch)
			{
				if (bathyspehere != null)
					spriteBatch.Draw( bathyspehere, new Rectangle((int)posx-5, (int)posy-5, 10, 10), null, Color.White);
			}

			public void Update (GameTime gameTime)
			{
			}

			public void SetPosition (float posx, float posy)
			{
				this.posx = posx;
				this.posy = posy;
			}

			public void ApplyForce (int xForce, int yForce)
			{
			}
		}

		#region Fields

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		LevelEntity levelEntity;
		BathysphereEntity bathysphereEntity;

		Keys[] oldKeyDowns;

		#endregion

		#region Initialization

		LevelDescription levelDescription;

		const float ROOM_SCALE = 2.0f;

		const int TILE_WIDTH = (int)(80.0f * ROOM_SCALE);
		const int TILE_HEIGHT = (int)(45.0f * ROOM_SCALE);

		const int MAP_WIDTH	= 6;
		const int MAP_HEIGHT = 6;

		const int WORLD_WIDTH = MAP_WIDTH * TILE_WIDTH;
		const int WORLD_HEIGHT = MAP_HEIGHT * TILE_HEIGHT;

		const int SCREEN_WIDTH = 6 * 160;
		const int SCREEN_HEIGHT = 6 * 90;

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

			GenerateLevel ();
		}

		void GenerateLevel ()
		{

			levelDescription = LevelGenerator.Generate(MAP_WIDTH, MAP_HEIGHT);
			levelEntity = new LevelEntity(levelDescription, MAP_WIDTH, MAP_HEIGHT);
			bathysphereEntity = new BathysphereEntity ();

			slow = false;
			SetStartXY ();
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
			game = true;
			slow = false;
		}

		void SetStartXY ()
		{
			var startIndex = levelDescription.Tiles.FindIndex (tile => tile.Start);
			var c = (startIndex % levelDescription.Width) + 0.5f;
			var l = (startIndex / levelDescription.Width) + 0.5f;

			float posx = (c * (float)TILE_WIDTH);
			float posy = (l * (float)TILE_HEIGHT);

			bathysphereEntity.SetPosition (posx, posy);
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
			levelEntity.Load(Content);
			bathysphereEntity.Load (Content);
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
			UpdateKeys (gameTime);
			UpdateSlow (gameTime);
			UpdateGame (gameTime);
			base.Update (gameTime);
		}

		void UpdateKeys (GameTime gameTime)
		{
			Keys[] newKeyDowns = Keyboard.GetState ().GetPressedKeys ();
			IEnumerable<Keys> keyStillDowns = newKeyDowns.Intersect (oldKeyDowns);
			IEnumerable<Keys> newKeyPresses = newKeyDowns.Except (keyStillDowns);
			IEnumerable<Keys> newKeyReleases = oldKeyDowns.Except (keyStillDowns);

			if (newKeyReleases.Contains(Keys.R)) {
				GenerateLevel ();
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
			}

			if (keyStillDowns.Contains (Keys.Down)) {
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

			if (game)
				RenderGame ();
			else
				RenderMap ();

			spriteBatch.End ();

			//TODO: Add your drawing code here
			base.Draw (gameTime);
		}

		void RenderGame()
		{
		}

		void RenderMap ()
		{
			levelEntity.Render (levelDescription, spriteBatch);
			bathysphereEntity.Render (spriteBatch);
		}
			
		#endregion
	}
}
