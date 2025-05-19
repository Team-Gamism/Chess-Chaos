using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMoveAppear : MonoBehaviour
{
	private SpriteRenderer sr;

	public bool PieceMoveable;
	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
	}
	private void Update()
	{
		if (PieceMoveable)
		{
			sr.enabled = true;
		}
		else
		{
			sr.enabled = false;
		}
	}
}
