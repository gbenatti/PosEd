using System;

namespace MapGen
{
	public class TileData
	{
		public WallTypes Walls {
			get;
			set;
		}

		public int PathIndex {
			get;
			set;
		}

		public bool MainPath {
			get;
			set;
		}

		public bool Empty {
			get;
			set;
		}

		public bool Start {
			get;
			set;
		}

		public bool Finish {
			get;
			set;
		}

		public byte[,] Blocks {
			get;
			set;
		}

		public TileData ()
		{
			Walls = WallTypes.None;
			PathIndex = -1;
			MainPath = false;
			Empty = true;
			Start = false;
			Finish = false;
			Blocks = new byte[16, 9];
		}

		public static TileData Create (WallTypes walls, bool mainPath, bool start = false, bool finish = false)
		{
			return new TileData {
				Walls = walls,
				MainPath = mainPath,
				Empty = false,
				Start = start,
				Finish = finish,
				Blocks = new byte[16, 9]
			};
		}
	}
}
