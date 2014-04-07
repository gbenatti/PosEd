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
	class Camera
	{
		public float ScaleX {
			get;
			private set;
		}

		public float ScaleY {
			get;
			private set;
		}

		int screenWidth;
		int screenHeight;

		public Camera (int screenWidth, int screenHeight, int viewWidth, int viewHeight)
		{
			this.screenHeight = screenHeight;
			this.screenWidth = screenWidth;

			CalculateScale (viewWidth, viewHeight);
		}

		void CalculateScale (int viewWidth, int viewHeight)
		{
			ScaleX = this.screenWidth / viewWidth;
			ScaleY = this.screenHeight / viewHeight;
		}

		public void SetViewPort (int newViewWidth, int newViewHeight)
		{
			CalculateScale (newViewWidth, newViewHeight);
		}
	}
	
}
