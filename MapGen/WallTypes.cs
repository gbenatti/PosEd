using System;

namespace MapGen
{
	[Flags]
	public enum WallTypes
	{
		None 	= 0,
		Left 	= 1,
		Right 	= 2,
		Top 	= 4,
		Bottom 	= 8
	}
}

