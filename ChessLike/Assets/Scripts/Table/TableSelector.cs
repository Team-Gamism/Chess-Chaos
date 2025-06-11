using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TableSelector : MonoBehaviour
{
	private Canvas cameraSize;

	[SerializeField]
	private TableSetter tableSetter;

	[SerializeField]
	private GameObject log;

	[SerializeField]
	private Button DoneBtn;

	private Image image;

	public bool Donable = false;

	private TableManager tableManager;

	private void OnEnable()
	{
		tableManager = FindObjectOfType<TableManager>();
		image = GetComponent<Image>();
		cameraSize = FindObjectsOfType<Canvas>().Where(p => p.CompareTag("ScreenUI")).FirstOrDefault();
		GetComponent<RectTransform>().sizeDelta = cameraSize.GetComponent<RectTransform>().sizeDelta;

		GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -230, 0);

		GameManager.instance.isSelectorEnable = true;

		transform.SetAsLastSibling();

		log.SetActive(true);
		DoneBtn.gameObject.SetActive(true);
		DoneBtn.onClick.AddListener(()=>TableAttributeChange());
	}

	public void DisableImage()
	{
		GameManager.instance.isSelectorEnable = false;
		log.SetActive(false);
		DoneBtn.gameObject.SetActive(false);
		gameObject.SetActive(false);
	}

	private void Update()
	{
		if (Donable)
		{
			DoneBtn.interactable = true;
		}
		else DoneBtn.interactable = false;
	}

	public void TableAttributeChange()
	{
		tableSetter.GetCoordinateList();
		List<Vector2Int> list = tableSetter.selectedTable;
		for(int i = 0; i < list.Count; i++)
		{
			TableData t=  tableManager.GetTableByCoordinate(list[i]);
			t.IsMoveable = false;
			t.NotMoveCount = 1;
			t.registeredTurnCount = GameManager.instance.TurnCount;
		}
		DisableImage();
	}

}
