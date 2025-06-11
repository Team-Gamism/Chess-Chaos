using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public bool PlayerTurn;

	/// <summary>
	/// False이면 검정, True이면 흰색
	/// </summary>
	/// 
	public bool PlayerColor;

	public bool IsCheckmate = false;

	public bool IsPromotion = false;

	[Header("스킬 조건 관련 변수")]

	public bool isSelectorEnable = false;
	public bool isPawnMoveOnce = false;
	public bool isSnakePawn = false;
	public bool isPawnShield = false;
	public bool isPawnAimming = false;

	public int TurnCount = 0;
	
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

	private void Start()
	{
		Application.targetFrameRate = 65;
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
}

