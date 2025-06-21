using System.Linq;
using UnityEngine;

public class PawnAimming : MonoBehaviour, ICardSkill
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
		GameManager.instance.isPawnAimming = true;
		gameObject.SetActive(false);
	}
}
