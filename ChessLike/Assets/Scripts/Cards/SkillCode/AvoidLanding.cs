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
		cardData = GetComponent<NCard>().cardData;
		TableSelectPanel.cardData = cardData;
		TableSelectPanel.gameObject.SetActive(true);
	}
}
