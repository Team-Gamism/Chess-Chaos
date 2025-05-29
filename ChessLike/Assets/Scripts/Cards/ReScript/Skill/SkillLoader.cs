using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillLoader : MonoBehaviour
{
	[SerializeField]
	private TMP_Text title;
	[SerializeField]
	private Toggle isOwn;
	[SerializeField]
	private TMP_Text description;

	public void LoadSkill(CardData cardData)
	{
		CanvasGroup skillDescription = GetComponent<CanvasGroup>();
		skillDescription.interactable = true;
		DOTween.To(() => skillDescription.alpha, x => skillDescription.alpha = x, 1f, 0.2f);
		isOwn.isOn = cardData.isOwn;	
		title.text = cardData.Title;
		description.text = cardData.Description;
	}

	public void CloseSkill()
	{
		CanvasGroup skillDescription = GetComponent<CanvasGroup>();
		skillDescription.interactable = false;
		DOTween.To(() => skillDescription.alpha, x => skillDescription.alpha = x, 0f, 0.2f);
	}
}
