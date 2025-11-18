using ChessEngine.Game;
using UnityEngine;

public class PawnController : MonoBehaviour
{
	[SerializeField] private VisualChessPiece piece;

	public void UpdatePawnState()
	{
		if(piece.GetSnakePawn()) piece.SetSnakePawn(false);
		if(piece.GetMoveSide()) piece.SetMoveSide(false);
		if(piece.GetTwoMove()) piece.SetTwoMove(false);
	}
}
