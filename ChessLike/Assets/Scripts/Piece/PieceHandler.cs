using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;

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

	[SerializeField]
	private GameObject SelectSprite;

	private Pawn pawn;
	private Knight knight;
	private Rook rook;
	private Bishop bishop;
	private Queen queen;
	private King king;

	private bool isDragging = false;

	private List<TableData> tableList = new List<TableData>();
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
		SelectSprite.SetActive(true);
		prevMousePos = Input.mousePosition;
		pervPos = rectTransform.anchoredPosition;

		int count = transform.parent.childCount;
		transform.SetSiblingIndex(count-1);

		isDragging = true;

		pieceData.curTable.IsPiece = false;

		if (!tableManager.isSelect) tableManager.isSelect = true;
		
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

			if(pieceData.PieceType == PieceType.Pawn) pawn.IsFirstMove = false;
			pieceData.coordinate = tableManager.ReturnTableNear(rectTransform.anchoredPosition).Coordinate;
			pieceData.curTable.IsPiece = false;

			pieceData.UpdateTableCoordinate();
			pieceData.curTable.IsPiece = true;
		}

		SortPieceSibling();

		for (int i = 0; i < tableList.Count; i++)
		{	
			tableList[i].pieceMoveAppear.PieceMoveable = false;
		}
	}

	private void SortPieceSibling()
	{
		List<PieceData> list = new List<PieceData>();
		list = FindObjectsOfType<PieceData>().ToListPooled();
		list.Sort((a,b) =>
		{
			if(a.coordinate.y != b.coordinate.y) 
				return a.coordinate.y.CompareTo(b.coordinate.y);
			else
				return a.coordinate.x.CompareTo(b.coordinate.x);
		});
		for(int i = 0; i < list.Count; i++)
		{
			list[i].transform.SetSiblingIndex(i);
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
			case PieceType.Queen:
				return queen.FindMoveableSpots(pieceData.coordinate, tableManager);
			case PieceType.King:
				return king.FindMoveableSpots(pieceData.coordinate, tableManager);
		}
		return null;
	}
}
