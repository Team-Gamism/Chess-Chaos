using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableSetter : MonoBehaviour
{
	private List<TableSelectorChild> tableSelectorChildren = new List<TableSelectorChild>();

	public Queue<TableSelectorChild> tableSelected = new Queue<TableSelectorChild>();

	[HideInInspector]
	public TableSelector tableSelector;

	private void OnEnable()
	{
		int cnt = tableSelected.Count;
		for (int i = 0; i < cnt; i++)
		{
			tableSelected.First().InitField();
			tableSelected.Dequeue();
		}
		tableSelector = GetComponentInParent<TableSelector>();
		tableSelectorChildren = FindObjectsOfType<TableSelectorChild>().ToList();
		GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 230f, 0f);
	}

	private void Update()
	{
		if (tableSelected.Count >= tableSelector.cardData.RequireZoneCnt && tableSelected.Count < tableSelector.cardData.MaxZoneCnt+1)
		{
			tableSelector.Donable = true;
		}
		else
		{
			tableSelector.Donable = false;
		}
	}

	public int GetCoordinateListCount()
	{
		List<Vector2Int> result = new List<Vector2Int>();
		foreach (TableSelectorChild child in tableSelectorChildren)
		{
			if (child.isSelected) result.Add(child.coordinate);
		}
		return result.Count;
	}

	public void SetFirstFalse()
	{
		tableSelected.ElementAt(0).isSelected = false;
		Debug.Log(tableSelectorChildren[0].isSelected);
		tableSelected.Dequeue();
	}

	public void RemoveFromQueue(TableSelectorChild target)
	{
		Queue<TableSelectorChild> newQueue = new Queue<TableSelectorChild>();
		foreach (var item in tableSelected)
		{
			if (item != target)
				newQueue.Enqueue(item);
			else
				item.isSelected = false;
		}
		tableSelected = newQueue;
	}
}
