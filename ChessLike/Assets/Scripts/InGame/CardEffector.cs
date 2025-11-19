using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class CardEffector : MonoBehaviour
{
	[Header("Images")]
	[SerializeField] private Image background;
	[SerializeField] private Image card;

	[Header("Texts")]
	[SerializeField] private TMP_Text title;
	[SerializeField] private TMP_Text description;

	[Header("Components")]
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private RectTransform rect;

	[SerializeField] private RenderObjects BWRender;

	public void CardAppear(CardData data, SpawnType type)
	{
		RectTransform cardRect = card.GetComponent<RectTransform>();
		cardRect.rotation = Quaternion.identity;
		cardRect.DORotate(new Vector3(-20f, 380f, 0f), 0.7f, RotateMode.FastBeyond360).SetEase(Ease.OutQuint);

		background.sprite = data.cardImage;
		card.sprite = data.cardImage;
		title.text = data.Title;
		description.text = data.Description;

		if (type == SpawnType.UseCard)
			DOUseAnim();
		else
			DOBreakAnim();
	}

	[ContextMenu("Do Animation")]
	private void DOUseAnim()
	{
		Vector3 endValue = new Vector3(0f, 0f, 0f);
		Sequence seq = DOTween.Sequence();

		seq.Append(rect.DORotate(endValue, 0.5f, RotateMode.Fast).SetEase(Ease.OutQuint));
		seq.Append(canvasGroup.DOFade(0f, 0.5f)
			.OnComplete(() =>
			{
				rect.rotation = Quaternion.Euler(0f, 0f, -30f);
				canvasGroup.alpha = 1f;
			})
			.SetDelay(0.2f));

		seq.Play();
	}

	[ContextMenu("DOBreakAnim")]
	private void DOBreakAnim()
	{
		Vector3 endValue = new Vector3(0f, 0f, 0f);
		Sequence seq = DOTween.Sequence();

		seq.Append(rect.DORotate(endValue, 0.5f, RotateMode.Fast)
			.SetEase(Ease.OutQuint));

		// 수정: Append와 OnStart의 위치 조정
		seq.Append(rect.DOShakeAnchorPos(0.3f, new Vector2(0f, 100f), 50)
			.OnStart(() =>
			{
				BWRender.SetActive(true);
			}));

		seq.Append(canvasGroup.DOFade(0f, 0.5f)
			.OnComplete(() =>
			{
				rect.rotation = Quaternion.Euler(0f, 0f, -30f);
				canvasGroup.alpha = 1f;
				BWRender.SetActive(false);
			})
			.SetDelay(0.2f));

		seq.Play();
	}
}