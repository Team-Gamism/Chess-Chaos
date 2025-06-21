using System.Linq;
using UnityEngine;

public class SnakePawn : MonoBehaviour, ICardSkill
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
		GameManager.instance.isSnakePawn = true;
		selector.EnableImage(PieceType.Pawn);
	}

	public void SetpieceData(PieceData pieceData)
	{
		this.pieceData = pieceData;
	}


	public void Execute(PieceData piece)
	{
		pieceData.RegisterPieceType(PieceType.Knight);
		pieceData.IsSnakePawn = true;

		pieceData.ChangePieceType(PieceType.Knight);

		FindObjectOfType<PieceSelector>().DisableImage();
		gameObject.SetActive(false);
	}
}
