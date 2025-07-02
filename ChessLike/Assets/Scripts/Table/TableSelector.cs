using System.Collections.Generic;
using System.Linq;
using ChessEngine.Game;
using ChessEngine.Game.AI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TableSelector : MonoBehaviour
{
	private Canvas cameraSize;

	[SerializeField]
	private GameObject log;

	[SerializeField]
	private Button DoneBtn;

	private Image image;

	[HideInInspector]
	public bool Donable = false;

	private static List<VisualChessTableTile> tiles;
	public List<VisualChessTableTile> selectedTiles = new List<VisualChessTableTile>();

	[HideInInspector]
	public CardData cardData;
	[HideInInspector]
	public int NotMoveCount;

	private UnityEvent<VisualChessTableTile> OnAddEntity = new UnityEvent<VisualChessTableTile>();
	private UnityEvent<VisualChessTableTile> OnDestroyEntity = new UnityEvent<VisualChessTableTile>();

	private static ChessAIGameManager aIGameManager;
	private static SkillLoader skillLoader;

	private void Awake()
	{
		tiles = FindObjectsOfType<VisualChessTableTile>().ToList();
		aIGameManager = FindObjectOfType<ChessAIGameManager>();
		skillLoader = FindObjectOfType<SkillLoader>();
	}

    private void OnEnable()
	{
		aIGameManager.isCardSelect = true;
		aIGameManager.Deselect();
		image = GetComponent<Image>();
		cameraSize = FindObjectsOfType<Canvas>().Where(p => p.CompareTag("ScreenUI")).FirstOrDefault();

		Vector2 scale = cameraSize.GetComponent<RectTransform>().sizeDelta;

		GetComponent<RectTransform>().sizeDelta = new Vector2(scale.x + 500, scale.y + 500);

		GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -230, 0);

		transform.SetAsLastSibling();

		log.SetActive(true);
		DoneBtn.gameObject.SetActive(true);
		DoneBtn.onClick.RemoveAllListeners();
		DoneBtn.onClick.AddListener(() => TableAttributeChange());
	}

	public void DisableImage()
	{
		aIGameManager.isCardSelect = false;
		DestroyAllEntity();
		log.SetActive(false);
		DoneBtn.gameObject.SetActive(false);
		gameObject.SetActive(false);
	}

	private void Update()
	{
		if (selectedTiles.Count >= cardData.RequireZoneCnt)
		{
			DoneBtn.interactable = true;
		}
		else DoneBtn.interactable = false;
	}

	public void AddEntity(VisualChessTableTile tile)
	{
		selectedTiles.Add(tile);
		OnAddEntity.RemoveAllListeners();
		OnAddEntity.AddListener((t) =>
		{
			TileSelect(t);
		});

		OnAddEntity?.Invoke(tile);
	}

	public void DestroyEntity(VisualChessTableTile tile)
	{
		selectedTiles.Remove(tile);
		OnDestroyEntity.RemoveAllListeners();
		OnDestroyEntity.AddListener((t) =>
		{
			TileDeselect(t);
		});	

		OnDestroyEntity?.Invoke(tile);
	}

	public void DestroyAllEntity()
	{
		List<VisualChessTableTile> list = selectedTiles.ToList();
		foreach (VisualChessTableTile tile in list)
		{
			selectedTiles.Remove(tile);
			TileDeselect(tile);
		}
	}

	public void DestroyFirstEntity()
	{
		VisualChessTableTile tile1 = selectedTiles[0];
		selectedTiles.Remove(tile1);
		OnDestroyEntity.RemoveAllListeners();
		OnDestroyEntity.AddListener((t) =>
		{
			TileDeselect(t);
		});

		OnDestroyEntity?.Invoke(tile1);
	}

	public void TileSelect(VisualChessTableTile tile)
	{
		tile.GetComponent<TileHandler>().OnFrontHighlight();
	}
	public void TileDeselect(VisualChessTableTile tile)
	{
		tile.GetComponent<TileHandler>().OnFrontUnhilight();
	}

	public void TableAttributeChange()
	{
		skillLoader.ExecuteSkill(selectedTiles);
		DisableImage();
	}

}
