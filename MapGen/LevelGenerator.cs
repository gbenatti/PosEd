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
				FillEmpySpaces,
				FixEdges
			};

			Context = new MapGenerationContext {Width = width, Height = height};

			steps.ForEach ((step) => step(Context));

			return new LevelDescription (Context.Width, Context.Height, Context.LastSnapshot);
		}

		static private void CreateEmptyLevel(MapGenerationContext context)
		{
			var empty = new List<TileData> ( from _ in Enumerable.Range(0, context.Width*context.Height) select new TileData() );
			context.Snapshots.Add (empty);
		}

		static private void AddStartRoom(MapGenerationContext context)
		{
			var index = context.Randomizer.RandomIndex (0, context.Width);
			var newLevel = CloneUpdatingTileAtIndex (index, TileData.Create(WallTypes.Top|WallTypes.Bottom, true, true), context.LastSnapshot);

			context.Snapshots.Add (newLevel);
			context.LastUpdatedTileIndex = index;
		}

		static public List<TileData> CloneUpdatingTileAtIndex (int index, TileData tile, List<TileData> lastSnapshot)
		{
			var clone = new List<TileData> (lastSnapshot);
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
				if (!context.LastSnapshot [i].Empty)
					continue;

				var tile = CreateTileWithRandomWalls ();
				var newSnapshot = CloneUpdatingTileAtIndex (i, tile, context.LastSnapshot);
				context.Snapshots.Add (newSnapshot);
			}
		}

		static TileData CreateTileWithRandomWalls ()
		{
			WallTypes walls = WallTypes.None;

			var validTypes = new List<WallTypes> {WallTypes.Left, WallTypes.Right, WallTypes.Bottom, WallTypes.Top};

			for (int i = 0; i < 4; i++) {
				int chance = LevelGenerator.Context.Randomizer.RandomIndex (0, 4);
				if (chance == 0)
					walls |= validTypes[i];
			}

			return TileData.Create (walls, false);
		}

		static void FixEdges (MapGenerationContext context)
		{
			for (int i = 0; i < context.Width*context.Height; i++) {
				if (IsTileInFirstRow (context, i)) {
					var targetTile = context.LastSnapshot [i];
					var snapshot = CloneUpdatingTileAtIndex(i, TileData.Create(targetTile.Walls | WallTypes.Top, targetTile.MainPath, targetTile.Start, targetTile.Finish), context.LastSnapshot);
					context.Snapshots.Add (snapshot);
				}
				if (IsTileInLastRow (context, i)) {
					var targetTile = context.LastSnapshot [i];
					var snapshot = CloneUpdatingTileAtIndex(i, TileData.Create(targetTile.Walls | WallTypes.Bottom, targetTile.MainPath, targetTile.Start, targetTile.Finish), context.LastSnapshot);
					context.Snapshots.Add (snapshot);
				}
				if (IsTileRightMost (context, i)) {
					var targetTile = context.LastSnapshot [i];
					var snapshot = CloneUpdatingTileAtIndex(i, TileData.Create(targetTile.Walls | WallTypes.Right, targetTile.MainPath, targetTile.Start, targetTile.Finish), context.LastSnapshot);
					context.Snapshots.Add (snapshot);
				}
				if (IsTileLeftMost (context, i)) {
					var targetTile = context.LastSnapshot [i];
					var snapshot = CloneUpdatingTileAtIndex(i, TileData.Create(targetTile.Walls | WallTypes.Left, targetTile.MainPath, targetTile.Start, targetTile.Finish), context.LastSnapshot);
					context.Snapshots.Add (snapshot);
				}
			}
		}

		static bool IsTileLeftMost (MapGenerationContext context, int tileIndex)
		{
			return (tileIndex % context.Width) == 0;
		}

		static bool IsTileRightMost (MapGenerationContext context, int tileIndex)
		{
			return (tileIndex % context.Width) == (context.Width - 1);
		}

		static bool IsTileInFirstRow (MapGenerationContext context, int tileIndex)
		{
			return tileIndex >= 0 && tileIndex < context.Width;
		}

		static bool IsTileInLastRow (MapGenerationContext context, int tileIndex)
		{
			return tileIndex + context.Width >= context.Width*context.Height;
		}
	}
}

