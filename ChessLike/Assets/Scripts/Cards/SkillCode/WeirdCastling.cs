using System.Linq;
using UnityEngine;

public class WeirdCastling : MonoBehaviour, ICardSkill
{
	private TableManager tableManager;
	private RectTransform rectTransform;

	private PieceSelector selector;

	private PieceData pieceData;

	public PieceType[] EnablePieces;
	private void Start()
	{
		tableManager = FindObjectOfType<TableManager>();
		rectTransform = GetComponent<RectTransform>();

		selector = FindObjectOfType<PieceSelector>();
	}

	public void Execute()
	{
		GameManager.instance.WeirdCasting = true;
		selector.EnableImage(EnablePieces);
	}
}
