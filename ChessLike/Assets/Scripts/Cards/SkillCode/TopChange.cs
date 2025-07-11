using System.Collections.Generic;
using ChessEngine;
using ChessEngine.Game;
using UnityEngine;

public class TopChange : MonoBehaviour, IPieceSkill
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
		TileIndex t = pieces[0].Piece.TileIndex;

		int reverseX = 7 - t.x;
		int reverseY = 7 - t.y;

		TileIndex newT = new TileIndex
		{
			x = reverseX,
			y = reverseY
		};

		ChessTableTile tile = pieces[0].VisualTable.Table.GetTile(newT);
		
		tile.MovePieceToTileNotCond(pieces[0].Piece, false);

		ncard.DOEndAnimation();
	}

}

