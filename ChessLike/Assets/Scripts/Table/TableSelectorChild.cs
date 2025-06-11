using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class TableSelectorChild : MonoBehaviour, IPointerClickHandler
{
	private TableSetter tableSetter;
	public bool isSelected { get { return isSelect; } set { isSelect = value; } }

	private bool isSelect = false;
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

		isSelect = !isSelect;
		Debug.Log(isSelect);
		tableSetter.GetCoordinateList();
	}

	private void Update()
	{
		if (isSelect)
		{
			GetComponent<Image>().color = new Color(1f,1f,1f, 0);
		}
		else
		{
			GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
		}
	}
}
