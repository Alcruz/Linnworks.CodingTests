using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Linnworks.CodingTests.Part3.Domain
{
	internal class FindShorestPath<T> where T : State<T>
	{
		private Node startNode;
		private Node goalNode;

		public FindShorestPath(State<T> startLoc, State<T> endLoc, Func<State<T>, IEnumerable<State<T>>> successorsFunc)
		{
			this.startNode = new Node(startLoc);
			this.goalNode = new Node(endLoc);
			SuccessorsFunc = successorsFunc;
		}

		public Func<State<T>, IEnumerable<State<T>>> SuccessorsFunc { get; }

		internal State<T>[] Run()
		{

			var node = this.startNode;
			var frontier = new SortedSet<Node>(new Node[] { node }, Node.Comparer);
			var frontierSet = new HashSet<Node>(new Node[] { node }, NodeEqualityComparer.Default);
			var explored = new HashSet<Node>(NodeEqualityComparer.Default);

			while (true)
			{
				if (!frontierSet.Any())
					return new State<T>[0];

				node = frontier.Max;

				frontier.Remove(node);
				frontierSet.Remove(node);

				if (node.State.Equals(goalNode.State))
					return Path(node);

				explored.Add(node);
				foreach (var child in SuccessorsFunc.Invoke(node.State).Select(successor => new Node(successor, node)))
				{
					if (child.State.Cost > 0)
					{
						if (!explored.Contains(child) && !frontierSet.Contains(child))
						{
							frontier.Add(child);
							frontierSet.Add(child);
						}
						else if (frontierSet.TryGetValue(child, out Node foundNode) && child.PathCost > foundNode.PathCost)
						{
							frontier.Remove(foundNode);
							frontierSet.Remove(foundNode);

							frontier.Add(child);
							frontierSet.Add(child);
						}
					}
				}
			}
		}

		private State<T>[] Path(Node node) => node.Reverse().Select(n => n.State).ToArray();
		private class Node : IEnumerable<Node>
		{
			private static Lazy<Comparer<Node>> LazyComparer = new Lazy<Comparer<Node>>(() => {
				return Comparer<Node>.Create((node1, node2) =>
				{
					if (node1.PathCost.CompareTo(node2.PathCost) != 0)
						return node1.PathCost.CompareTo(node2.PathCost);
					
					return node1.State.CompareTo(node2.State);
				});
			});

			public Node(State<T> location, Node predecessor = null)
			{
				State = location;
				Predecessor = predecessor;
			}

			public static Comparer<Node> Comparer => LazyComparer.Value;
			public State<T> State { get; }
			public Node Predecessor { get; }
			public int PathCost
			{
				get
				{
					if (State.Cost == 0) return int.MinValue;

					return State.Cost - (Predecessor?.PathCost ?? 0);
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
				return x.State.Equals(y.State);
			}

			public override int GetHashCode(Node obj)
			{
				return obj.State.GetHashCode();
			}
		}
	}

	public abstract class State<T> : IEquatable<T>, IComparable<State<T>> where T : State<T>
	{
		public abstract int Cost { get; }

		public abstract int CompareTo(State<T> other);

		public abstract bool Equals(T other);
	}
}