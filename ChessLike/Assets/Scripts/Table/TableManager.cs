using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
	public List<TableData> TableList = new List<TableData>();

	public void Awake()
	{
		TableData[] tables = FindObjectsOfType<TableData>();
		foreach(TableData table in tables)
		{
			TableList.Add(table);
		}
		TableList.Sort((a, b) =>
		{
			if (a.Coordinate.y != b.Coordinate.y)
				return a.Coordinate.y.CompareTo(b.Coordinate.y);
			else
				return a.Coordinate.x.CompareTo(b.Coordinate.x);
		});
	}
	

}
