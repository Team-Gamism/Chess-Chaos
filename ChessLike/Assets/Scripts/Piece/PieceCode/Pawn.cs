using System.Collections.Generic;
using UnityEngine;

public class Pawn : PieceAbstract
{
	public bool IsFirstMove = true;//첫 움직임 확인
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

		//이동 가능 경로 찾기(직선)
		for (int i = 1; i <= (IsFirstMove ? 2 : 1); i++)
		{
			Vector2Int moveTable = new Vector2Int(curTable.Coordinate.x, curTable.Coordinate.y - i);
			if(!OverCoordinate(moveTable)) continue;

			TableData table = tableManager.GetTableByCoordinate(moveTable);
			if (table.IsMoveable) result.Add(table);
		}

		//이동 가능 경로 찾기(대각선)
		for (int i = -1; i <= 1; i += 2)
		{
			Vector2Int moveTable = new Vector2Int(curTable.Coordinate.x + i, curTable.Coordinate.y - 1);
			if (!OverCoordinate(moveTable)) continue;

			TableData table = tableManager.GetTableByCoordinate(moveTable);
			if (table.IsPiece) result.Add(table);
		}
		return result;
	}

}
