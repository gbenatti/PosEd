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
	class LevelEntity
	{
		static public int TILE_WIDTH = 80;
		static public int TILE_HEIGHT = 45;

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

		public int Width {
			get;
			private set;
		}

		public int Height {
			get;
			private set;
		}

		LevelDescription level;

		public LevelEntity (LevelDescription level, int mapWidth, int mapHeight)
		{
			this.level = level;
			this.mapWidth = mapWidth;
			this.mapHeight = mapHeight;

			this.Width = mapWidth * TILE_WIDTH;
			this.Height = mapHeight * TILE_HEIGHT;
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

		public void Render(Camera camera, SpriteBatch spriteBatch)
		{
			for (int l = 0; l < level.Height; l++) {
				for (int c = 0; c < level.Width; c++) {
					var rectangle = CreateTileRecatangle (l, c, camera);
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

		static Rectangle CreateTileRecatangle (int l, int c, Camera camera)
		{
			return new Rectangle ((int)(c * TILE_WIDTH* camera.ScaleX), (int)(l * TILE_HEIGHT* camera.ScaleY), (int)(TILE_WIDTH*camera.ScaleX), (int)(TILE_HEIGHT*camera.ScaleY));
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
	
}
