using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardEffector : MonoBehaviour
{
	[SerializeField] private Image background;
	[SerializeField] private Image card;
	[SerializeField] private TMP_Text title;
	[SerializeField] private TMP_Text description;

	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private RectTransform rect;


	public void CardAppear(CardData data)
	{
		background.sprite = data.cardImage;
		card.sprite = data.cardImage;
		title.text = data.Title;
		description.text = data.Description;
		DOAnim();
	}

	private void DOAnim()
	{
		Vector3 endValue = new Vector3(0f, 0f, 0f);

		Sequence seq = DOTween.Sequence();

		seq.Append(rect.DORotate(endValue, 0.5f, RotateMode.Fast).SetEase(Ease.OutQuint));
		seq.Append(canvasGroup.DOFade(0f, 0.5f).OnComplete(() => { Destroy(gameObject); }).SetDelay(0.2f));

		seq.Play();
	}
}
