using System.Linq;
using UnityEngine;

public class DarknessHand : MonoBehaviour, ICardSkill
{
	private TableManager tableManager;
	private RectTransform rectTransform;

	private PieceSelector selector;

	private PieceData pieceData;

	private void Start()
	{
		tableManager = FindObjectOfType<TableManager>();
		rectTransform = GetComponent<RectTransform>();

		selector = FindObjectOfType<PieceSelector>();
	}

	public void Execute()
	{
		GameManager.instance.DarknessHand = true;
		selector.EnableImage(false);
	}

	public void SetpieceData(PieceData pieceData)
	{
		this.pieceData = pieceData;
	}


	public void Execute(PieceData piece)
	{
		pieceData.IsStatic = true;
		pieceData.StaticedTurn = 1;

		FindObjectOfType<PieceSelector>().DisableImage();
		gameObject.SetActive(false);
	}
}
