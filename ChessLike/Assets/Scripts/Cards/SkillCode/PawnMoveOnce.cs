using System.Collections.Generic;
using ChessEngine.Game;
using DG.Tweening;
using UnityEngine;

public class PawnMoveOnce : MonoBehaviour, IPieceSkill
{
	public PieceSelector selector;

	private NCard ncard;
	public PieceSkillType skillType;
	private void Start()
	{
		ncard = GetComponent<NCard>();
	}

    public void LoadSelector(List<ChessEngine.ChessPieceType> pieceTypes, bool isAll, ChessEngine.ChessColor color)
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
		for (int i = 0; i < pieces.Count; i++)
		{
			pieces[i].isTwoMove = true;
			pieces[i].UpdateTwoMove();
		}
		
        ncard.DOEndAnimation();
    }
}

