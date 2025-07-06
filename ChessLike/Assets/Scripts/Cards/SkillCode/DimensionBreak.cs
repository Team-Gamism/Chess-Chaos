using System.Collections.Generic;
using ChessEngine;
using ChessEngine.Game;
using ChessEngine.Game.AI;
using UnityEngine;

public class DimensionBreak : MonoBehaviour, IPieceSkill
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
						color: FindObjectOfType<ChessAIGameManager>().IsBlackAIEnabled ? ChessColor.Black : ChessColor.White);
	}

public void Execute(List<VisualChessPiece> pieces)
{
	for (int i = 0; i < pieces.Count; i++)
	{
		string Rand = "";
		VisualChessTableTile targetTile = null;

		do
		{
			Rand = "";
			Rand += (char)Random.Range(97, 105); // a~h
			Rand += Random.Range(1, 9); // 1~8

			targetTile = pieces[i].VisualTable.GetVisualTileByID(Rand);
		}
		while (
			targetTile == null || // 존재하지 않거나
			targetTile.isTileblock || // 막혀있거나
			targetTile.GetVisualPiece() != null // 이미 기물이 있는 경우
		);

		targetTile.Tile.MovePieceToTileNotCond(pieces[i].Piece, false);
	}

	ncard.DOEndAnimation();
}
}

