using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NCard : MonoBehaviour, IPointerClickHandler
{
	private SkillLoader skillLoader;

	[SerializeField]
	private CardData cardData;

	private void Start()
	{
		GetComponent<Image>().sprite = cardData.cardImage;
		skillLoader = FindObjectOfType<SkillLoader>();
	}

	public void LoadEvent()
	{
		skillLoader.LoadSkill(cardData);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		LoadEvent();
	}
}
