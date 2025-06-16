using System.Linq;
using UnityEngine;

public class AvoidLanding : MonoBehaviour, ICardSkill
{
	[SerializeField]
	private TableSelector TableSelectPanel;

	[SerializeField]
	private CardData cardData;

	public void Execute()
	{
		GameManager.instance.AvoidLanding = true;
		cardData = GetComponent<NCard>().cardData;
		TableSelectPanel.cardData = cardData;
		TableSelectPanel.gameObject.SetActive(true);
	}

	public void Execute(PieceData none)
	{
		GameManager.instance.AvoidLanding = false;
	}
}
