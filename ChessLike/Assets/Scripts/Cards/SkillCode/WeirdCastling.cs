using System.Collections.Generic;
using System.Linq;
using ChessEngine;
using ChessEngine.Game;
using ChessEngine.Game.AI;
using UnityEngine;

public class WeirdCastling : MonoBehaviour, IPieceSkill
{
	public PieceSelector selector;

	private NCard ncard;
	public PieceSkillType skillType;
	private void Start()
	{
		ncard = GetComponent<NCard>();
	}

	public void LoadSelector(List<ChessPieceType> pieceTypes, bool isAll, ChessColor color)
	{
		selector.gameObject.SetActive(true);
		selector.SetField(cardData: ncard.cardData,
						skillType: skillType,
						pieceTypes: pieceTypes,
						isAll: false,
						color: color);
	}

	public void Execute(List<VisualChessPiece> pieces)
	{
		//킹과 위치 바꾸기
		ChessPiece fromPiece = pieces[0].Piece;
		ChessPiece king = pieces[0].VisualTable.Table.GetKing(!FindObjectOfType<ChessAIGameManager>().IsBlackAIEnabled ? ChessColor.Black : ChessColor.White);

		fromPiece.Tile.MovePieceToPieceTile(king);

		ncard.DOEndAnimation();
	}
}

