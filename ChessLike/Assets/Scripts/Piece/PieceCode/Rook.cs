using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : PieceAbstract
{
	public override List<TableData> FindMoveableSpots(Vector2Int curPos, TableManager tableManager)
	{
		List<TableData> result = new List<TableData>();

		TableData curTable = null;

		//현재 위치 찾기
		foreach (TableData table in tableManager.TableList)
		{
			if (table.Coordinate == curPos)
			{
				curTable = table;
				break;
			}
		}
		if (curTable == null) return null;

		//이동 경로 찾기

		//오른쪽 경로 탐색
		for(int i = 1; i < 8; i++)
		{
			Vector2Int moveTable = new Vector2Int(curTable.Coordinate.x + i, curTable.Coordinate.y);
			if (!OverCoordinate(moveTable)) break;

			TableData table = tableManager.GetTableByCoordinate(moveTable);

			if (table.IsPiece) break;

			if (table.IsMoveable) result.Add(table);
		}
		
		//왼쪽 경로 탐색
		for(int i = 1; i < 8; i++)
		{
			Vector2Int moveTable = new Vector2Int(curTable.Coordinate.x - i, curTable.Coordinate.y);
			if (!OverCoordinate(moveTable)) break;

			TableData table = tableManager.GetTableByCoordinate(moveTable);

			if (table.IsPiece) break;

			if (table.IsMoveable) result.Add(table);
		}

		//위쪽 경로 탐색
		for (int i = 1; i < 8; i++)
		{
			Vector2Int moveTable = new Vector2Int(curTable.Coordinate.x, curTable.Coordinate.y + i);
			if (!OverCoordinate(moveTable)) break;

			TableData table = tableManager.GetTableByCoordinate(moveTable);

			if (table.IsPiece) break;

			if (table.IsMoveable) result.Add(table);
		}

		//아래쪽 경로 탐색
		for (int i = 1; i < 8; i++)
		{
			Vector2Int moveTable = new Vector2Int(curTable.Coordinate.x, curTable.Coordinate.y - i);
			if (!OverCoordinate(moveTable)) break;

			TableData table = tableManager.GetTableByCoordinate(moveTable);

			if (table.IsPiece) break;

			if (table.IsMoveable) result.Add(table);
		}

		return result;
	}
}
