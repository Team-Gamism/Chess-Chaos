using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PieceHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private Material material;

	private void Start()
	{
		material = GetComponent<Image>().material;
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
