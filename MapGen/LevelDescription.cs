using System;
using System.Collections.Generic;

namespace MapGen
{
	public class LevelDescription
	{
		public int Width {
			get;
			private set;
		}

		public int Height {
			get;
			private set;
		}

		public List<TileData> Tiles {
			get;
			private set;
		}

		public LevelDescription (int width, int height, List<TileData> tiles)
		{
			this.Width = width;
			this.Height = height;
			this.Tiles = tiles;
		}
	}
}

