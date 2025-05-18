using System.Collections;
using System.Collections.Generic;
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
}

