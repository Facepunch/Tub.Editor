using System;

public struct IntrusiveMinHeap<T> where T : IMinHeapNode<T>
{
	private T head;

	public bool Empty
	{
		get { return head == null; }
	}

	public void Add(T item)
	{
		if (head == null)
		{
			head = item;
		}
		else if (head.child == null && item.order <= head.order)
		{
			item.child = head;
			head = item;
		}
		else
		{
			var prev = head;

			while (prev.child != null && prev.child.order < item.order) prev = prev.child;

			item.child = prev.child;
			prev.child = item;
		}
	}

	public T Pop()
	{
		var res = head;

		head = head.child;
		res.child = default(T);

		return res;
	}
}

public interface IMinHeapNode<T>
{
	T child { get; set; }
	int order { get; }
}
