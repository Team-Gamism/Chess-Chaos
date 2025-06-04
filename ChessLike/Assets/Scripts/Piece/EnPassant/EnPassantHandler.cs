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
		if(!pawn.IsFirstMove && pieceData.moveCount == 1 && TwoMove)
		{
			isEnPassant = true;
		}
		else
		{
			isEnPassant = false;
		}
	}
}
