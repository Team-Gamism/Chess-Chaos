using UnityEngine;
using DG.Tweening;
using System.Diagnostics;

public class BotManager : MonoBehaviour
{
	[SerializeField]
	private TableManager tableManager;

	public bool CompleteDone = false;

	public bool IsCheckmate = false;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.D))
		{
			MoveByFEN("e7e5");
		}
	}
	public void MoveByFEN(string FEN)
	{
		string start = FEN.Substring(0, 2);
		string end = FEN.Substring(2, 2);

		Vector2Int startCoor = FENToCoordinate(start);
		Vector2Int endCoor = FENToCoordinate(end);

		TableData startTable = tableManager.GetTableByCoordinate(startCoor);
		TableData endTable = tableManager.GetTableByCoordinate(endCoor);

		//특수 조건(방어막 여부, 잡기 불가능) 발동 여부 확인
		//만약 조건 만족 시, BestMove 재계산

		CompleteDone = true;
		MovePiece(startTable, endTable);

	}

	private void MovePiece(TableData table, TableData destination)
	{
		PieceData piece = table.piece;

		Vector2 dest = destination.positionToRect(tableManager.PieceCanvas);

		Vector2Int startCoord = table.Coordinate;
		Vector2Int endCoord = destination.Coordinate;

		if(piece.PieceType == PieceType.Pawn)
		{
			EnPassantHandler enPassantHandler = piece.GetComponent<EnPassantHandler>();
			Pawn pawn = piece.GetComponent<Pawn>();

			if (pawn.IsFirstMove) pawn.IsFirstMove = false;

			if (Mathf.Abs(startCoord.y - endCoord.y) == 2)
			{
				UnityEngine.Debug.Log("앙파상 가능");
				enPassantHandler.TwoMove = true;
				enPassantHandler.isEnPassant = true;
			}
			else
			{
				enPassantHandler.isEnPassant = false;
			}
		}

		piece.GetComponent<RectTransform>().DOAnchorPos(dest, 0.2f).SetEase(Ease.OutCirc);
		piece.coordinate = destination.Coordinate;
		piece.curTable.IsPiece = false;
		piece.curTable.piece = null;

		piece.UpdateTableCoordinate();
		piece.curTable.IsPiece = true;
		piece.curTable.piece = piece;

		piece.moveCount++;

		GameManager.instance.SortPieceSibling();
	}

	private Vector2Int FENToCoordinate(string FEN)
	{
		Vector2Int result = new Vector2Int();
		result.x = FEN[0] - 'a';

		int n = FEN[1] - '0';
		result.y = Mathf.Clamp(Mathf.Abs(n - 8), 0, 7);

		return result;
	}

	private string[] BestMove()
	{
		//최고의 수를 반환하는 함수
		string[] moves = { "", "" };
		return moves;
	}

	private void BotPromotion()
	{

	}
}
