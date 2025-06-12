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

	public void LoadEvent()
	{
		skillLoader.LoadSkill(cardData, skill);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		LoadEvent();
	}
}
