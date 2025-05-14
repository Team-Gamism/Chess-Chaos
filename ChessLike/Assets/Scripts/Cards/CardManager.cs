using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	public List<Card> cards = new List<Card>();
	public Canvas canvas;

	public Vector2[] rects = new Vector2[5];

	private void Start()
	{
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

}
