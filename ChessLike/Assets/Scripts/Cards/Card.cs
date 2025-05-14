using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, ISwapHandler
{
	public GameObject childCard;
	public CardManager manager;

	private Animator animator;

	private RectTransform rectTransform;
	private Vector2 originalPos;
	private Transform originalParent;

	private Vector2 dragOffset;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		childCard = transform.GetChild(0).gameObject;
		animator = GetComponent<Animator>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		originalPos = rectTransform.anchoredPosition;
		originalParent = transform.parent;

		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			manager.canvas.transform as RectTransform,
			eventData.position,
			manager.canvas.worldCamera,
			out Vector2 localPoint
		);

		dragOffset = rectTransform.anchoredPosition - localPoint;
	}

	public void OnDrag(PointerEventData eventData)
	{
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
		GetComponent<RectTransform>().DOAnchorPos(manager.rects[transform.GetSiblingIndex()], 0.2f);
	}

	private void Swap(int i, int dir)
	{
		OnSwapped(dir, manager.cards[i].childCard);

		// Swap 카드 위치 (부모의 SiblingIndex만 바꿈)
		int myIndex = transform.GetSiblingIndex();
		int otherIndex = manager.cards[i].transform.GetSiblingIndex();

		transform.SetSiblingIndex(otherIndex);
		manager.cards[i].transform.SetSiblingIndex(myIndex);
	}

	public void OnSwapped(int dir, GameObject swapped)
	{
		if (dir > 0) swapped.transform.parent.GetComponent<Animator>().SetTrigger("Right");
		else swapped.transform.parent.GetComponent<Animator>().SetTrigger("Left");
	}
}
