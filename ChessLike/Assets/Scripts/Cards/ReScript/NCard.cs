using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class NCard : MonoBehaviour
{
	private SkillLoader skillLoader;

	public CardData cardData;

	private ICardSkill skill;

	public AudioClip CardSpawn;
	public AudioClip CardDraw;

	private void Start()
	{
		skill = GetComponent<ICardSkill>();
		GetComponent<Image>().sprite = cardData.cardImage;
		skillLoader = FindObjectOfType<SkillLoader>();
	}
	private void OnEnable()
	{
		SoundManager.Instance.SFXPlay("CardSpawn", CardSpawn);
		RectTransform rect = GetComponent<RectTransform>();

		Vector2 vec = new Vector2(rect.anchoredPosition.x, -500);
		GetComponent<RectTransform>().anchoredPosition = vec;

		float Delay = 0.4f;
		GetComponent<RectTransform>().DOAnchorPosY(0, 0.7f).SetEase(Ease.OutBack).SetDelay(Delay);
	}
	private void CloseCard()
	{
		GetComponentInParent<CardSortManager>().curCard.Remove(this);
		GetComponentInParent<CardSortManager>().CardSort();
	}

	public void LoadEvent()
	{
		SoundManager.Instance.SFXPlay("CardDraw", CardDraw);
		skillLoader.LoadSkill(gameObject);
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
