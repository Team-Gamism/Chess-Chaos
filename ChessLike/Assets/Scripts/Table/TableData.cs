using UnityEngine;

public class TableData : MonoBehaviour
{
	public TableManager TableManager;
	public Vector2Int Coordinate;
	[HideInInspector]
	public Vector2 position;

	public bool IsMoveable;
	public int NotMoveCount;
	public int registeredTurnCount;
	public bool IsPiece;

	public bool IsCastlingAble = false;

	public PieceData piece;

	public PieceMoveAppear pieceMoveAppear;

	private void Awake()
	{
		pieceMoveAppear = GetComponentInChildren<PieceMoveAppear>();
		position = transform.position;
	}
	public Vector2 positionToRect(Canvas canvas)
	{
		Vector3 tableTrans = Camera.main.WorldToScreenPoint(position);

		Vector2 canvasPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), tableTrans, Camera.main, out canvasPos);

		return canvasPos;
	}

	private void Update()
	{
		if (GameManager.instance.TurnCount >= registeredTurnCount + NotMoveCount)
		{
			IsMoveable = true;
			NotMoveCount = 0;
			registeredTurnCount = 0;
		}
	}
}
