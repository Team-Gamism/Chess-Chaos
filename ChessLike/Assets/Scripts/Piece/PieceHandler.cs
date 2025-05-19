using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

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

	private bool isDragging = false;

	private void Awake()
	{
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
		isDragging = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;
		rectTransform.DOAnchorPos(tableManager.ReturnNearTable(rectTransform.anchoredPosition), 0.2f);
		rectTransform.DORotate(new Vector3(0f, 0f, 0f), 0.2f);
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
}
