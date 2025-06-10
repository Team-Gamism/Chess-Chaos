using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;

public class PieceHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	private RectTransform rectTransform;
	[HideInInspector]
	public Canvas canvas;

	private TableManager tableManager;

	private Vector3 prevMousePos;
	private Vector3 movementDelta;
	private Vector3 rotationDelta;

	public float rotationAmount = 0.2f;     // 마우스 이동 대비 회전 민감도
	public float rotationSpeed = 10f;       // 회전 반응 속도
	public float maxRotation = 60f;         // 최대 회전 각도
	public float followSpeed = 10f;         // 마우스를 따라가는 속도

	[SerializeField]
	private PieceData pieceData;

	[SerializeField]
	private GameObject SelectSprite;

	public Pawn pawn;
	public Knight knight;
	public Rook rook;
	public Bishop bishop;
	public Queen queen;
	public King king;

	private bool isDragging = false;
	public bool isDragable = false;

	public bool isEnable = true;

	[HideInInspector]
	public List<TableData> tableList = new List<TableData>();
	private Vector2 pervPos;

	private void Awake()
	{
		SelectSprite = transform.GetChild(0).gameObject;

		switch (pieceData.PieceType)
		{
			case PieceType.Pawn:
				pawn = gameObject.GetComponent<Pawn>(); break;
			case PieceType.Knight:
				knight = gameObject.GetComponent<Knight>(); break;
			case PieceType.Rook:
				rook = gameObject.GetComponent<Rook>(); break;
			case PieceType.Bishop:
				bishop = gameObject.GetComponent<Bishop>(); break;
			case PieceType.Queen:
				queen = gameObject.GetComponent<Queen>(); break;
			case PieceType.King:
				king = gameObject.GetComponent<King>(); break;

		}
		tableManager = FindObjectOfType<TableManager>();
		rectTransform = GetComponent<RectTransform>();
	}

	public void SetPiece(PieceType piece)
	{
		switch (piece)
		{
			case PieceType.Pawn:
				pawn = gameObject.GetComponent<Pawn>(); break;
			case PieceType.Knight:
				knight = gameObject.GetComponent<Knight>(); break;
			case PieceType.Rook:
				rook = gameObject.GetComponent<Rook>(); break;
			case PieceType.Bishop:
				bishop = gameObject.GetComponent<Bishop>(); break;
			case PieceType.Queen:
				queen = gameObject.GetComponent<Queen>(); break;
			case PieceType.King:
				king = gameObject.GetComponent<King>(); break;

		}
	}
	private void Update()
	{
		if (!isEnable) return;

		if (isDragging)
		{
			FollowMouseRotation();
			FollowMousePosition();
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!isEnable) return;

		if (!isDragable || GameManager.instance.IsPromotion) return;

		SelectSprite.SetActive(true);
		prevMousePos = Input.mousePosition;
		pervPos = rectTransform.anchoredPosition;

		int count = transform.parent.childCount;
		transform.SetSiblingIndex(count-1);

		isDragging = true;

		pieceData.curTable.IsPiece = false;

		if (!tableManager.isSelect) tableManager.isSelect = true;
		
		SetTablelist();

		for (int i = 0; i < tableList.Count; i++)
		{
			tableList[i].pieceMoveAppear.PieceMoveable = true;
		}
	}

	public void SetTablelist()
	{
		tableList.Clear();
		tableList = FindSpots();
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!isEnable) return;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!isEnable) return;

		if (!isDragable || GameManager.instance.IsPromotion) return;

		isDragging = false;
		SelectSprite.SetActive(false);

		//드래그한 곳이 갈 수 없다면 원래 좌표로 이동
		if (!tableManager.ReturnTableNear(rectTransform.anchoredPosition).pieceMoveAppear.PieceMoveable)
		{
			rectTransform.DOAnchorPos(pervPos, 0.2f).SetEase(Ease.OutCirc);
			rectTransform.DORotate(new Vector3(0f, 0f, 0f), 0.2f).SetEase(Ease.OutCirc);
			pieceData.curTable.IsPiece = true;
		}
		
		//드래그한 곳이 갈 수 있다면 그 근처 테이블 좌표로 이동
		else
		{
			Vector2 p = tableManager.ReturnNearTable(rectTransform.anchoredPosition);
			rectTransform.DOAnchorPos(p, 0.2f).SetEase(Ease.OutCirc);
			rectTransform.DORotate(new Vector3(0f, 0f, 0f), 0.2f).SetEase(Ease.OutCirc);
			pervPos = p;

			Vector2Int newCoord = tableManager.ReturnTableNear(rectTransform.anchoredPosition).Coordinate;

			//폰이 2칸 이동한 경우 앙파상 가능하도록 하기
			if (pieceData.PieceType == PieceType.Pawn)
			{
				if(Mathf.Abs(pieceData.coordinate.y - newCoord.y) == 2)
				{
					GetComponent<EnPassantHandler>().TwoMove = true;
				}
				else
				{
					GetComponent<EnPassantHandler>().TwoMove = false;
				}
			}

			//테이블 위치 업데이트
			pieceData.coordinate = newCoord;
			pieceData.curTable.IsPiece = false;
			pieceData.curTable.piece = null;

			pieceData.UpdateTableCoordinate();
			pieceData.curTable.IsPiece = true;
			pieceData.curTable.piece = pieceData;

			pieceData.moveCount++;

			//폰일 시 이후 스킬 적용 / 행동 금지
			if (pieceData.PieceType == PieceType.Pawn && pieceData.IsPlayerPiece)
			{
				pawn.CanMoveSide = false;
				pawn.IsFirstMove = false;

				//스킬 사용 흔적 지우기
				if (GameManager.instance.isPawnMoveOnce)
				{
					Pawn[] pawns = FindObjectsOfType<Pawn>().Where(p => p.GetComponent<PieceData>().IsPlayerPiece).ToArray();
					for (int i = 0; i < pawns.Length; i++)
					{
						if (pawns[i].GetComponent<PieceData>().moveCount > 0 && pawns[i].IsFirstMove) pawns[i].IsFirstMove = false;
					}
					GameManager.instance.isPawnMoveOnce = false;
				}

				int promotionY = pieceData.IsPlayerPiece ? 0 : 8;

				//폰 프로모션 확인
				if (pieceData.coordinate.y == promotionY) GetComponent<PawnPromotion>().StartPromotion();
			}

			//암습의 폰 켜져있을 시 끄기
			if (pieceData.IsSnakePawn)
			{
				pieceData.ChangePieceType(PieceType.Pawn);
				pieceData.IsSnakePawn = false;
			}

			//룩일 시 첫 움직임 해제
			if (pieceData.PieceType == PieceType.Rook && pieceData.IsPlayerPiece)
			{
				rook.isFirstMove = false;
			}
			
			//킹일 경우 첫 움직임 해제
			if(pieceData.PieceType == PieceType.King && pieceData.IsPlayerPiece)
			{
				king.isFirstMove = false;
				if (pieceData.curTable.IsCastlingAble)
				{
					//근처 룩 이동 함수
					MoveRook();
					//-------//
					pieceData.curTable.IsCastlingAble = false;
				}
			}
		}

		GameManager.instance.SortPieceSibling();

		for (int i = 0; i < tableList.Count; i++)
		{	
			tableList[i].pieceMoveAppear.PieceMoveable = false;
		}
	}

	private void MoveRook()
	{
		Rook nRook;

		if(GetComponent<PieceData>().coordinate.x >= 4)
		{
			nRook = FindObjectsOfType<Rook>()
				.Select(r => new { rook = r, data = r.GetComponent<PieceData>() })
				.Where(x => x.data.coordinate.x >= 4 && x.data.IsPlayerPiece)
				.Select(x => x.rook)
				.FirstOrDefault();
		}
		else
		{
			nRook = FindObjectsOfType<Rook>()
				.Select(r => new { rook = r, data = r.GetComponent<PieceData>() })
				.Where(x => x.data.coordinate.x < 4 && x.data.IsPlayerPiece)
				.Select(x => x.rook)
				.FirstOrDefault();
		}

		Vector2Int pos = gameObject.GetComponent<PieceData>().coordinate;
		Vector2Int newPos = new Vector2Int(pos.x >= 4 ? 5 : 3, pos.y);

		nRook.gameObject.GetComponent<PieceHandler>().MovePieceByCoordinate(newPos);

		GameManager.instance.SortPieceSibling();
	}

	public void MovePieceByCoordinate(Vector2Int coord)
	{
		TableData t = tableManager.GetTableByCoordinate(coord);
		Vector2 p = t.positionToRect(canvas);
		rectTransform.DOAnchorPos(p, 0.2f).SetEase(Ease.OutCirc);
		rectTransform.DORotate(new Vector3(0f, 0f, 0f), 0.2f).SetEase(Ease.OutCirc);

		//테이블 위치 업데이트
		pieceData.coordinate = coord;
		pieceData.curTable.IsPiece = false;
		pieceData.curTable.piece = null;

		pieceData.UpdateTableCoordinate();
		pieceData.curTable.IsPiece = true;
		pieceData.curTable.piece = pieceData;
	}

	private void FollowMouseRotation()
	{
		Vector3 currentMousePos = Input.mousePosition;
		Vector3 mouseDelta = currentMousePos - prevMousePos;

		float mouseMovementX = mouseDelta.x;

		movementDelta = Vector3.Lerp(movementDelta, new Vector3(mouseMovementX, 0f, 0f), 25f * Time.deltaTime);
		Vector3 movementRotation = movementDelta * rotationAmount;
		rotationDelta = Vector3.Lerp(rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);

		rectTransform.localEulerAngles = new Vector3(
			rectTransform.localEulerAngles.x,
			rectTransform.localEulerAngles.y,
			Mathf.Clamp(-rotationDelta.x, -maxRotation, maxRotation)
		);

		prevMousePos = currentMousePos;
	}

	private void FollowMousePosition()
	{
		Vector2 targetPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			canvas.transform as RectTransform,
			Input.mousePosition,
			canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
			out targetPos
		);

		// 부드럽게 위치 이동
		rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, targetPos, followSpeed * Time.deltaTime);
	}

	private List<TableData> FindSpots()
	{
		switch (pieceData.PieceType)
		{
			case PieceType.Pawn:
				return pawn.FindMoveableSpots(pieceData.coordinate, tableManager);
			case PieceType.Knight:
				return knight.FindMoveableSpots(pieceData.coordinate, tableManager);
			case PieceType.Rook:
				return rook.FindMoveableSpots(pieceData.coordinate, tableManager);
			case PieceType.Bishop:
				return bishop.FindMoveableSpots(pieceData.coordinate, tableManager);
			case PieceType.Queen:
				return queen.FindMoveableSpots(pieceData.coordinate, tableManager);
			case PieceType.King:
				return king.FindMoveableSpots(pieceData.coordinate, tableManager);
		}
		return null;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (GameManager.instance.isSelectorEnable)
		{
			if (pieceData.PieceType == PieceType.Pawn && GameManager.instance.isSnakePawn)
			{
				FindObjectOfType<SnakePawn>().SetpieceData(pieceData);

				FindObjectOfType<SkillLoader>().ExecuteSkill();
				GameManager.instance.isSnakePawn = false;
			}

			else if(pieceData.PieceType == PieceType.Pawn && GameManager.instance.isPawnShield)
			{
				FindObjectOfType<PawnShield>().SetpieceData(pieceData);

				FindObjectOfType<SkillLoader>().ExecuteSkill();
				GameManager.instance.isPawnShield = false;
			}
		}
	}
}
