using System.Numerics;

namespace TestTasks.Task1;

public class CustomSorter
{
	[Flags]
	public enum SortDirection {
		Ascending, Descending
	}
	[Flags]
	public enum KnownArrayState {
		Unknown, Sorted, Chaotic
	}
	
	/// <summary>
	/// selfmade quick-sort implementation
	/// </summary>
	public static T[] Sort<T>(
		T[] array, 
		SortDirection sortDirection = SortDirection.Ascending, 
		KnownArrayState knownArrayState = KnownArrayState.Unknown
	) where T : INumber<T>
	{
		if (array.Length == 0) return array;
		InternalSort(array, 0, array.Length-1);
		return array;
		
		void InternalSort(T[] array, int left, int right)
		{
			if (left > right) return;
			var p = Pivot(array, left, right);
			InternalSort(array, left, p - 1);
			InternalSort(array, p + 1, right);
		}
		int Pivot(T[] array, int left, int right)
		{
			var index = left - 1;
			var piv = GetPivot(array, left, right);

			for (int i = left; i <= right; i++) {
				if (sortDirection == SortDirection.Ascending 
					    ? array[i] > piv : array[i] < piv) continue;
				index++;
				Swap(ref array[index], ref array[i]);
			}
			return index;
			
			T GetPivot(T[] arr, int l, int r)
			{
				if (knownArrayState == (KnownArrayState.Unknown | KnownArrayState.Sorted))
					return arr[(r - l) / 2 + l];
				else return arr[r];
			}
		}

		void Swap(ref T a, ref T b)
		{
			T temp = a;
			(a, b) = (b, temp);
		}
	}
}