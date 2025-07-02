using System.Collections.Generic;
using ChessEngine;
using ChessEngine.Game;
using ChessEngine.Game.AI;
using UnityEngine;

public class DarknessHand : MonoBehaviour, IPieceSkill
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
			pieces[i].SetPin(true, 2);
			pieces[i].UpdatePin();
		}
		
        ncard.DOEndAnimation();
    }
}

