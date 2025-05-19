using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PieceAbstract : MonoBehaviour
{
	public abstract List<TableData> FindMoveableSpots(Vector2Int curPos, TableManager tableManager);

	/// <summary>
	/// 넘지 않았다면 true를, 넘었다면 false를 반환한다.
	/// </summary>
	/// <param name="coord"></param>
	/// <returns></returns>
	public virtual bool OverCoordinate(Vector2Int coord)
	{
		return coord.x <= 7 && coord.y <= 7 && coord.x >= 0 && coord.y >= 0;
	}
}
