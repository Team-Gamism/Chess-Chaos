using System.Collections.Generic;
using UnityEngine;

public class Pawn : PieceAbstract
{
	public bool IsFirstMove = true; //첫 움직임 확인
	public bool CanMoveSide = false;
	public bool IsShield = false;

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

			//Debug.Log($"{gameObject.name}, {moveTable}, {table == null}");

			if (table.IsPiece) break;

			if (table.IsMoveable) result.Add(table);
		}

		//이동 가능 경로 찾기(대각선)
		for (int i = -1; i <= 1; i += 2)
		{
			Vector2Int moveTable = new Vector2Int(curTable.Coordinate.x + i, curTable.Coordinate.y - 1);
			if (!OverCoordinate(moveTable)) continue;

			TableData table = tableManager.GetTableByCoordinate(moveTable);

			//Debug.Log($"{gameObject.name} : {table == null}");

			//스킬 카드 적용 시 대각선 이동 가능
			if (CanMoveSide)
			{
				if(table.IsPiece && !table.piece.IsPlayerPiece)
					table.pieceMoveAppear.DeathPiece = true;

				result.Add(table);
				continue;
			}

			else
			{
				if (table.IsPiece)
				{
					if (!table.piece.IsPlayerPiece)
					{
						table.pieceMoveAppear.DeathPiece = true;
						result.Add(table);
					}
				}
				else
				{
					table.pieceMoveAppear.DeathPiece = false;
				}
			}
		}
		
		//앙파상 경로 확인
		for(int i = -1; i <= 1; i += 2)
		{
			Vector2Int moveTable = new Vector2Int(curTable.Coordinate.x + i, curTable.Coordinate.y);
			if (!OverCoordinate(moveTable)) continue;

			TableData table = tableManager.GetTableByCoordinate(moveTable);

			//Debug.Log($"{gameObject.name} : {table == null}");

			if (table.IsPiece)
			{
				if (!table.piece.IsPlayerPiece)
				{
					if (table.piece.PieceType == PieceType.Pawn && table.piece.gameObject.GetComponent<EnPassantHandler>().isEnPassant)
					{
						Vector2Int pos = new Vector2Int(moveTable.x, moveTable.y - 1);
						TableData t = tableManager.GetTableByCoordinate(pos);
						t.pieceMoveAppear.DeathPiece = true;
						result.Add(t);
					}
				}
			}
		}

		return result;
	}

	public void Promotion(PieceType pieceType)
	{
		PieceData curPieceData = GetComponent<PieceData>();

		if (curPieceData == null)
		{
			Debug.LogWarning("curPieceData가 존재하지 않습니다!");
			return;
		}
	}
}
