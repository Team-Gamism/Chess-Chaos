using System.Collections.Generic;
using ChessEngine;
using ChessEngine.Game;
using DG.Tweening;
using UnityEngine;

public class PawnShield : MonoBehaviour, IPieceSkill
{
	[SerializeField]
	private PieceSelector selector;

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
		for (int i = 0; i < pieces.Count; i++)
		{
			pieces[i].SetShield(true);
		}
		
		ncard.DOEndAnimation();
    }
}
