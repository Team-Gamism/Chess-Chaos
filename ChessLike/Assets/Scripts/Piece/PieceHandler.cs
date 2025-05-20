using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;

public class PieceHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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

	private Pawn pawn;
	private Knight knight;
	private Rook rook;
	private Bishop bishop;

	private bool isDragging = false;

	private List<TableData> tableList = new List<TableData>();
	private Vector2 pervPos;

	private void Awake()
	{
		//추후 다른 기물들도 추가 예정
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
		}
		tableManager = FindObjectOfType<TableManager>();
		rectTransform = GetComponent<RectTransform>();
	}

	private void Update()
	{
		if (isDragging)
		{
			FollowMouseRotation();
			FollowMousePosition();
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		prevMousePos = Input.mousePosition;
		pervPos = rectTransform.anchoredPosition;
		isDragging = true;

		if (!tableManager.isSelect) tableManager.isSelect = true;
		//기존 보이던 위치 이동 스프라이트를 모두 제거하는 코드 추가하기
		
		tableList.Clear();
		tableList = FindSpots();

		for (int i = 0; i < tableList.Count; i++)
		{
			tableList[i].pieceMoveAppear.PieceMoveable = true;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;

		//놓을 수 없는 부분에 두면 기존 위치로 돌아가기

		if (!tableManager.ReturnTableNear(rectTransform.anchoredPosition).pieceMoveAppear.PieceMoveable)
		{
			rectTransform.DOAnchorPos(pervPos, 0.2f).SetEase(Ease.OutCirc);
			rectTransform.DORotate(new Vector3(0f, 0f, 0f), 0.2f).SetEase(Ease.OutCirc);
		}
		else
		{
			Vector2 p = tableManager.ReturnNearTable(rectTransform.anchoredPosition);
			rectTransform.DOAnchorPos(p, 0.4f).SetEase(Ease.OutCirc);
			rectTransform.DORotate(new Vector3(0f, 0f, 0f), 0.4f).SetEase(Ease.OutCirc);
			pervPos = p;

			if(pieceData.PieceType == PieceType.Pawn) pawn.IsFirstMove = false;
			pieceData.coordinate = tableManager.ReturnTableNear(rectTransform.anchoredPosition).Coordinate;
		}

		for (int i = 0; i < tableList.Count; i++)
		{	
			tableList[i].pieceMoveAppear.PieceMoveable = false;
		}
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
		}
		return null;
	}
}
