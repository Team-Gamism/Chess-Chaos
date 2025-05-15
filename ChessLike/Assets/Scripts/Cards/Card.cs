using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, 
	IEndDragHandler, ISwapHandler, IPointerEnterHandler, 
	IPointerExitHandler, IPointerClickHandler
{
	public GameObject childCard;
	public CardManager manager;

	private Canvas thisCanvas;

	private RectTransform rectTransform;

	private Vector2 dragOffset;

	public float rotationAmount = 5f;
	public float rotationSpeed = 10f;

	public bool isSelect = false;


	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		childCard = transform.GetChild(0).gameObject;
		thisCanvas = GetComponent<Canvas>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (isSelect) return;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			manager.canvas.transform as RectTransform,
			eventData.position,
			manager.canvas.worldCamera,
			out Vector2 localPoint
		);

		dragOffset = rectTransform.anchoredPosition - localPoint;

		thisCanvas.sortingOrder = 2;

		manager.isDragging = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (isSelect) return;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			manager.canvas.transform as RectTransform,
			eventData.position,
			manager.canvas.worldCamera,
			out Vector2 localPoint
		);

		rectTransform.anchoredPosition = localPoint + dragOffset;

		for (int i = 0; i < manager.cards.Count; i++)
		{
			if (manager.cards[i] == this) continue;

			if (transform.position.x > manager.cards[i].transform.position.x &&
				transform.GetSiblingIndex() < manager.cards[i].transform.GetSiblingIndex())
			{
				Swap(i, 1);
				break;
			}

			if (transform.position.x < manager.cards[i].transform.position.x &&
				transform.GetSiblingIndex() > manager.cards[i].transform.GetSiblingIndex())
			{
				Swap(i, -1);
				break;
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (isSelect) return;
		GetComponent<RectTransform>().DOAnchorPos(manager.rects[transform.GetSiblingIndex()], 0.2f).SetEase(Ease.OutQuad);
		manager.isDragging = false;

		thisCanvas.sortingOrder = 1;
	}

	private void Swap(int i, int dir)
	{
		if (isSelect) return;
		OnSwapped(dir, manager.cards[i].childCard);

		// Swap 카드 위치 (부모의 SiblingIndex만 바꿈)
		int myIndex = transform.GetSiblingIndex();
		int otherIndex = manager.cards[i].transform.GetSiblingIndex();

		transform.SetSiblingIndex(otherIndex);
		manager.cards[i].transform.SetSiblingIndex(myIndex);
	}

	public void OnSwapped(int dir, GameObject swapped)
	{
		if (isSelect) return;
		if (dir > 0) swapped.transform.parent.GetComponent<Animator>().SetTrigger("Right");
		else swapped.transform.parent.GetComponent<Animator>().SetTrigger("Left");
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (manager.isDragging || isSelect) return;
		childCard.transform.DOKill(childCard.transform);
		childCard.transform.DOPunchRotation(new Vector3(0, 0, 5f), 0.2f);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (manager.isDragging || isSelect) return;

	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Vector3 scale = childCard.transform.localScale;
		if (manager.isDragging) return;

		isSelect = !isSelect;

		if (isSelect) childCard.transform.DOScale(new Vector2(scale.x+0.1f, scale.y+0.1f), 0.2f);
		else childCard.transform.DOScale(new Vector2(scale.x  - 0.1f, scale.y - 0.1f), 0.2f);
	}
}
