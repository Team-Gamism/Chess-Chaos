using System.Collections.Generic;
using System.Linq;
using ChessEngine;
using ChessEngine.Game;
using ChessEngine.Game.AI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillLoader : MonoBehaviour
{
	[SerializeField]
	private TMP_Text title;
	[SerializeField]
	private Image cardImage;
	[SerializeField]
	private TMP_Text description;
	[SerializeField]
	private TMP_Text tier;
	[SerializeField]
	private GameObject rect;
	[SerializeField]
	private TMP_Text subText;
	[SerializeField]
	private Image tierColorImage;
    public WarningLog warningLog;
	[Header("Sound")]
	public AudioClip CardClose;
	public AudioClip UseCard;

	private List<PieceInfo> images = new List<PieceInfo>();

	private SkillType currentSkillType;
	private ISkill immeSkill; //즉시 실행 스킬
	private IPieceSkill pieceSkill; // 기물 관련 스킬
	private ITableSkill tableSkill; // 테이블 관련 스킬
	private CardData cardData;

	private GameObject selectedObj;

	private Color32[] colors = {
		new Color32(99, 155, 255, 255),
		new Color32(106,  190, 48, 255),
		new Color32(223,  113, 38, 255),
		new Color32(118, 66, 138, 255),
		new Color32(251, 242, 54, 255)
	};

	private void Start()
	{
		images = rect.GetComponentsInChildren<PieceInfo>().ToList();
    }


	public void LoadSkill(GameObject obj)
	{
		selectedObj = obj;
		selectedObj.GetComponent<RectTransform>().DOAnchorPosY(100, 0.5f).SetEase(Ease.OutQuint);


		InitSkillType();

		FindObjectOfType<ChessGameManager>().isCardInfo = true;

		NCard n = obj.GetComponent<NCard>();
		CardData data = n.cardData;
		cardData = data;
		CanvasGroup skillDescription = GetComponent<CanvasGroup>();

		skillDescription.interactable = true;
		skillDescription.blocksRaycasts = true;

		DOTween.To(() => skillDescription.alpha, x => skillDescription.alpha = x, 1f, 0.2f);

		cardImage.sprite = data.cardImage;
		title.text = data.Title;
		description.text = data.Description;
		tier.text = CardTierToKorean(data.cardTier);
		tierColorImage.color = colors[(int)data.cardTier];

		ChessAIGameManager ai = FindObjectOfType<ChessAIGameManager>();

		bool playerColor = ai.enableBlackAI ? true : false;

		for (int i = 0; i < images.Count; i++)
		{

			images[i].GetComponent<Image>().sprite = AtlasManager.instance.GetCurrentSkinSprite(!data.EnemyTarget, images[i].GetComponent<PieceInfo>().pieceType);
			if (data.pieces.Contains(images[i].piece))
				images[i].gameObject.SetActive(true);

			else
				images[i].gameObject.SetActive(false);
		}

		if (data.pieces.Count <= 0)
		{
			if (data.skillType == SkillType.Imme) subText.text = "즉시 실행 기믹";
			else subText.text = "칸 선택 기믹";
		}
		else
		{
			subText.text = "적용 가능 기물";
		}

		currentSkillType = data.skillType;

		switch ((int)data.skillType)
		{
			case 0:
				immeSkill = n.GetComponent<ISkill>();
				break;
			case 1:
				pieceSkill = n.GetComponent<IPieceSkill>();
				break;
			case 2:
				tableSkill = n.GetComponent<ITableSkill>();
				break;
		}
	}

	public void CloseSkill()
	{
		FindObjectOfType<ChessGameManager>().isCardInfo = false;

		SoundManager.Instance.SFXPlay("CardClose", CardClose);

		selectedObj.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f).SetEase(Ease.OutQuint);

		CanvasGroup skillDescription = GetComponent<CanvasGroup>();
		skillDescription.interactable = false;
		skillDescription.blocksRaycasts = false;
		DOTween.To(() => skillDescription.alpha, x => skillDescription.alpha = x, 0f, 0.2f);
	}

	private void InitSkillType()
	{
		immeSkill = null;
		pieceSkill = null;
		tableSkill = null;

		cardData = null;
	}

	//버튼 이벤트 전용
	public void Execute()
	{
		SoundManager.Instance.SFXPlay("useCard", UseCard);
		if ((int)currentSkillType == 0)
			immeSkill.Execute();
		else
		{
			if (currentSkillType == SkillType.Piece)
				pieceSkill.LoadSelector(cardData.pieces, !cardData.isOwn, !FindObjectOfType<ChessAIGameManager>().IsBlackAIEnabled ? ChessColor.Black : ChessColor.White);
			else
				tableSkill.LoadSelector();
		}
	}

	//즉시 실행
	public void ExecuteSkill()
	{
		immeSkill.Execute();
	}

	//아군 또는 적군 특정 기물 / 모든 기물 중 선택 후 실행
	public void ExecuteSkill(List<VisualChessPiece> list)
	{
		pieceSkill.Execute(list);
	}

	//모든 / 기물 없는 테이블 중 선택 후 실행
	public void ExecuteSkill(List<VisualChessTableTile> list)
	{
		tableSkill.Execute(list);
	}
	
	private string CardTierToKorean(CardTier tier) 
	{
		switch (tier)
		{
			case CardTier.Common:
				return "일반";
			case CardTier.Uncommon:
				return "고급";
			case CardTier.Rare:
				return "희귀";
			case CardTier.Mythic:
				return "신화";
			case CardTier.Legendary:
				return "전설";
			default:
				return "알 수 없음";
		}
	}

}
