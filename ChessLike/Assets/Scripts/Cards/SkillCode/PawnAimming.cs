using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PawnAimming : MonoBehaviour, ICardSkill
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
		GameManager.instance.isPawnAimming = true;
		ncard.DOEndAnimation();
	}
}
