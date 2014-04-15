using System;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace MapGen
{
	class TileDescriptors
	{
		bool loaded;

		WallTypes actualRoomType;
		List<byte> newRoom;

		Dictionary<WallTypes, List<byte[,]>> RoomDescriptors = new Dictionary<WallTypes, List<byte[,]>>();

		public TileDescriptors ()
		{
			loaded = false;
		}

		public void Load(Stream stream)
		{
			if (loaded)
				return;

			var data = LoadFromStream (stream);
			ParseRoomDescriptorsFile (data);
		}

		public byte[,] ByWallTypes(WallTypes walls, Randomizer randomizer)
		{
			var count = RoomDescriptors [walls].Count();
			var index = randomizer.RandomIndex (0, count);
			return RoomDescriptors [walls] [index];
		}

		string LoadFromStream (Stream stream)
		{
			loaded = true;
			return new System.IO.StreamReader(stream).ReadToEnd();	
		}

		void ParseRoomDescriptorsFile (string data)
		{
			var lines = data.Split('\n');
			foreach (var line in lines) {
				ParseLine (line);
			}

			EndActualRoom ();
		}

		void ParseLine (string line)
		{
			if (line.StartsWith (">")) 
			{
				SetRoomType (CreateWalls(line));
				StartingNewRoom ();
				return;
			}
			if (line.StartsWith("-"))
			{
				EndActualRoom ();
				StartingNewRoom ();
				return;
			}

			ProcessRoomLine (line);
		}

		void SetRoomType (WallTypes walls)
		{
			actualRoomType = walls;
		}

		void StartingNewRoom ()
		{
			newRoom = new List<byte> ();
		}

		void EndActualRoom()
		{
			if (newRoom.Count <= 0)
				return;

			if (!RoomDescriptors.ContainsKey (actualRoomType))
				RoomDescriptors[actualRoomType] = new List<byte[,]> ();

			var listOfRooms = RoomDescriptors [actualRoomType];
			listOfRooms.Add (RoomFromList(newRoom));
		}

		byte[,] RoomFromList (List<byte> newRoom)
		{
			var tile = new byte[16,9];

			for (int l = 0; l < 9; l++) {
				for (int c = 0; c < 16; c++) {
					tile[c, l] = newRoom[l * 16 + c];
				}
			}

			return tile;
		}
			
		WallTypes CreateWalls (string line)
		{
			var walls = from wallName in line.Substring (1).Split ('|') select (WallTypes)Enum.Parse(typeof(WallTypes), wallName);
			return walls.Aggregate(WallTypes.None, (current, item) => current|item);
		}

		void ProcessRoomLine (string line)
		{
			newRoom.AddRange (
				from digit in line.ToArray ()
				select Byte.Parse (digit.ToString()));
		}
	}

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

		static TileDescriptors tileDescriptors = new TileDescriptors();
		static Randomizer randomizer = new Randomizer();

		public TileData ()
		{
			tileDescriptors.Load (TitleContainer.OpenStream("Content/Rooms.txt"));

			Walls = WallTypes.None;
			PathIndex = -1;
			MainPath = false;
			Empty = true;
			Start = false;
			Finish = false;
			Blocks = tileDescriptors.ByWallTypes(Walls, randomizer);
		}

		public static TileData Create (WallTypes walls, bool mainPath, bool start = false, bool finish = false)
		{
			tileDescriptors.Load (TitleContainer.OpenStream("Content/Rooms.txt"));

			return new TileData {
				Walls = walls,
				MainPath = mainPath,
				Empty = false,
				Start = start,
				Finish = finish,
				Blocks = tileDescriptors.ByWallTypes(walls, randomizer)
			};
		}
	}
}
