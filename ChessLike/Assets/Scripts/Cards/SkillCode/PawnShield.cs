using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PawnShield : MonoBehaviour, ICardSkill
{
	private TableManager tableManager;
	private RectTransform rectTransform;

	private PieceSelector selector;

	private PieceData pieceData;
	private NCard ncard;
	private void Start()
	{
		tableManager = FindObjectOfType<TableManager>();
		rectTransform = GetComponent<RectTransform>();

		selector = FindObjectOfType<PieceSelector>();
		ncard = GetComponent<NCard>();
	}

	public void Execute()
	{
		GameManager.instance.isPawnShield = true;
		selector.EnableImage(PieceType.Pawn);
	}

	public void SetpieceData(PieceData pieceData)
	{
		this.pieceData = pieceData;
	}


	public void Execute(PieceData piece)
	{
		pieceData.IsShield = true;

		FindObjectOfType<PieceSelector>().DisableImage();
		ncard.DOEndAnimation();
	}
}
