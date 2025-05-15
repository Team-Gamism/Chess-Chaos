using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public List<Card> cards = new List<Card>();
	public Canvas canvas;

	public Vector2[] rects = new Vector2[5];

	public bool isDragging = false;

	private void Start()
	{
		Application.targetFrameRate = 65;
		StartCoroutine(rectLoad());
	}
	private IEnumerator rectLoad()
	{
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < rects.Length; i++)
		{
			rects[i] = cards[i].GetComponent<RectTransform>().anchoredPosition;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		throw new System.NotImplementedException();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		throw new System.NotImplementedException();
	}
}
