using System.Collections;
using KutCode.Optionality;

namespace TestTasks.Task5;

/// <summary>
/// Very simple and naive one way linked list implementation, just for task resolving and testing
/// </summary>
/// <typeparam name="T">List Typed param</typeparam>
public sealed class OneWayLinkedList<T> : IEnumerable<T>
{
	/// <summary>
	/// First element in a row.
	/// </summary>
	public OneWayLinkedListNode<T>? Head { get; set; } 
	public OneWayLinkedListNode<T>? Last { get; set; } 

	/// <summary>
	/// Adds element to the end of sequence
	/// </summary>
	public void AddLast(OneWayLinkedListNode<T> node)
	{
		if (Head is null) Head = node;
		if (Last is {Next: null}) Last.Next = node;
		Last = node;
	}
	
	IEnumerator IEnumerable.GetEnumerator()=> GetEnumerator();
	public IEnumerator<T> GetEnumerator()
	{
		if (Head == null) yield break;
		Optional<OneWayLinkedListNode<T>> justReturned;
		yield return Head.NodeValue;
		justReturned = Head;
		
		while (justReturned.Value!.Next != null) {
			yield return justReturned.Value.Next.NodeValue;
			justReturned = justReturned.Value.Next;
		}
	}
}

/// <summary>
/// Just a node with a pointer only to next element
/// </summary>
public sealed class OneWayLinkedListNode<TValue>
{
	public TValue NodeValue  { get; private set; }
	public OneWayLinkedListNode<TValue>? Next { get; set; }
		
	public OneWayLinkedListNode(TValue nodeValue) => NodeValue = nodeValue;
	public OneWayLinkedListNode(TValue nodeValue, OneWayLinkedListNode<TValue> next) => (NodeValue, Next) = (nodeValue, next);
	public static OneWayLinkedListNode<TValue> From(TValue value) => new(value);
	public static OneWayLinkedListNode<TValue> From(TValue value, OneWayLinkedListNode<TValue> nextNode) => new(value, nextNode);
		
	public override string ToString() => NodeValue!.ToString() ?? string.Empty;
}