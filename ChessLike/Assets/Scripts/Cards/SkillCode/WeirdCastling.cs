using System.Linq;
using UnityEngine;

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
		pieceData = selector.ReturnPiece();

		PieceData n = pieceData;

		pieceData.GetComponent<PieceHandler>().MovePieceByCoordinate_AnimationDone(King.coordinate);
		King.GetComponent<PieceHandler>().MovePieceByCoordinate_AnimationDone(n.coordinate);

		GameManager.instance.WeirdCasting = false;

		GameManager.instance.SortPieceSibling();
	}

}
