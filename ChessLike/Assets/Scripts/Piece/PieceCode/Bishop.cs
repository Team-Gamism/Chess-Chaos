using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : PieceAbstract
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


		//이동 가능 경로 탐색(1사분면)
		for (int i = 1; i < 8; i++)
		{
			Vector2Int moveTable = new Vector2Int(curTable.Coordinate.x + i, curTable.Coordinate.y + i);
			if (!OverCoordinate(moveTable)) break;

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
			else break;
		}

		//이동 가능 경로 탐색(2사분면)
		for (int i = 1; i < 8; i++)
		{
			Vector2Int moveTable = new Vector2Int(curTable.Coordinate.x - i, curTable.Coordinate.y + i);
			if (!OverCoordinate(moveTable)) break;

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
			else break;
		}

		//이동 가능 경로 탐색(3사분면)
		for (int i = 1; i < 8; i++)
		{
			Vector2Int moveTable = new Vector2Int(curTable.Coordinate.x - i, curTable.Coordinate.y - i);
			if (!OverCoordinate(moveTable)) break;

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
			else break;
		}

		//이동 가능 경로 탐색(4사분면)
		for (int i = 1; i < 8; i++)
		{
			Vector2Int moveTable = new Vector2Int(curTable.Coordinate.x + i, curTable.Coordinate.y - i);
			if (!OverCoordinate(moveTable)) break;

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
			else break;
		}

		return result;
	}
}
