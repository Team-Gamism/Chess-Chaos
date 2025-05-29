using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class NCard : MonoBehaviour, IPointerClickHandler
{
	private SkillLoader skillLoader;

	[SerializeField]
	private CardData cardData;

	private void Start()
	{
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
