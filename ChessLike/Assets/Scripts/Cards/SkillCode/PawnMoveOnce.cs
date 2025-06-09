using System.Linq;
using UnityEngine;

public class PawnMoveOnce : MonoBehaviour, ICardSkill
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
		Debug.Log("Skill Start");
		Pawn[] pawns = FindObjectsOfType<Pawn>().Where(p => p.GetComponent<PieceData>().IsPlayerPiece).ToArray();
		for(int i = 0; i < pawns.Length; i++)
		{
			if (!pawns[i].IsFirstMove) pawns[i].IsFirstMove = true;
		}
	}

}

public interface ICardSkill
{
    public void Execute();
}
