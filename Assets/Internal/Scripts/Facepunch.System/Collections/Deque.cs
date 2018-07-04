using System;

public class Deque<T>
{
	private T[] buffer;
	private int offset;
	private int count;

	public Deque(int capacity = 8)
	{
		buffer = new T[capacity];
	}

	public int Count
	{
		get { return count; }
	}

	public bool IsEmpty
	{
		get { return Count == 0; }
	}

	public bool IsFull
	{
		get { return Count == Capacity; }
	}

	public bool IsSplit
	{
		get { return offset > (Capacity - Count); }
	}

	public int Capacity
	{
		get { return buffer.Length; }
	}

	public T Front
	{
		get
		{
			if (IsEmpty) return default(T);

			return buffer[offset];
		}
	}

	public T Back
	{
		get
		{
			if (IsEmpty) return default(T);

			return buffer[(count + offset - 1) % Capacity];
		}
	}

	public void Clear()
	{
		offset = count = 0;
	}

	public void Resize(int capacity)
	{
		if (capacity <= Capacity) return;

		T[] newBuffer = new T[capacity];
		if (IsSplit)
		{
			int length = Capacity - offset;
			Array.Copy(buffer, offset, newBuffer, 0, length);
			Array.Copy(buffer, 0, newBuffer, length, Count - length);
		}
		else
		{
			Array.Copy(buffer, offset, newBuffer, 0, Count);
		}

		buffer = newBuffer;
		offset = 0;
	}

	public void PushBack(T value)
	{
		if (IsFull) Resize(Capacity * 2);

		buffer[(count + offset) % Capacity] = value;
		count++;
	}

	public void PushFront(T value)
	{
		if (IsFull) Resize(Capacity * 2);

		if (--offset < 0) offset += Capacity;
		buffer[offset] = value;
		count++;
	}

	public T PopBack()
	{
		if (IsEmpty) return default(T);

		T res = buffer[(count + offset - 1) % Capacity];
		count--;

		return res;
	}

	public T PopFront()
	{
		if (IsEmpty) return default(T);

		T res = buffer[offset];
		offset = (offset + 1) % Capacity;
		count--;

		return res;
	}
}
