using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceData : MonoBehaviour
{
	public PieceType PieceType;
	public bool isWhite;
	[HideInInspector]
	public bool IsFirstMove = true;

	public bool IsStatic;
	[HideInInspector]
	public int StaticTurn;

	private Vector2 coordinate;
	private TableManager tableManager;

	private void Start()
	{
		tableManager = FindObjectOfType<TableManager>();
		gameObject.transform.position = GetTablePosition(53);
	}

	public void MovePiece()
	{
		if (IsFirstMove)
		{
			//좌표 변경 코드
			IsFirstMove = false;
		}
		else
		{
			//좌표 변경 코드

		}
	}
	private Vector2 GetTablePosition(int idx)
	{
		Transform tableTrans = tableManager.TableList[idx].transform;
		return tableTrans.parent.TransformPoint(tableTrans.position);
	}
}

