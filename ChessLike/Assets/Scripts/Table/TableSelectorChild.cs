using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class TableSelectorChild : MonoBehaviour, IPointerClickHandler
{
	private TableSetter tableSetter;
	public bool isSelected;

	private bool isPiece;

	public Vector2Int coordinate;
	private void Start()
	{
		tableSetter = GetComponentInParent<TableSetter>();
		coordinate.x = transform.GetSiblingIndex() % 8;
		coordinate.y = transform.GetSiblingIndex() / 8;

		TableManager t = FindObjectOfType<TableManager>();

		if (t.GetTableByCoordinate(coordinate).IsPiece) isPiece = true;
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		if (isPiece) return;

		isSelected = !isSelected;
		if (isSelected)
			tableSetter.tableSelected.Enqueue(this);
		else
			tableSetter.RemoveFromQueue(this);

		if (tableSetter.tableSelected.Count > tableSetter.tableSelector.cardData.MaxZoneCnt)
		{
			tableSetter.SetFirstFalse();
		}
	}

	public void Update()
	{
		if (isSelected)
		{
			GetComponent<Image>().color = new Color(1f,1f,1f, 0);
		}
		else
		{
			GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
		}
	}
}
