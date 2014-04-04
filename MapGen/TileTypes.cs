using System;

namespace MapGen
{
	[Flags]
	public enum Walls
	{
		Left,
		Right,
		Top,
		Bottom
	}

	public enum TileTypes
	{
		Empty,
		LeftRight,
		LeftRightBottom,
		LeftRightTop,
		LeftRightLost,
		LeftRightBottomLost,
		LeftRightTopLost
	}

}

