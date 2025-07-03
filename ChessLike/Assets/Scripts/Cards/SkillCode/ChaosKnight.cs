using System.Collections.Generic;
using ChessEngine;
using ChessEngine.Game;
using ChessEngine.Game.AI;
using UnityEngine;

public class ChaosKnight : MonoBehaviour, IPieceSkill
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
        ChessPiece piece = pieces[0].Piece;
        List<ChessTableTile> tiles = piece.Table.GenerateRangeMoveList(piece.TileIndex, 2);

		ChessTableTile tile = tiles[Random.Range(0, tiles.Count - 1)];
		//지정 타일에 pieces[0]을 이동시키기
		tile.MovePieceToTileNotCond(piece, false);
            
		ncard.DOEndAnimation();
	}
}

