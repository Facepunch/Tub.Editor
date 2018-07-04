using System;
using System.Collections.Generic;

// Reference:
// http://www.informit.com/guides/content.aspx?g=dotnet&seqNum=626

public class MruDictionary<Key, Value>
{
	private int capacity;
	private Queue<LinkedListNode<KeyValuePair<Key, Value>>> recycled;
	private LinkedList<KeyValuePair<Key, Value>> list;
	private Dictionary<Key, LinkedListNode<KeyValuePair<Key, Value>>> dict;

	public int Count { get { return list.Count; } }

	public MruDictionary( int capacity )
	{
		this.capacity = capacity;

		list = new LinkedList<KeyValuePair<Key, Value>>();
		dict = new Dictionary<Key, LinkedListNode<KeyValuePair<Key, Value>>>( capacity );
		recycled = new Queue<LinkedListNode<KeyValuePair<Key, Value>>>( capacity );

		for ( int i = 0; i < capacity; i++ )
			recycled.Enqueue( new LinkedListNode<KeyValuePair<Key, Value>>( new KeyValuePair<Key, Value>( default( Key ), default( Value ) ) ) );
	}

	public void Clear()
	{
		list.Clear();
		dict.Clear();
	}

	public void Add( Key key, Value value )
	{
		// performs sorting when key already exists
		LinkedListNode<KeyValuePair<Key, Value>> node;
		if ( dict.TryGetValue( key, out node ) )
		{
			// already exists? move it to the front of the list
			list.Remove( node );
			list.AddFirst( node );
		}
		else
		{
			if ( dict.Count == capacity - 1 )
				RemoveLast();

			node = recycled.Dequeue();
			node.Value = new KeyValuePair<Key, Value>( key, value );

			list.AddFirst( node );
			dict.Add( key, list.First );
		}
	}

	public KeyValuePair<Key, Value> GetLast()
	{
		return list.Last.Value;
	}

	public void RemoveLast()
	{
		recycled.Enqueue( list.Last );
		dict.Remove( list.Last.Value.Key );
		list.RemoveLast();
	}

	public bool TryGetValue( Key key, out Value value )
	{
		LinkedListNode<KeyValuePair<Key, Value>> node;
		if ( dict.TryGetValue( key, out node ) )
		{
			value = node.Value.Value;
			return true;
		}
		value = default( Value );
		return false;
	}

	public bool Touch( Key key )
	{
		LinkedListNode<KeyValuePair<Key, Value>> node;
		if ( dict.TryGetValue( key, out node ) )
		{
			// move this node to the front of the list
			list.Remove( node );
			list.AddFirst( node );			
			return true;
		}
		return false;
	}	
}
