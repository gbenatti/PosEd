using System;
using System.Collections.Generic;
using System.Linq;

namespace MapGen
{
	public class LevelGenerator
	{						
		private LevelGenerator () { }

		static public MapGenerationContext Context {
			get;
			private set;
		}

		static public LevelDescription Generate(int width, int height)
		{
			var steps = new List<Action<MapGenerationContext>> {
				CreateEmptyLevel,
				AddStartRoom,
				CreatePath,
				FillEmpySpaces
			};

			Context = new MapGenerationContext {Width = width, Height = height};

			steps.ForEach ((step) => step(Context));

			return new LevelDescription (Context.Width, Context.Height, Context.LastSnapshot);
		}

		static private void CreateEmptyLevel(MapGenerationContext context)
		{
			var empty = new List<TileTypes> ( from _ in Enumerable.Range(0, context.Width*context.Height) select TileTypes.Empty );
			context.Snapshots.Add (empty);
		}

		static private void AddStartRoom(MapGenerationContext context)
		{
			var index = context.Randomizer.RandomIndex (0, context.Width);
			var newLevel = CloneUpdatingTileAtIndex (index, TileTypes.LeftRight, context.LastSnapshot);

			context.Snapshots.Add (newLevel);
			context.LastUpdatedTileIndex = index;
		}

		static public List<TileTypes> CloneUpdatingTileAtIndex (int index, TileTypes tile, List<TileTypes> lastSnapshot)
		{
			var clone = new List<TileTypes> (lastSnapshot);
			clone [index] = tile;
			return clone;
		}

		static private void CreatePath(MapGenerationContext context)
		{
			MainPathGenerator.Generate (context);
		}
			
		static private void FillEmpySpaces (MapGenerationContext context)
		{
			for (int i = 0; i < context.Width*context.Height; i++) {
				if (context.LastSnapshot [i] != TileTypes.Empty)
					continue;

				var tile = SelectRandomTileTypeLost ();
				var newSnapshot = CloneUpdatingTileAtIndex (i, tile, context.LastSnapshot);
				context.Snapshots.Add (newSnapshot);
			}
		}

		static TileTypes SelectRandomTileTypeLost ()
		{
			var lost = new List<TileTypes> {TileTypes.LeftRightLost, TileTypes.LeftRightBottomLost, TileTypes.LeftRightTopLost};
			var index = LevelGenerator.Context.Randomizer.RandomIndex (0, 3);
			return lost [index];
		}

	}
}

