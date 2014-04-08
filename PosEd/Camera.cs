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

		public float TargetX {
			get;
			set;
		}

		public float TargetY {
			get;
			set;
		}

		int screenWidth;
		int screenHeight;
		int viewWidth;
		int viewHeight;

		public Camera (int screenWidth, int screenHeight, int viewWidth, int viewHeight)
		{
			this.viewHeight = viewHeight;
			this.viewWidth = viewWidth;
			this.screenHeight = screenHeight;
			this.screenWidth = screenWidth;

			CalculateScale ();
		}
			
		public void SetViewPort (int newViewWidth, int newViewHeight)
		{
			viewWidth = newViewWidth;
			viewHeight = newViewHeight;

			CalculateScale ();
		}

		public Tuple<int, int> GetViewPort ()
		{
			return Tuple.Create (viewWidth, viewHeight);
		}

		public void SetTarget(int targetX, int targetY)
		{
			TargetX = targetX;
			TargetY = targetY;
		}

		public Tuple<int, int> TransformPoint(int x, int y)
		{
			float transX = (x - TargetX) + viewWidth/2;
			float transY = (y - TargetY) + viewHeight/2;

			return new Tuple<int, int> ((int)(transX * ScaleX), (int)(transY * ScaleY));
		}

		void CalculateScale ()
		{
			ScaleX = this.screenWidth / this.viewWidth;
			ScaleY = this.screenHeight / this.viewHeight;
		}

	}
	
}
