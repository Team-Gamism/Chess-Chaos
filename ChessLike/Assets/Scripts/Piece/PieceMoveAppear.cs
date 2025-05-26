using UnityEngine;

public class PieceMoveAppear : MonoBehaviour
{
	private SpriteRenderer sr;

	public bool PieceMoveable;
	public bool DeathPiece;
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

		if (DeathPiece)
		{
			sr.color = Color.red;
		}
		else
		{
			sr.color = Color.white;
		}
	}
}
