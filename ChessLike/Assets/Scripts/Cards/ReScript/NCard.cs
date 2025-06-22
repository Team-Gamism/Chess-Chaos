using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NCard : MonoBehaviour, IPointerClickHandler
{
	private SkillLoader skillLoader;

	public CardData cardData;

	private ICardSkill skill;
	private bool isFirst = false;

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

		float Delay = !isFirst ? 2.5f : 0.3f;
		GetComponent<RectTransform>().DOAnchorPosY(0, 0.7f).SetEase(Ease.OutQuint).SetDelay(Delay).OnComplete(() => { isFirst = true; });
	}
	private void CloseCard()
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
	public void DOEndAnimation()
	{
		var rect = gameObject.GetComponent<RectTransform>();

		DOTween.Sequence()
			.Append(rect.DOAnchorPosY(-500, 0.5f).SetEase(Ease.OutQuad))
			.Join(DOTween.Sequence()
				.AppendInterval(0.2f)
				.AppendCallback(() =>
				{
					gameObject.SetActive(false);
					CloseCard();
				})
			);
	}
}
