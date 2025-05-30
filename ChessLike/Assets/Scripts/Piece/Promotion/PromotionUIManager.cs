using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromotionUIManager : MonoBehaviour
{
	public Button[] btns;

	private CanvasGroup canvasGroup;

	private RectTransform rectTransform;

	private void Start()
	{
		canvasGroup = GetComponent<CanvasGroup>();

		rectTransform = GetComponent<RectTransform>();
	}

	public void AddButtonEvent(PawnPromotion pawnPromotion)
	{
		//애니메이션 실행

		canvasGroup.alpha = 1.0f;
		rectTransform.anchoredPosition = new Vector3(0f, 8f, 0f);
		GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 4.7f), 0.5f).SetEase(Ease.OutCirc);

		for (int i = 0; i < btns.Length; i++)
		{
			int idx = i;
			btns[i].onClick.AddListener(() =>pawnPromotion.Promotion(idx));
		}
	}

	public void ClosePanel()
	{
		canvasGroup.alpha = 1.0f;
		GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 8f), 0.5f).SetEase(Ease.OutCirc).OnComplete(() => { canvasGroup.alpha = 0f; });
	}
}
