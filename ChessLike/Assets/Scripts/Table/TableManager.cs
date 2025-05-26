using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using UnityEditor.Tilemaps;
using UnityEngine;

public class TableManager : MonoBehaviour
{
	public List<TableData> TableList = new List<TableData>();
	public Canvas PieceCanvas;

	public bool isSelect = false;


	public void Awake()
	{
		TableData[] tables = FindObjectsOfType<TableData>();
		foreach(TableData table in tables)
		{
			TableList.Add(table);
		}
		TableList.Sort((a, b) =>
		{
			if (a.Coordinate.y != b.Coordinate.y)
				return a.Coordinate.y.CompareTo(b.Coordinate.y);
			else
				return a.Coordinate.x.CompareTo(b.Coordinate.x);
		});
	}

	
	//테스트 코드
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log(ReturnCurrentTableToFEN());
		}
	}

	public Vector2 ReturnNearTable(Vector2 pos)
	{
		float distance = float.MaxValue;
		Vector2 result = Vector2.zero;
		foreach(TableData table in TableList)
		{
			Vector2 tablePos = table.positionToRect(PieceCanvas);
			float _distance = Vector2.Distance(tablePos, pos);
			if (_distance < distance)
			{
				distance = _distance;
				result = tablePos;
			}
		}
		return result;
	}

	public TableData ReturnTableNear(Vector2 pos)
	{
		float distance = float.MaxValue;
		TableData tableData = null;
		foreach (TableData table in TableList)
		{
			Vector2 tablePos = table.positionToRect(PieceCanvas);
			float _distance = Vector2.Distance(tablePos, pos);
			if (_distance < distance)
			{
				distance = _distance;
				tableData = table;
			}
		}
		return tableData;
	}

	public TableData GetTableByCoordinate(Vector2Int coord)
	{
		for(int i  = 0; i < TableList.Count; i++)
		{
			if (TableList[i].Coordinate == coord) return TableList[i];
		}
		return null;
	}

	public string ReturnCurrentTableToFEN()
	{
		string result = "";
		for(int i = 0; i < 8; i++)
		{
			result += currentRowFEN(i);
		}
		return result;
	}
	private string currentRowFEN(int idx)
	{
		string result = "";

		List<TableData> tables = new List<TableData>();
		tables = findRowTable(idx);

		int pieceidx = 0;

		bool isRowNull = true;
		
		for(int i = 0; i < 8; i++)
		{
			string n = "";
			if (!tables[i].piece)
			{
				pieceidx++;
				continue;
			}
			else
			{
				isRowNull = false;
				if(pieceidx > 0)
				{
					n += pieceidx;
					pieceidx = 0;
				}
				switch ((int)tables[i].piece.PieceType)
				{
					case 0:
						n += "p"; break;
					case 1:
						n += "b"; break;
					case 2:
						n += "r"; break;
					case 3:
						n += "n"; break;
					case 4:
						n += "q"; break;
					case 5:
						n += "k"; break;
				}

				if (tables[i].piece.IsPlayerPiece)
				{
					if (GameManager.instance.PlayerColor)
					{
						n = n.ToUpper();
					}
				}
				else
				{
					if (!GameManager.instance.PlayerColor)
					{
						n = n.ToUpper();
					}
				}
			}

			result += n;
		}
		if (isRowNull && pieceidx == 8) return "8/";

		if (pieceidx > 0) result += pieceidx;
		pieceidx = 0;

		if(idx != 7) result += "/";

		if (GameManager.instance.PlayerTurn) result += GameManager.instance.PlayerColor ? "w" : "b";

			return result;
	}
	private List<TableData> findRowTable(int rowIdx)
	{
		List<TableData> result = new List<TableData>();
		for(int i = 0; i < TableList.Count; i++)
		{
			if (TableList[i].Coordinate.y == rowIdx)
			{
				result.Add(TableList[i]);
			}
		}

		result.Sort((a, b) =>
			a.Coordinate.x.CompareTo(b.Coordinate.x));

		return result;
	}
}
