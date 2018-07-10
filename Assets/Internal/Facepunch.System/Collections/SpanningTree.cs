using System;
using System.Collections.Generic;

public class SpanningTree<T>
{
	private List<Node> nodes = new List<Node>();
	private List<Edge> edges = new List<Edge>();

	public int AddNode()
	{
		nodes.Add(new Node());
		return nodes.Count-1;
	}

	public void AddEdge(int a_idx, int b_idx, int cost, T value)
	{
		var a = nodes[a_idx];
		var b = nodes[b_idx];
		a.edges.Add(new Edge(a, b, cost, value));
	}

	public void Clear()
	{
		nodes.Clear();
		edges.Clear();
	}

	public void Reset()
	{
		foreach (var node in nodes)
		{
			node.connected = false;
		}

		edges.Clear();
	}

	public void CalculateMin()
	{
		Reset();

		var open = new IntrusiveMinHeap<Edge>();

		foreach (var node in nodes)
		{
			if (node.connected) continue;

			foreach (var edge in node.edges)
			{
				if (edge.target.connected) continue;

				open.Add(edge);
			}

			node.connected = true;

			while (!open.Empty)
			{
				var edge = open.Pop();
				var target = edge.target;

				if (target.connected) continue;

				target.connected = true;

				foreach (var connection in target.edges)
				{
					if (connection.target == edge.source) edge = connection;

					if (connection.target.connected) continue;

					open.Add(connection);
				}

				edges.Add(edge);
			}
		}
	}

	public void ForEach(Action<T> action)
	{
		foreach (var edge in edges)
		{
			action(edge.value);
		}
	}

	private class Node
	{
		public List<Edge> edges;
		public bool connected;

		public Node()
		{
			this.edges = new List<Edge>();
			this.connected = false;
		}
	}

	private class Edge : IMinHeapNode<Edge>
	{
		public Node source;
		public Node target;
		public T value;

		public int order
		{
			get; private set;
		}

		public Edge child
		{
			get; set;
		}

		public Edge(Node source, Node target, int order, T value)
		{
			this.source = source;
			this.target = target;
			this.order  = order;
			this.value  = value;
		}
	}
}
