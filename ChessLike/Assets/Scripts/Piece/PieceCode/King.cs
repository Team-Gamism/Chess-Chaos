using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class King : PieceAbstract
{
	public bool isFirstMove = true;
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

		if (isFirstMove)
		{
			//킹 캐슬링 여부 확인
			int castlingAble = IsCastlingAble();

			Debug.Log(castlingAble);

			Vector2Int curCoord = GetComponent<PieceData>().coordinate;

			switch (castlingAble)
			{
				case 0:
					break;
				case 1:
					Vector2Int newCoord = new Vector2Int(curCoord.x - 2, curCoord.y);
					result.Add(tableManager.GetTableByCoordinate(newCoord));
					break;
				case 2:
					Vector2Int newCoord2 = new Vector2Int(curCoord.x + 2, curCoord.y);
					result.Add(tableManager.GetTableByCoordinate(newCoord2));
					break;
				case 3:
					for (int i = -2; i < 2; i += 2)
					{
						Vector2Int newCoord3 = new Vector2Int(curCoord.x + i, curCoord.y);
						result.Add(tableManager.GetTableByCoordinate(newCoord3));
					}
					break;
			}
		}

		//최종 결과 반환
		return result;
	}

	/// <summary>
	/// 0은 캐슬링 불가, 1은 퀸사이드 캐슬링, 2는 킹사이드 캐슬링, 3은 둘다 가능
	/// </summary>
	/// <returns></returns>
	public int IsCastlingAble()
	{
		int n = 0;

		if (!GameManager.instance.IsCheckmate)
		{
			Rook[] rooks;

			rooks = FindObjectsOfType<Rook>().Where(r => r.GetComponent<PieceData>().IsPlayerPiece && r.isFirstMove).ToArray();

			if (rooks.Length == 0) return 0;

			//왕 기물이 첫 움직임인지 확인
			if (isFirstMove)
			{
				//룩과 킹 사이에 기물이 존재하는지 확인
				if (IsLeftCastle())
					n |= (1 << 0);
				//룩과 킹 사이에 기물이 존재하는지 확인
				if (IsRightCastle())
					n |= (1 << 1);

				return n;
			}
			else return 0;
		}
		else return 0;
	}
	private bool IsQueenSideCastleRoute()
	{
		//판에 있는 상대 기물을 모두 조회해 캐슬링 경로에 이동 가능한지 확인
		List<PieceHandler> pieceHandlers = new List<PieceHandler>();

		pieceHandlers = FindObjectsOfType<PieceHandler>().Where(p => !p.gameObject.GetComponent<PieceData>().IsPlayerPiece).ToList();

		Debug.Log($"[퀸사이드 캐슬링]찾은 기물 수 : {pieceHandlers.Count}");

		foreach(PieceHandler handler in pieceHandlers)
		{
			Debug.Log($"[퀸사이드 캐슬링]가능한 경로 수 : {handler.tableList.Count}");
			foreach(TableData table in handler.tableList)
			{
				if (table.Coordinate.y == 7 && table.Coordinate.x < 4) return false;
			}
		}
		return true;
	}

	private bool IsKingSideCastleRoute()
	{
		//판에 있는 상대 기물을 모두 조회해 캐슬링 경로에 이동 가능한지 확인
		List<PieceHandler> pieceHandlers = new List<PieceHandler>();

		pieceHandlers = FindObjectsOfType<PieceHandler>().Where(p => !p.gameObject.GetComponent<PieceData>().IsPlayerPiece).ToList();

		Debug.Log($"[킹사이드 캐슬링]찾은 기물 수 : {pieceHandlers.Count}");

		foreach (PieceHandler handler in pieceHandlers)
		{
			Debug.Log($"[킹사이드 캐슬링]가능한 경로 수 : {handler.tableList.Count}");
			foreach (TableData table in handler.tableList)
			{
				if (table.Coordinate.y == 7 && table.Coordinate.x >= 4) return false;
			}
		}
		return true;
	}
	private bool IsLeftCastle()
	{
		List<PieceData> pieceDatas = new List<PieceData>();

		pieceDatas = FindObjectsOfType<PieceData>().Where(p => (p.IsPlayerPiece && p.coordinate.y == 7) && p.coordinate.x < 4 && p.PieceType != PieceType.King).ToList();

		Debug.Log($"[퀸사이드 캐슬링]찾은 기물 수 : {pieceDatas.Count}");

		if (pieceDatas.Count <= 1) return true;
		else return false;
	}

	private bool IsRightCastle()
	{
		List<PieceData> pieceDatas = new List<PieceData>();

		pieceDatas = FindObjectsOfType<PieceData>().Where(p => (p.IsPlayerPiece && p.coordinate.y == 7) && p.coordinate.x >= 4 && p.PieceType != PieceType.King).ToList();

		Debug.Log($"[킹사이드 캐슬링]찾은 기물 수 : {pieceDatas.Count}");

		if (pieceDatas.Count <= 1) return true;
		else return false;
	}
}