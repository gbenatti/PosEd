using System;
using System.Collections.Generic;
using System.Linq;

namespace MapGen
{
	public class MapGenerationContext
	{
		public int Width {
			get;
			set;
		}

		public int Height {
			get;
			set;
		}

		public List<List<TileTypes>> Snapshots {
			get;
			set;
		}

		public int LastUpdatedTileIndex {
			get;
			set;
		}

		public List<TileTypes> LastSnapshot {
			get {
				return Snapshots.LastOrDefault ();
			}
		}

		public Randomizer Randomizer {
			get;
			set;
		}

		public MapGenerationContext ()
		{
			this.Snapshots = new List<List<TileTypes>>();
			this.Randomizer = new Randomizer();
		}
	}
}

