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

		public bool LR {
			get {
				return Walls == (WallTypes.Left | WallTypes.Right);
			}
		}

		public bool Start {
			get;
			set;
		}

		public bool Finish {
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
		}

		public static TileData Create (WallTypes walls, bool mainPath, bool start = false, bool finish = false)
		{
			return new TileData {
				Walls = walls,
				MainPath = mainPath,
				Empty = false,
				Start = start,
				Finish = finish
			};
		}
	}
}
