using System.Collections.Generic;
using ChessEngine;
using ChessEngine.Game;
using ChessEngine.Game.AI;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
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

	private SkillType currentSkillType;
	private ISkill immeSkill; //즉시 실행 스킬
	private IPieceSkill pieceSkill; // 기물 관련 스킬
	private ITableSkill tableSkill; // 테이블 관련 스킬
	private CardData cardData;


	public WarningLog warningLog;

	public void LoadSkill(GameObject obj)
	{
		InitSkillType();

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

}
