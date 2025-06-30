using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PawnMoveOnce : MonoBehaviour, ICardSkill
{
	private TableManager tableManager;
	private RectTransform rectTransform;

	private PieceData pieceData;
	private NCard ncard;
	private void Start()
	{
		tableManager = FindObjectOfType<TableManager>();
		rectTransform = GetComponent<RectTransform>();
		ncard = GetComponent<NCard>();
	}


	public void Execute()
	{
		Pawn[] pawns = FindObjectsOfType<Pawn>().Where(p => p.GetComponent<PieceData>().IsPlayerPiece).ToArray();
		for (int i = 0; i < pawns.Length; i++)
		{
			if (!pawns[i].IsFirstMove) pawns[i].IsFirstMove = true;
		}
		GameManager.instance.isPawnMoveOnce = true;
		ncard.DOEndAnimation();
	}

}

