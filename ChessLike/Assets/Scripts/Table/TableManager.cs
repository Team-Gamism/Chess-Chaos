using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
	public List<TableData> TableList = new List<TableData>();
	public Canvas PieceCanvas;

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

	public Vector2 ReturnNearTable(Vector2 pos)
	{
		float distance = float.MaxValue;
		Vector2 result = Vector2.zero;
		foreach(TableData table in TableList)
		{
			Vector2 tablePos = table.positionToRect(PieceCanvas);
			Debug.Log(tablePos);
			float _distance = Vector2.Distance(tablePos, pos);
			if (_distance < distance)
			{
				distance = _distance;
				result = tablePos;
			}
		}
		return result;
	}
}
