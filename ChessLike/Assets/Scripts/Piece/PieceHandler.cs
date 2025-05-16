using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PieceHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public SpriteRenderer spriteRenderer;

	private Material material;
	private void Start()
	{
		material = spriteRenderer.material;
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		material.SetFloat("_OutlineThick", 1f);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		material.SetFloat("_OutlineThick", 0f);
	}
}
