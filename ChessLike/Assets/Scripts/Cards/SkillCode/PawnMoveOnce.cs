using UnityEngine;

public class PawnMoveOnce : MonoBehaviour, ICardSkill
{
    public CardData cardData;

	private TableManager tableManager;
	private RectTransform rectTransform;

	private PieceData pieceData;
	private void Start()
	{
		tableManager = FindObjectOfType<TableManager>();
		rectTransform = GetComponent<RectTransform>();
	}

/*	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			Execute();
		}
	}
*/
	public void Execute()
	{
		
	}

}

public interface ICardSkill
{
    public void Execute();
}
