using System;

public struct IntrusiveLinkedList<T> where T : ILinkedListNode<T>
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
		else
		{
			item.next = head;
			head = item;
		}
	}

	public T Pop()
	{
		var res = head;

		head = head.next;
		res.next = default(T);

		return res;
	}
}

public interface ILinkedListNode<T>
{
	T next { get; set; }
}
