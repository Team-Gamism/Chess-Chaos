using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : PieceAbstract
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

		for(int i = -1; i <= 1; i++)
		{
			for(int j = -1;  j <= 1; j++)
			{
				Vector2Int moveTable = new Vector2Int(curTable.Coordinate.x + i, curTable.Coordinate.y + j);
				if (!OverCoordinate(moveTable)) break;
				if (i == 0 && j == 0) continue;

				TableData table = tableManager.GetTableByCoordinate(moveTable);

				if (table.IsPiece)
				{
					if (table.piece.IsPlayerPiece)
					{
						break;
					}
					table.pieceMoveAppear.DeathPiece = true;
					result.Add(table);
					break;
				}
				else
				{
					table.pieceMoveAppear.DeathPiece = false;
				}

				if (table.IsMoveable) result.Add(table);
			}
		}
		return result;
	}
}
