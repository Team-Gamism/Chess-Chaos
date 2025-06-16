using System.Linq;
using UnityEngine;

public class TripleBarrier : MonoBehaviour, ICardSkill
{
	[SerializeField]
	private TableSelector TableSelectPanel;

	[SerializeField]
	private CardData cardData;

	public void Execute()
	{
		GameManager.instance.TripleBarrier = true;
		cardData = GetComponent<NCard>().cardData;
		TableSelectPanel.cardData = cardData;
        TableSelectPanel.NotMoveCount = 2;
		TableSelectPanel.gameObject.SetActive(true);
	}

	public void Execute(PieceData none)
	{
		GameManager.instance.TripleBarrier = false;
	}
}
