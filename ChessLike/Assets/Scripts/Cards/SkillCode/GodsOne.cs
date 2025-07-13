using System.Collections.Generic;
using ChessEngine;
using ChessEngine.Game;
using ChessEngine.Game.AI;
using UnityEngine;

public class GodsOne : MonoBehaviour, IPieceSkill
{
	public PieceSelector selector;

	private NCard ncard;
	public PieceSkillType skillType;
	public AudioClip SFX;
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
		SoundManager.Instance.SFXPlay("sfx", SFX);
		for (int i = 0; i < pieces.Count; i++)
		{
			pieces[i].ChangeQueen();
		}
		
        ncard.DOEndAnimation();
    }
}

