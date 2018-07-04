using System;
using System.Collections;
using System.Collections.Generic;

//
// What the fuck, right?
// The reason this exists is because iterating over a list is twice as slow as iterating over an array
// This can make a significant difference for things that regularly iterate over long lists
// With this they can simply iterate over the buffer array from zero to count with no overhead
//

public class BufferList<T> : IEnumerable<T>
{
	public int Count
	{
		get { return count; }
	}

	public int Capacity
	{
		get { return buffer.Length; }
	}

	public T[] Buffer
	{
		get { return buffer; }
	}

	public T this[int index]
	{
		 get { return buffer[index];  }
		 set { buffer[index] = value; }
	}

	private int count;
	private T[] buffer;

	public BufferList(int capacity = 8)
	{
		buffer = new T[capacity];
	}

	public void Add(T element)
	{
		if (count == buffer.Length) Array.Resize(ref buffer, buffer.Length * 2);

		buffer[count] = element;
		count++;
	}

	public bool Remove(T element)
	{
		var index = Array.IndexOf(buffer, element);

		if (index == -1) return false;

		RemoveAt(index);
		return true;
	}

	public void RemoveAt(int index)
	{
		for (int i = index; i < count - 1; i++)
		{
			buffer[i] = buffer[i + 1];
		}

		buffer[count - 1] = default(T);
		count--;
	}

	public void RemoveUnordered(int index)
	{
		buffer[index] = buffer[count - 1];

		buffer[count - 1] = default(T);
		count--;
	}

	public int IndexOf(T element)
	{
		return Array.IndexOf(buffer, element);
	}

	public int LastIndexOf(T element)
	{
		return Array.LastIndexOf(buffer, element);
	}

	public bool Contains(T element)
	{
		return Array.IndexOf(buffer, element) != -1;
	}

	public void Clear()
	{
		if (count == 0) return;

		Array.Clear(buffer, 0, count);
		count = 0;
	}

	public void Sort()
	{
		Array.Sort(buffer);
	}

	public IEnumerator<T> GetEnumerator()
	{
		for (int i = 0; i < count; i++)
		{
			yield return buffer[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
