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
		Texture2D submarine;

		float posx = 0;
		float posy = 0;

		float oldPosx = 0;
		float oldPosy = 0;

		float velx = 0;
		float vely = 0;

		float direction = 0.0f;

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
			submarine = content.Load<Texture2D> ("player");
		}

		public void Render (Camera camera, SpriteBatch spriteBatch, PoseidonGame.RenderMode renderMode)
		{
			spriteBatch.Draw (renderMode.ShowBlocks ? submarine : bathyspehere, CreateEntityRectangle (camera), null, Color.White);
		}

		Rectangle CreateEntityRectangle (Camera camera)
		{
			float width = 5;
			float height = 5;
			float halfWidth = width/2;
			float halfHeight = height/2;

			var newPos = camera.TransformPoint ((int)(posx - halfWidth), (int)(posy - halfHeight));
			return new Rectangle (newPos.Item1, newPos.Item2, (int)(width*camera.ScaleX), (int)(height*camera.ScaleY));
		}

		public void Update (GameTime gameTime)
		{
			oldPosx = posx;
			oldPosy = posy;

			float fractionOfSeconds = gameTime.ElapsedGameTime.Milliseconds/1000.0f;

			if (velx != 0.0f) {
				posx += velx * fractionOfSeconds;

				velx = velx > 0.0f ? velx - 4.0f * fractionOfSeconds : velx + 4.0f * fractionOfSeconds;

				if (velx > -0.01f && velx < 0.01f)
					velx = 0.0f;

				if (velx > 16.0f)
					velx = 16.0f;

				if (velx < -16.0f)
					velx = -16.0f;
			}
				
			if (vely != 0.0f) {
				posy += vely * fractionOfSeconds;

				vely = vely > 0.0f ? vely - 8.0f * fractionOfSeconds : vely + 8.0f * fractionOfSeconds;

				if (vely > -0.01f && vely < 0.01f)
					vely = 0.0f;

				if (vely > 8.0f)
					vely = 8.0f;

				if (vely < -8.0f)
					vely = -8.0f;
			}

			Console.WriteLine (velx);
		}

		public void SetPosition (float posx, float posy)
		{
			this.posx = posx;
			this.posy = posy;
		}

		public void ApplyForce (int xForce, int yForce)
		{
			velx += xForce;
			vely += yForce;
		}

		public void Rewind ()
		{
			posx = oldPosx;
			posy = oldPosy;
			velx = 0.0f;
			vely = 0.0f;
		}
	}
	
}
