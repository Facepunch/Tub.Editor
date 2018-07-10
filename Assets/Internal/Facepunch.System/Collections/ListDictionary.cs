using System.Collections;
using System.Collections.Generic;

public class ListDictionary<TKey, TVal> : IEnumerable<KeyValuePair<TKey, TVal>>
{
	private Dictionary<TKey, int> key2idx;
	private Dictionary<int, TKey> idx2key;

	private BufferList<TKey> keys;
	private BufferList<TVal> vals;

	public ListDictionary(int capacity = 8)
	{
		key2idx = new Dictionary<TKey, int>(capacity);
		idx2key = new Dictionary<int, TKey>(capacity);

		keys = new BufferList<TKey>(capacity);
		vals = new BufferList<TVal>(capacity);
	}

	public void Add(TKey key, TVal val)
	{
		int idx = keys.Count;

		key2idx.Add(key, idx);
		idx2key.Add(idx, key);

		keys.Add(key);
		vals.Add(val);
	}

	public bool Contains(TKey key)
	{
		return key2idx.ContainsKey(key);
	}

	public bool Remove(TKey key)
	{
		int idx;
		if (!key2idx.TryGetValue(key, out idx)) return false;
		Remove(idx, key);
		return true;
	}

	public bool RemoveAt(int idx)
	{
		TKey key;
		if (!idx2key.TryGetValue(idx, out key)) return false;
		Remove(idx, key);
		return true;
	}

	private void Remove(int idx_remove, TKey key_remove)
	{
		int idx_update = keys.Count-1;
		var key_update = idx2key[idx_update];

		// Remove key and value
		keys.RemoveUnordered(idx_remove);
		vals.RemoveUnordered(idx_remove);

		// Update moved key and value in the lookup dictionaries
		key2idx[key_update] = idx_remove;
		idx2key[idx_remove] = key_update;

		// Remove outdated key and value from the lookup dictionaries
		key2idx.Remove(key_remove);
		idx2key.Remove(idx_update);
	}

	public bool TryGetValue(TKey key, out TVal val)
	{
		int idx;
		if (key2idx.TryGetValue(key, out idx))
		{
			val = vals[idx];
			return true;
		}

		val = default(TVal);
		return false;
	}

	public KeyValuePair<TKey, TVal> GetByIndex(int idx)
	{
		return new KeyValuePair<TKey, TVal>(idx2key[idx], vals[idx]);
	}

	public TVal this[TKey key]
	{
		get
		{
			return vals[key2idx[key]];
		}
		set
		{
			vals[key2idx[key]] = value;
		}
	}

	public void Clear()
	{
		if (Count == 0) return;

		key2idx.Clear();
		idx2key.Clear();
		keys.Clear();
		vals.Clear();
	}

	public BufferList<TKey> Keys
	{
		get { return keys; }
	}

	public BufferList<TVal> Values
	{
		get { return vals; }
	}

	public int Count
	{
		get { return vals.Count; }
	}

	public IEnumerator<KeyValuePair<TKey, TVal>> GetEnumerator()
	{
		for (int i = 0; i < vals.Count; i++)
		{
			yield return GetByIndex(i);
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
