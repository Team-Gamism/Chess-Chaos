using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Knight : PieceAbstract
{
	public List<Vector2Int> moveList = new List<Vector2Int>
{
	new Vector2Int(2, -1),
	new Vector2Int(2, 1),
	new Vector2Int(1, -2),
	new Vector2Int(1, 2),
	new Vector2Int(-1, -2),
	new Vector2Int(-1, 2),
	new Vector2Int(-2, -1),
	new Vector2Int(-2, 1)
};
	public override List<TableData> FindMoveableSpots(Vector2Int curPos, TableManager tableManager)
	{
		List<TableData> result = new List<TableData>();

		TableData curTable = null;

		foreach (TableData table in tableManager.TableList)
		{
			if (table.Coordinate == curPos)
			{
				curTable = table;
				break;
			}
		}
		if (curTable == null) return null;

		//이동 가능 경로 찾기
		for(int i = 0; i < moveList.Count; i++)
		{
			Vector2Int moveTable = new Vector2Int(curTable.Coordinate.x + moveList[i].x, 
				curTable.Coordinate.y + moveList[i].y);
			if (!OverCoordinate(moveTable)) continue;

			TableData table = tableManager.GetTableByCoordinate(moveTable);

			if (table.IsPiece)
			{
				if (table.piece.IsPlayerPiece)
				{
					continue;
				}
				table.pieceMoveAppear.DeathPiece = true;
				result.Add(table);
				continue;
			}
			else
			{
				table.pieceMoveAppear.DeathPiece = false;
			}
			if (table.IsMoveable) result.Add(table);
		}
		return result;
	}	
}
