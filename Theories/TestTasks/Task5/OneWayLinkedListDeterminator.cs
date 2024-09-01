namespace TestTasks.Task5;

/// <summary>
/// 5.	There is a one-way linked list (each node has only one reference to the next one).
/// Please write code that determines if this list “closed” (has a node that is referenced by several nodes).
/// </summary>
public class OneWayLinkedListDeterminator<T>
{
	// LinkedList<T> class in .NET as a doubly-linked list, not a one-way linked list.
	
	/// <summary>
	/// Find out has linked list looped
	/// </summary>
	public static bool IsClosed(OneWayLinkedList<T> list)
	{
		if (list.Head is null) return false;
		HashSet<OneWayLinkedListNode<T>> exists = new();
		var head = list.Head;
		while (head.Next is not null) {
			if (exists.Contains(head.Next))
				return true;
			else 
				exists.Add(head.Next);
			head = head.Next;
		}
		return false;
	}
}

public class OneWayLinkedListDeterminator
{
	//preset lists
	public static OneWayLinkedList<int> LoopList 
	{
		get {
			OneWayLinkedList<int> list = new();
			var node1 = OneWayLinkedListNode<int>.From(1);
			var node2 = OneWayLinkedListNode<int>.From(2);
			var node3 = OneWayLinkedListNode<int>.From(3);
			var node4 = OneWayLinkedListNode<int>.From(4);
			var node5 = OneWayLinkedListNode<int>.From(5, node2); // <-- loop
			var node6 = OneWayLinkedListNode<int>.From(6);
			var node7 = OneWayLinkedListNode<int>.From(7);
			list.AddLast(node1);
			list.AddLast(node2);
			list.AddLast(node3);
			list.AddLast(node4);
			list.AddLast(node5);
			list.AddLast(node6);
			list.AddLast(node7);
			return list;
		}
	}

	public static OneWayLinkedList<int> NotLoopedList {
		get {
			OneWayLinkedList<int> list = new();
			list.AddLast(OneWayLinkedListNode<int>.From(1));
			list.AddLast(OneWayLinkedListNode<int>.From(2));
			list.AddLast(OneWayLinkedListNode<int>.From(3));
			list.AddLast(OneWayLinkedListNode<int>.From(4));
			list.AddLast(OneWayLinkedListNode<int>.From(5));
			list.AddLast(OneWayLinkedListNode<int>.From(6));
			list.AddLast(OneWayLinkedListNode<int>.From(7));
			return list;
		}
	}
}