using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NCard : MonoBehaviour, IPointerClickHandler
{
	private SkillLoader skillLoader;

	public CardData cardData;

	private ICardSkill skill;

	private void Start()
	{
		skill = GetComponent<ICardSkill>();
		GetComponent<Image>().sprite = cardData.cardImage;
		skillLoader = FindObjectOfType<SkillLoader>();
	}
    private void OnEnable()
    {
		RectTransform rect = GetComponent<RectTransform>();

		Vector2 vec = new Vector2(rect.anchoredPosition.x, -500);
		GetComponent<RectTransform>().anchoredPosition = vec;
		
		GetComponent<RectTransform>().DOAnchorPosY(0, 0.8f).SetEase(Ease.OutQuint);
    }
    private void OnDisable()
	{
		GetComponentInParent<CardSortManager>().curCard.Remove(this);
		GetComponentInParent<CardSortManager>().CardSort();
	}

    public void LoadEvent()
	{
		skillLoader.LoadSkill(cardData, skill, this.gameObject);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		LoadEvent();
	}
}
