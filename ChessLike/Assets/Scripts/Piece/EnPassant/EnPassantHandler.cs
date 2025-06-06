using UnityEngine;

public class EnPassantHandler : MonoBehaviour
{
	public bool isEnPassant = false;

	public bool TwoMove = false;
	private PieceData pieceData;
	private Pawn pawn;

	private void Start()
	{
		pieceData = GetComponent<PieceData>();
		pawn = GetComponent<Pawn>();
	}

	private void Update()
	{
		if (!pawn.IsFirstMove && pieceData.moveCount == 1 && TwoMove)
		{
			isEnPassant = CheckOtherPiece();
		}
		else
		{
			isEnPassant = false;
		}
	}
	private bool CheckOtherPiece()
	{
		Vector2Int coord = new Vector2Int(pieceData.coordinate.x, pieceData.coordinate.y + (pieceData.IsPlayerPiece ? 1 : -1));
		TableData t = pieceData.GetTableManager().GetTableByCoordinate(coord);

		if (t.IsPiece)
		{
			//플레이어 기물인지 확인
			if (pieceData.IsPlayerPiece)
			{
				if (t.piece.IsPlayerPiece)
				{
					return false;
				}
			}
			else
			{
				if (!t.piece.IsPlayerPiece)
				{
					return false;
				}
			}
		}
		return true;
	}
}
