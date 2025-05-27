using UnityEngine;
using DG.Tweening;

public class BotManager : MonoBehaviour
{
	[SerializeField]
	private TableManager tableManager;
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.D))
		{
			MoveByFEN("g8f6");
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

		MovePiece(startTable, endTable);
	}

	private void MovePiece(TableData table, TableData destination)
	{
		PieceData piece = table.piece;

		Vector2 dest = destination.positionToRect(tableManager.PieceCanvas);

		piece.GetComponent<RectTransform>().DOAnchorPos(dest, 0.2f).SetEase(Ease.OutCirc);
		piece.coordinate = destination.Coordinate;
		piece.curTable.IsPiece = false;
		piece.curTable.piece = null;

		piece.UpdateTableCoordinate();
		piece.curTable.IsPiece = true;
		piece.curTable.piece = piece;

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
}
