using System;

public class MinQueue
{
	private Deque<int> data;
	private Deque<int> min;

	public MinQueue(int capacity = 8)
	{
		data = new Deque<int>(capacity);
		min  = new Deque<int>(capacity);
	}

	public void Push(int value)
	{
		data.PushBack(value);
		while (!min.IsEmpty && min.Back > value)
		{
			min.PopBack();
		}
		min.PushBack(value);
	}

	public int Pop()
	{
		if (min.Front == data.Front) min.PopFront();
		return data.PopFront();
	}

	public int Min
	{
		get { return min.Front; }
	}
}

public class MaxQueue
{
	private Deque<int> data;
	private Deque<int> max;

	public MaxQueue(int capacity = 8)
	{
		data = new Deque<int>(capacity);
		max  = new Deque<int>(capacity);
	}

	public void Push(int value)
	{
		data.PushBack(value);
		while (!max.IsEmpty && max.Back < value)
		{
			max.PopBack();
		}
		max.PushBack(value);
	}

	public int Pop()
	{
		if (max.Front == data.Front) max.PopFront();
		return data.PopFront();
	}

	public int Max
	{
		get { return max.Front; }
	}
}
