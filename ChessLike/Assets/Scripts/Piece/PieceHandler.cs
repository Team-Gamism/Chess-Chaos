using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PieceHandler : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	private Material material;


	private void Start()
	{
		material = spriteRenderer.material;
	}
	
}
