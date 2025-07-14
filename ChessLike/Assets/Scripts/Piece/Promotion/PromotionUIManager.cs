using ChessEngine;
using ChessEngine.Game;
using ChessEngine.Game.AI;
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
			btns[i].image.sprite = AtlasManager.instance.GetCurrentSkinSprite(GameManager.instance.PlayerColor, btns[i].GetComponent<ButtonInfo>().piece);
	}
	public void StartPromotion(VisualChessPiece piece, MoveInfo move)
	{
		canvasGroup.DOFade(1f, 0.5f);
		canvasGroup.blocksRaycasts = true;
		FindObjectOfType<ChessAIGameManager>().isPromotionSelect = true;
		AddButtonEvent(piece, move);
	}
	public void AddButtonEvent(VisualChessPiece piece, MoveInfo move)
	{
		for (int i = 0; i < btns.Length; i++)
		{
			int idx = i;
			btns[i].onClick.RemoveAllListeners();
			btns[i].onClick.AddListener(() =>
			{
				//프로모션 진행하기 & 턴 끝내기
				piece.ChangeOther(idx + 2);
				canvasGroup.blocksRaycasts = false;
				canvasGroup.DOFade(0f, 0.5f);
				FindObjectOfType<ChessAIGameManager>().isPromotionSelect = false;
			});
		}
	}
}
