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
	class BathysphereEntity
	{
		Texture2D bathyspehere;
		float posx = 0;
		float posy = 0;

		public float Posx {
			get {
				return posx;
			}
		}

		public float Posy {
			get {
				return posy;
			}
		}

		public void Load(ContentManager content)
		{
			bathyspehere = content.Load<Texture2D> ("bathysphere-small");
		}

		public void Render (Camera camera, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (bathyspehere, CreateEntityRectangle (camera), null, Color.White);
		}

		Rectangle CreateEntityRectangle (Camera camera)
		{
			int width = 10;
			int height = 10;
			int halfWidth = width/2;
			int halfHeight = height/2;

			var newPos = camera.TransformPoint ((int)(posx - halfWidth), (int)(posy - halfHeight));
			return new Rectangle (newPos.Item1, newPos.Item2, (int)(width*camera.ScaleX), (int)(height*camera.ScaleY));
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
	
}
