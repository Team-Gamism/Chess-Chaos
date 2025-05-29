using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public bool PlayerTurn;

	/// <summary>
	/// False이면 검정, True이면 흰색
	/// </summary>
	public bool PlayerColor;

	
	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(instance);
		}
	}

	public void SortPieceSibling()
	{
		List<PieceData> list = new List<PieceData>();
		list = FindObjectsOfType<PieceData>().ToListPooled();
		list.Sort((a, b) =>
		{
			if (a.coordinate.y != b.coordinate.y)
				return a.coordinate.y.CompareTo(b.coordinate.y);
			else
				return a.coordinate.x.CompareTo(b.coordinate.x);
		});
		for (int i = 0; i < list.Count; i++)
		{
			list[i].transform.SetSiblingIndex(i);
		}
	}

	//폰 방어막이 켜져있으면 true 반환
	public bool isPawnShield(PieceData piece)
	{
		return piece.PieceType == PieceType.Pawn && piece.GetComponent<PieceHandler>().pawn.IsShield;
	}
}

