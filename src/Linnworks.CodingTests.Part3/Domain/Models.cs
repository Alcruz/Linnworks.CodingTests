using System;
using System.Collections.Generic;

namespace Linnworks.CodingTests.Part3.Domain
{
	public class Location
	{
		public int X;
		public int Y;

		public Location(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
	}

	public class World
	{
		public class Cell : Location
		{
			// Defines cell passability from 0 (can't go) to 100 (normal passability)
			// The higher is passability, the quicker it is possible to pass the cell
			public byte Passability;

			public Cell(int x, int y, byte passability)
				: base(x, y)
			{
				this.Passability = passability;
			}
		}

		private Cell[,] cells; // World map

		public World(int sizeX, int sizeY)
		{
			var rnd = new Random();

			// Build map and randomly set passability for each cell
			cells = new Cell[sizeX, sizeY];
			for (int x = 0; x < sizeX; x++)
				for (int y = 0; y < sizeY; y++)
					cells[x, y] = new Cell(x, y, (byte)rnd.Next(0, 100));
		}

		public Location[] FindShortestWay(Location startLoc, Location endLoc)
		{
			// TODO: Implement finding most shortest way between start and end locations
			return new FindShorestPath(this.cells[startLoc.X, startLoc.Y], this.cells[endLoc.X, endLoc.Y], nextLoc =>
			{
				var successors = new List<Cell>();
				if (nextLoc.X + 1 < this.cells.GetLength(0))
					successors.Add(this.cells[nextLoc.X + 1, nextLoc.Y]);

				if (nextLoc.Y + 1 < this.cells.GetLength(1))
					successors.Add(this.cells[nextLoc.X, nextLoc.Y + 1]);

				if (nextLoc.X - 1 >= 0)
					successors.Add(this.cells[nextLoc.X - 1, nextLoc.Y]);

				if (nextLoc.Y - 1 >= 0)
					successors.Add(this.cells[nextLoc.X, nextLoc.Y - 1]);

				return successors;
			}).Run();
		}

		public void Print() {

			int rowLength = this.cells.GetLength(0);
			int colLength = this.cells.GetLength(1);

			for (int i = 0; i < rowLength; i++)
			{
				for (int j = 0; j < colLength; j++)
				{
					Console.Write(string.Format("{0} ", this.cells[i, j].Passability));
				}
				Console.Write(Environment.NewLine + Environment.NewLine);
			}
		}
	}
}
