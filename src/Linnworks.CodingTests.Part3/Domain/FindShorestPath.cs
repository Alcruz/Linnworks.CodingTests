using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Linnworks.CodingTests.Part3.Domain
{
	internal class FindShorestPath
	{
		private Node startNode;
		private Node goalNode;

		public FindShorestPath(World.Cell startLoc, World.Cell endLoc, Func<Location, IEnumerable<World.Cell>> successorsFunc)
		{
			this.startNode = new Node(startLoc);
			this.goalNode = new Node(endLoc);
			SuccessorsFunc = successorsFunc;
		}

		public Func<Location, IEnumerable<World.Cell>> SuccessorsFunc { get; }

		internal Location[] Run()
		{

			var node = this.startNode;
			var frontier = new SortedSet<Node>(new Node[] { node }, Node.Comparer);
			var frontierSet = new HashSet<Node>(new Node[] { node }, NodeEqualityComparer.Default);
			var explored = new HashSet<Node>(NodeEqualityComparer.Default);

			while (true)
			{
				if (!frontierSet.Any())
					return new Location[0];

				node = frontier.Max;

				frontier.Remove(node);
				frontierSet.Remove(node);

				if (node.Equals(goalNode))
					return Path(node);

				explored.Add(node);
				foreach (var child in SuccessorsFunc.Invoke(node.Location).Select(successor => new Node(successor, node)))
				{
					if (!explored.Contains(child) && !frontierSet.Contains(child) )
					{
						frontier.Add(child);
						frontierSet.Add(child);
					}
					else if (frontierSet.TryGetValue(child, out Node foundNode) && child.Cost > foundNode.Cost)
					{
						frontier.Remove(foundNode);
						frontierSet.Remove(foundNode);

						frontier.Add(child);
						frontierSet.Add(child);
					}
				}
			}
		}

		private Location[] Path(Node node) => node.Reverse().Select(n => n.Location).ToArray();
		private class Node : IEquatable<Node>, IEnumerable<Node>
		{
			private static Lazy<Comparer<Node>> LazyComparer = new Lazy<Comparer<Node>>(() => {
				return Comparer<Node>.Create((node1, node2) =>
				{
					if (node1.Cost.CompareTo(node2.Cost) != 0)
						return node1.Cost.CompareTo(node2.Cost);
					else if (node1.Location.X.CompareTo(node2.Location.X) != 0)
						return node1.Location.X.CompareTo(node2.Location.X);
					else if (node1.Location.Y.CompareTo(node2.Location.Y) != 0)
						return node1.Location.Y.CompareTo(node2.Location.Y);

					return 0;
				});
			});

			public Node(World.Cell location, Node predecessor = null)
			{
				Location = location;
				Predecessor = predecessor;
			}

			public static Comparer<Node> Comparer => LazyComparer.Value;
			public World.Cell Location { get; }
			public Node Predecessor { get; }
			public int Cost
			{
				get
				{
					if (Location.Passability == 0) return int.MinValue;

					return Location.Passability - (Predecessor?.Cost ?? 0);
				}
			}

			public bool Equals(Node other)
			{
				if (other == null) return false;
				return Location.X == other.Location.X
				 && Location.Y == other.Location.Y;
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != GetType()) return false;
				return Equals(obj as Node);
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

			public IEnumerator<Node> GetEnumerator()
			{
				Node cur = this;
				while (cur != null)
				{
					yield return cur;
					cur = cur.Predecessor;
				}
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		private class NodeEqualityComparer : EqualityComparer<Node>
		{
			private static Lazy<NodeEqualityComparer> lazyFactory = new Lazy<NodeEqualityComparer>(() => new NodeEqualityComparer());

			public static new NodeEqualityComparer Default => lazyFactory.Value;

			private NodeEqualityComparer()
			{
			}

			public override bool Equals(Node x, Node y)
			{
				return x.Equals(y);
			}

			public override int GetHashCode(Node obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}