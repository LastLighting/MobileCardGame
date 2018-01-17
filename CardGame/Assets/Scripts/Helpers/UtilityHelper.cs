using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityHelper {

	public static List<T> RandomizeList<T>(List<T> list)
	{
		List<T> randomizedList = new List<T>();
		while (list.Count > 0)
		{
			int index = Random.Range(0, list.Count);
			randomizedList.Add(list[index]);
			list.RemoveAt(index);
		}
		return randomizedList;
	}
}
