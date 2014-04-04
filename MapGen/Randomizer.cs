using System;
using System.Collections.Generic;

namespace MapGen
{
	public class Randomizer
	{
		Random random = new Random();

		public int RandomIndex (int startIncluded, int endExcluded)
		{
			var range = endExcluded - startIncluded;
			var newIndex = startIncluded + (int)(random.NextDouble () * range);
			return Math.Max(startIncluded, Math.Min(newIndex, endExcluded));
		}

		public Directions RandomDirection()
		{ 
			var dirs = new List<Directions> {Directions.Left, Directions.Left, Directions.Left, Directions.Right, Directions.Right, Directions.Right, Directions.Bottom, Directions.Top};
			var index = RandomIndex (0, 8);
			return dirs[index];
		}
	}
}

