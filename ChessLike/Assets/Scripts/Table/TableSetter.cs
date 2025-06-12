using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableSetter : MonoBehaviour
{
	private List<TableSelectorChild> tableSelectorChildren = new List<TableSelectorChild>();

	public List<Vector2Int> selectedTable = new List<Vector2Int>();

	[HideInInspector]
	public TableSelector tableSelector;

	private void OnEnable()
	{
		tableSelector = GetComponentInParent<TableSelector>();
		tableSelectorChildren = FindObjectsOfType<TableSelectorChild>().ToList();
		GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 230f, 0f);
	}

	private void Update()
	{
		if (selectedTable.Count >= tableSelector.cardData.RequireZoneCnt && selectedTable.Count <= tableSelector.cardData.MaxZoneCnt)
		{
			tableSelector.Donable = true;
		}
		else
		{
			tableSelector.Donable = false;
		}
	}

	//선택된 테이블들을 배열로 반환
	public void GetCoordinateList()
	{
		selectedTable.Clear();
		List<Vector2Int> result = new List<Vector2Int>();
		foreach (TableSelectorChild child in tableSelectorChildren)
		{
			if (child.isSelected) result.Add(child.coordinate);
		}
		selectedTable = result;
	}	
}
