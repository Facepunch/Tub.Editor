using System.Collections;
using System.Collections.Generic;

public class ListHashSet<T> : IEnumerable<T>
{
	private Dictionary<T, int> val2idx;
	private Dictionary<int, T> idx2val;

	private BufferList<T> vals;

	public ListHashSet(int capacity = 8)
	{
		val2idx = new Dictionary<T, int>(capacity);
		idx2val = new Dictionary<int, T>(capacity);

		vals = new BufferList<T>(capacity);
	}

	public void Add(T val)
	{
		int idx = vals.Count;

		val2idx.Add(val, idx);
		idx2val.Add(idx, val);

		vals.Add(val);
	}

	public void AddRange(List<T> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			Add(list[i]);
		}
	}

	public bool Contains(T val)
	{
		return val2idx.ContainsKey(val);
	}

	public bool Remove(T val)
	{
		int idx;
		if (!val2idx.TryGetValue(val, out idx)) return false;
		Remove(idx, val);
		return true;
	}

	public bool RemoveAt(int idx)
	{
		T val;
		if (!idx2val.TryGetValue(idx, out val)) return false;
		Remove(idx, val);
		return true;
	}

	public void Clear()
	{
		if (Count == 0) return;

		val2idx.Clear();
		idx2val.Clear();
		vals.Clear();
	}

	private void Remove(int idx_remove, T val_remove)
	{
		int idx_update = vals.Count-1;
		var val_update = idx2val[idx_update];

		// Remove value
		vals.RemoveUnordered(idx_remove);

		// Update moved value in the lookup dictionaries
		val2idx[val_update] = idx_remove;
		idx2val[idx_remove] = val_update;

		// Remove outdated value from the lookup dictionaries
		val2idx.Remove(val_remove);
		idx2val.Remove(idx_update);
	}

	public BufferList<T> Values
	{
		get { return vals; }
	}

	public int Count
	{
		get { return vals.Count; }
	}

	public T this[int index]
	{
		 get { return vals[index];  }
		 set { vals[index] = value; }
	}

	public IEnumerator<T> GetEnumerator()
	{
		for (int i = 0; i < vals.Count; i++)
		{
			yield return vals[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
