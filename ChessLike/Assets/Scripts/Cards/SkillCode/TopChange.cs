using System.Linq;
using UnityEngine;

public class TopChange : MonoBehaviour, ICardSkill
{
	private TableManager tableManager;
	private RectTransform rectTransform;

	private PieceSelector selector;

	private PieceHandler handler;
	private void Start()
	{
		tableManager = FindObjectOfType<TableManager>();
		rectTransform = GetComponent<RectTransform>();

		selector = FindObjectOfType<PieceSelector>();
	}

	public void Execute()
	{
		GameManager.instance.TopChange = true;


		selector.EnableImageAndCheckCoord(PieceType.Rook);
	}

	public void SetHandler(PieceHandler handler)
	{
		this.handler = handler;
	}


	public void Execute(PieceData piece)
	{
		Vector2Int coord = handler.GetComponent<PieceData>().coordinate;
		Vector2Int newCoord = new Vector2Int(7 - coord.x, 7 - coord.y);

		handler.MovePieceByCoordinate(newCoord);

		FindObjectOfType<PieceSelector>().DisableImage();
	}

	private Vector2Int reverseCoordinate(Vector2Int coord)
	{
		return new Vector2Int(7 - coord.x, 7 - coord.y);
	}
}
