using DG.Tweening;
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

		for (int i = 0; i < btns.Length; i++)
			btns[i].image.sprite = AtlasManager.instance.GetCurrentSkinSprite(!GameManager.instance.PlayerColor, btns[i].GetComponent<ButtonInfo>().piece);
	}

	public void AddButtonEvent(PawnPromotion pawnPromotion)
	{
		//애니메이션 실행

		//프로모션 진행 상태 활성화
		GameManager.instance.IsPromotion = true;

		canvasGroup.alpha = 1.0f;
		rectTransform.anchoredPosition = new Vector3(0f, 8f, 0f);
		GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 4.7f), 0.5f).SetEase(Ease.OutCirc);

		for (int i = 0; i < btns.Length; i++)
		{
			int idx = i;
			btns[i].onClick.AddListener(() => pawnPromotion.Promotion(idx));
		}
	}

	public void ClosePanel()
	{
		canvasGroup.alpha = 1.0f;
		GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 8f), 0.5f).SetEase(Ease.OutCirc).OnComplete(() => { canvasGroup.alpha = 0f; });
	}
}
