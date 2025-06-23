using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using JetBrains.Annotations;

public class PieceData : MonoBehaviour
{
	private Sprite[] sprites = new Sprite[2];
	public PieceType PieceType;    //기물 종류
	public bool IsPlayerPiece;     //플레이어 기물 여부 확인

	public bool IsStatic;          //고정 상태 확인
	[HideInInspector]
	public int StaticTurn;         //얼마나 오래 고정되어 있을 것인지 확인
	public int StaticedTurn;       //고정이 된 턴 횟수 저장
	public bool IsSnakePawn;       //암습의 폰이 적용된 기물인지 확인
	public bool IsShield;
	public bool DownAnimation = true;

	public Material Outline;       //바깥 태두리 메테리얼

	public Vector2Int coordinate;    //현재 테이블에서의 좌표

	private TableManager tableManager;
	private RectTransform rectTransform;
	[HideInInspector]
	public TableData curTable;

	public int moveCount;

	public TableManager GetTableManager() { return tableManager; }

	public void SpawnAnimation(int idx) //스폰 시 나타나는 애니메이션
	{
		GetComponent<PieceHandler>().isEnable = false;
		moveCount = 0;
		
		SetOutlineMaterial();
		SetSpriteColor();
		UpdateField();

		//애니메이션 코드
		Vector3 endPos = GetTablePosition(idx);
		Vector3 startPos = new Vector3(endPos.x, endPos.y + 2000, 0f);

		Sequence animation = DOTween.Sequence();

		animation.Append(rectTransform.DOAnchorPos(endPos, 0.8f).SetEase(Ease.OutCirc));
		animation.Join(rectTransform.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 1f), 1f));

		rectTransform.anchoredPosition = startPos;
		rectTransform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

		animation.Play().OnComplete(() =>
		{
			GetComponent<PieceHandler>().isDragable = true;

			//현재 인덱스 값으로 좌표 저장
			coordinate = tableManager.TableList[idx].Coordinate;

			//테이블 가져오기
			curTable = tableManager.GetTableByCoordinate(coordinate);
			curTable.piece = this;
			curTable.IsPiece = true;

			DownAnimation = false;

			if (IsPlayerPiece) GetComponent<PieceHandler>().isEnable = true;

			GetComponent<PieceHandler>().SetTablelist();

		});
	}

	public void SetOutlineMaterial()
	{
		GetComponent<Image>().material = Instantiate(Outline);
		if (GetComponent<Image>().material.GetFloat("_OutlineThick") != 0f)
		{
			GetComponent<Image>().material.SetFloat("_OutlineThick", 0f);
		}
	}

	public void SetSpriteColor()
	{
		if (IsPlayerPiece)
		{
			UpdateSprites(!GameManager.instance.PlayerColor ? 1 : 0);
		}
		else
		{
			UpdateSprites(!GameManager.instance.PlayerColor ? 0 : 1);
		}
	}

	public void UpdateSprites(int n)
	{
		sprites[0] = AtlasManager.instance.GetCurrentSkinSprite(true, PieceType);
		sprites[1] = AtlasManager.instance.GetCurrentSkinSprite(false, PieceType);

		GetComponent<Image>().sprite = sprites[n];
	}

	private void Update()
	{
		int n = GameManager.instance.TurnCount;
		if (GameManager.instance.TurnCount < n)
		{
			StaticedTurn++;
		}
		if (StaticTurn + StaticedTurn <= GameManager.instance.TurnCount)
		{
			StaticedTurn = 0;
			StaticTurn = 0;
			IsStatic = false;
		}
	}

	public void UpdateField()
	{
		tableManager = FindObjectOfType<TableManager>();
		rectTransform = GetComponent<RectTransform>();
	}

	public void RegisterPieceType(PieceType piece)
	{
		PieceHandler handler = GetComponent<PieceHandler>();
		handler.SetPiece(piece);
	}

	public void ChangePieceType(PieceType piece)
	{
		PieceType = piece;
	}
	public Vector2 GetTablePosition(int idx)
	{
		Vector3 tableTrans = tableManager.TableList[idx].position;
		tableTrans = Camera.main.WorldToScreenPoint(tableTrans);

		Vector2 canvasPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), tableTrans, Camera.main, out canvasPos);

		return canvasPos;
	}

	public void UpdateTableCoordinate()
	{
		//현재 테이블 위치를 가져오기
		curTable = tableManager.GetTableByCoordinate(coordinate);

		//앙파상 발동 코드
		Vector2Int coord = new Vector2Int(coordinate.x, coordinate.y + (IsPlayerPiece ? 1 : -1));

		//coord 좌표에 있는 테이블 가져오기
		TableData t = tableManager.GetTableByCoordinate(coord);

		if (t != null && t.IsPiece)
		{
			if (t.piece.PieceType == PieceType.Pawn && !t.piece.IsPlayerPiece)
			{
				EnPassantHandler enPassantHandler = t.piece.GetComponent<EnPassantHandler>();
				if (enPassantHandler.isEnPassant)
				{
					enPassantHandler.gameObject.GetComponent<PieceData>().curTable.IsPiece = false;
					enPassantHandler.gameObject.GetComponent<PieceData>().curTable.piece = null;
					Destroy(enPassantHandler.gameObject);
				}
			}
		}

		if (IsPlayerPiece)
		{
			if (curTable.piece != null && !curTable.piece.IsPlayerPiece) Destroy(curTable.piece.gameObject);

			else curTable.piece = this;
		}
		else
		{
			if (curTable.piece && curTable.piece.IsPlayerPiece) Destroy(curTable.piece.gameObject);

			else curTable.piece = this;
		}
	}

	public void SetOutline(int depth)
	{
		GetComponent<Image>().material.SetFloat("_OutlineThick", depth);
	}

}

