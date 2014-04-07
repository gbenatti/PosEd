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

		public Camera (int screenWidth, int screenHeight, int viewWidth, int viewHeight)
		{
			ScaleX = screenWidth / viewWidth;
			ScaleY = screenHeight / viewHeight;
		}
	}
	
}
