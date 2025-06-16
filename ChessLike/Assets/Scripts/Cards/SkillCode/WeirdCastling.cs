using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WeirdCastling : MonoBehaviour, ICardSkill
{
	private TableManager tableManager;
	private RectTransform rectTransform;
	[SerializeField]
	private MultiPieceSelector selector;

	public PieceData pieceData;
	private PieceData King;

	public PieceType[] EnablePieces;

	private CardData cardData;
	private void Start()
	{
		tableManager = FindObjectOfType<TableManager>();
		rectTransform = GetComponent<RectTransform>();
	}

	public void SetpieceData(PieceData pieceData)
	{
		this.pieceData = pieceData;
		selector.QueueManage(pieceData);
	}

	public void Execute()
	{
		King = FindObjectsOfType<PieceData>().Where(p => p.IsPlayerPiece && p.PieceType == PieceType.King).FirstOrDefault();

		GameManager.instance.WeirdCasting = true;
		cardData = GetComponent<NCard>().cardData;
		selector.cardData = cardData;
		selector.gameObject.SetActive(true);

		selector.EnableImage(EnablePieces);
	}

	public void Execute(PieceData none)
	{
		GameManager.instance.WeirdCasting = false;
	}

}
