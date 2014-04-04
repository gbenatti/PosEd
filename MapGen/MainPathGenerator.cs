using System;

namespace MapGen
{
	internal class MainPathGenerator
	{
		static bool done;

		static public void Generate(MapGenerationContext context)
		{
			done = false;
			while (!done) {
				var nextIndex = GetNextTilePosition (context);
				RunSteps (context, nextIndex);
			}
		}	

		static Tuple<int, Directions> GetNextTilePosition (MapGenerationContext context)
		{
			var newDirirection = GetNewDirection (context);

			switch (newDirirection) {
			case Directions.Left:
				return Tuple.Create(context.LastUpdatedTileIndex - 1, newDirirection);
			case Directions.Right:
				return Tuple.Create(context.LastUpdatedTileIndex + 1, newDirirection);
			case Directions.Top:
				return Tuple.Create(context.LastUpdatedTileIndex - context.Width, newDirirection);
			case Directions.Bottom:
				return Tuple.Create(context.LastUpdatedTileIndex + context.Width, newDirirection);
			}

			return null;
		}

		static Directions GetNewDirection (MapGenerationContext context)
		{
			return context.Randomizer.RandomDirection ();
		}

		static bool RunSteps (MapGenerationContext context, Tuple<int, Directions> targetPosition)
		{
			return (
				GoLeft (context, targetPosition) || 
				GoRight(context, targetPosition) ||
				GoDown (context, targetPosition) ||
				GoTop  (context, targetPosition));
		}

		static bool GoLeft(MapGenerationContext context, Tuple<int, Directions> targetPosition)
		{
			if (targetPosition.Item2 != Directions.Left)
				return false;

			if (IsLastTileLeftMost (context))
				return false;

			if (!IsTargetTileEmpty (context, targetPosition.Item1))
				return false;

			CreateNewSnapshotWithChange (context, targetPosition.Item1, TileTypes.LeftRight);

			return true;
		}

		static bool GoRight (MapGenerationContext context, Tuple<int, Directions> targetPosition)
		{
			if (targetPosition.Item2 != Directions.Right)
				return false;

			if (IsLastTileRightMost (context))
				return false;

			if (!IsTargetTileEmpty (context, targetPosition.Item1))
				return false;

			CreateNewSnapshotWithChange (context, targetPosition.Item1, TileTypes.LeftRight);

			return true;
		}

		static bool GoDown (MapGenerationContext context, Tuple<int, Directions> targetPosition)
		{
			var down = 
				(targetPosition.Item2 == Directions.Left && IsLastTileLeftMost (context)) ||
				(targetPosition.Item2 == Directions.Right && IsLastTileRightMost(context));

			if (targetPosition.Item2 != Directions.Bottom && !down)
				return false;

			if (IsLastTileInLastRow (context)) {
				done = true;
				return false;
			}

			var newLevel = LevelGenerator.CloneUpdatingTileAtIndex (context.LastUpdatedTileIndex, TileTypes.LeftRightBottom, context.LastSnapshot);
			context.Snapshots.Add (newLevel);

			var targetIndex = context.LastUpdatedTileIndex += context.Width;

			CreateNewSnapshotWithChange (context, targetIndex, TileTypes.LeftRightTop);

			return true;
		}

		static bool GoTop (MapGenerationContext context, Tuple<int, Directions> targetPosition)
		{
			if (targetPosition.Item2 != Directions.Top)
				return false;

			if (IsLastTileInFirstRow (context))
				return false;

			if (!IsTargetTileEmpty (context, targetPosition.Item1))
				return false;

			var newLevel = LevelGenerator.CloneUpdatingTileAtIndex (context.LastUpdatedTileIndex, TileTypes.LeftRightTop, context.LastSnapshot);
			context.Snapshots.Add (newLevel);

			var targetIndex = context.LastUpdatedTileIndex -= context.Width;

			CreateNewSnapshotWithChange (context, targetIndex, TileTypes.LeftRightBottom);

			return true;
		}

		static void CreateNewSnapshotWithChange (MapGenerationContext context, int newIndex, TileTypes newTileType)
		{
			var newLevel = LevelGenerator.CloneUpdatingTileAtIndex (newIndex, newTileType, context.LastSnapshot);
			context.Snapshots.Add (newLevel);
			context.LastUpdatedTileIndex = newIndex;
		}

		static bool IsTargetTileEmpty (MapGenerationContext context, int tileIndex)
		{
			var targetTile = context.LastSnapshot [tileIndex];
			return targetTile == TileTypes.Empty;
		}

		static bool ValidDirection (Directions newDir, MapGenerationContext context)
		{
			switch (newDir) {
			case Directions.Left:
				return !IsLastTileLeftMost (context);
			case Directions.Right:
				return !IsLastTileRightMost (context);
			case Directions.Top:
				return !IsLastTileInFirstRow (context);
			case Directions.Bottom:
				return !IsLastTileInLastRow (context);
			}

			return false;

		}

		static bool IsLastTileLeftMost (MapGenerationContext context)
		{
			return (context.LastUpdatedTileIndex % context.Width) == 0;
		}

		static bool IsLastTileRightMost (MapGenerationContext context)
		{
			return (context.LastUpdatedTileIndex % context.Width) == (context.Width - 1);
		}

		static bool IsLastTileInFirstRow (MapGenerationContext context)
		{
			return context.LastUpdatedTileIndex >= 0 && context.LastUpdatedTileIndex < context.Width;
		}

		static bool IsLastTileInLastRow (MapGenerationContext context)
		{
			return context.LastUpdatedTileIndex + context.Width >= context.Width*context.Height;
		}
	}
}

