using System.Collections.Generic;
using ChessEngine;
using ChessEngine.Game;
using ChessEngine.Game.AI;
using UnityEngine;
using UnityEngine.UIElements;

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
			do
			{
				Rand = "";
				Rand += (char)Random.Range(97, 105);
				Rand += Random.Range(1, 9);
			} while (pieces[i].VisualTable.GetVisualTileByID(Rand) == null &&
				pieces[i].VisualTable.GetVisualTileByID(Rand).isTileblock &&
				pieces[i].VisualTable.GetVisualTileByID(Rand).GetVisualPiece() != null
				);
			pieces[i].VisualTable.GetVisualTileByID(Rand).Tile.MovePieceToTileNotCond(pieces[i].Piece, false);
		}


		ncard.DOEndAnimation();
	}
}

