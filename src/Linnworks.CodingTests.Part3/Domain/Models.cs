using System;
using System.Collections.Generic;
using System.Linq;

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
		public class CellState : State<CellState>
		{
			public CellState(Cell cell)
			{
				Location = cell;
			}
 
			public Cell Location { get; }
			public int X => Location.X;
			public int Y => Location.Y;
			public override int Cost => Location.Passability;

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != GetType()) return false;
				return Equals(obj as CellState);
			}

			public override int GetHashCode()
			{
				unchecked
				{
					var hashCode = 13;
					hashCode = (hashCode * 397) ^ Location.X;
					hashCode = (hashCode * 397) ^ Location.Y;
					return hashCode;
				}
			}

			public override bool Equals(CellState other)
			{
				if (other == null) return false;
				return Location.X == other.Location.X && Location.Y == other.Location.Y;
			}

			public override int CompareTo(State<CellState> other)
			{
				var otherCellState = other as CellState;
				if (otherCellState.Location.X.CompareTo(Location.X) != 0)
					return Location.X.CompareTo(otherCellState.Location.X);
				else if (otherCellState.Location.Y.CompareTo(Location.Y) != 0)
					return Location.Y.CompareTo(otherCellState.Location.Y);

				return 0;
			}
		}

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
			return new FindShorestPath<CellState>(new CellState(this.cells[startLoc.X, startLoc.Y]), new CellState(this.cells[endLoc.X, endLoc.Y]), currentState =>
			{
				var cellState = currentState as CellState;
				var successors = new List<Cell>();
				if (cellState.X + 1 < this.cells.GetLength(0))
					successors.Add(this.cells[cellState.X + 1, cellState.Y]);

				if (cellState.Y + 1 < this.cells.GetLength(1))
					successors.Add(this.cells[cellState.X, cellState.Y + 1]);

				if (cellState.X - 1 >= 0)
					successors.Add(this.cells[cellState.X - 1, cellState.Y]);

				if (cellState.Y - 1 >= 0)
					successors.Add(this.cells[cellState.X, cellState.Y - 1]);

				return successors.Select(successor => new CellState(successor));
			}).Run().OfType<CellState>().Select(cellState => cellState.Location).ToArray();
		}
	}
}
