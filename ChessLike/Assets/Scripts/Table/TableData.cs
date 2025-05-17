using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableData : MonoBehaviour
{
	public TableManager TableManager;
	public Vector2Int Coordinate;
	[HideInInspector]
	public Vector2 position;
	public bool IsMoveable { get; private set; }

	public void Moveable() => IsMoveable = true;
	private void Awake()
	{
		position = transform.position;
	}
}
