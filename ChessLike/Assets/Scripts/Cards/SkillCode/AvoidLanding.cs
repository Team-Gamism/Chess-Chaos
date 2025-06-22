using DG.Tweening;
using UnityEngine;

public class AvoidLanding : MonoBehaviour, ICardSkill
{
	[SerializeField]
	private TableSelector TableSelectPanel;

	[SerializeField]
	private CardData cardData;

	private NCard ncard;
    private void Start()
    {
		ncard = GetComponent<NCard>();
    }

    public void Execute()
	{
		GameManager.instance.AvoidLanding = true;
		cardData = GetComponent<NCard>().cardData;
		TableSelectPanel.cardData = cardData;
		TableSelectPanel.NotMoveCount = 1;
		TableSelectPanel.gameObject.SetActive(true);
	}

	public void Execute(PieceData none)
	{
		GameManager.instance.AvoidLanding = false;
		ncard.DOEndAnimation();
	}
}
