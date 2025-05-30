using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NCard : MonoBehaviour, IPointerClickHandler
{
	private SkillLoader skillLoader;

	[SerializeField]
	private CardData cardData;

	[SerializeField]
	private Sprite cardImage;

	private void Start()
	{
		skillLoader = FindObjectOfType<SkillLoader>();
		GetComponent<Image>().sprite = cardImage;
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
