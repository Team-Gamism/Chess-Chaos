using DG.Tweening;
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

	[SerializeField]
	private ICardSkill skill;

	public void LoadSkill(CardData cardData, ICardSkill skill)
	{
		CanvasGroup skillDescription = GetComponent<CanvasGroup>();
		skillDescription.interactable = true;
		skillDescription.blocksRaycasts = true;
		DOTween.To(() => skillDescription.alpha, x => skillDescription.alpha = x, 1f, 0.2f);
		isOwn.isOn = cardData.isOwn;	
		title.text = cardData.Title;
		description.text = cardData.Description;
		this.skill = skill;
	}

	public void CloseSkill()
	{
		CanvasGroup skillDescription = GetComponent<CanvasGroup>();
		skillDescription.interactable = false;
		skillDescription.blocksRaycasts = false;
		DOTween.To(() => skillDescription.alpha, x => skillDescription.alpha = x, 0f, 0.2f);
	}

	public void ExecuteSkill()
	{
		if (GameManager.instance.isSnakePawn)
		{
			skill.Execute(new PieceData());
		}
		else
		{
			skill.Execute();
		}
	}
}
