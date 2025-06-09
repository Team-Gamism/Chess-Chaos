using System.Linq;
using UnityEngine;

public class SnakePawn : MonoBehaviour, ICardSkill
{
	private TableManager tableManager;
	private RectTransform rectTransform;

	private PieceData pieceData;
	private void Start()
	{
		tableManager = FindObjectOfType<TableManager>();
		rectTransform = GetComponent<RectTransform>();
	}

	public void Execute()
	{
		SelectObj();
	}

	public void SelectObj()
	{


		//최종적으로 선택한 기물로 스킬 실행
		Execute(pieceData);
	}

	public void Execute(PieceData piece)
	{
		piece.GetComponent<Pawn>().enabled = false;
		piece.GetComponent<Knight>().enabled = true;

		GameManager.instance.isSnakePawn = true;
	}
}
