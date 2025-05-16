using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableData : MonoBehaviour
{
	public TableManager TableManager;
	public Vector2Int Coordinate;
	public bool IsMoveable { get; private set; }

	public void Moveable() => IsMoveable = true;
}
