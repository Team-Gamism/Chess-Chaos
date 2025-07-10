using System.Collections.Generic;
using System.Linq;
using ChessEngine;
using ChessEngine.Game;
using ChessEngine.Game.AI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PieceSelector : MonoBehaviour
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
	public List<VisualChessPiece> selectedPieces = new List<VisualChessPiece>();
	public List<VisualChessTableTile> selectedTiles = new List<VisualChessTableTile>();
	public List<ChessPieceType> pieceTypes = new List<ChessPieceType>();
	public ChessColor SelectColor;

	[HideInInspector]
	public CardData cardData;
	[HideInInspector]
	public int NotMoveCount;
	public Material OutlineMaterial;

	private UnityEvent<VisualChessTableTile> OnAddEntity = new UnityEvent<VisualChessTableTile>();
	private UnityEvent<VisualChessTableTile> OnDestroyEntity = new UnityEvent<VisualChessTableTile>();

	private static ChessAIGameManager aIGameManager;
	private static SkillLoader skillLoader;

	public PieceSkillType type;

	private void Awake()
	{
		tiles = FindObjectsOfType<VisualChessTableTile>().ToList();
		aIGameManager = FindObjectOfType<ChessAIGameManager>();
		skillLoader = FindObjectOfType<SkillLoader>();
	}

	public void SetField(CardData cardData, PieceSkillType skillType, List<ChessPieceType> pieceTypes, bool isAll, ChessColor color)
	{
		this.cardData = cardData;
		type = skillType;
		this.pieceTypes = pieceTypes;
		SelectColor = color;

	}

	private void OnEnable()
	{
		aIGameManager.isPieceSelect = true;
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
		DoneBtn.onClick.AddListener(() => PieceAttributeChange());
	}

	public void DisableImage()
	{
		aIGameManager.isPieceSelect = false;
		DestroyAllEntity();
		log.SetActive(false);
		DoneBtn.gameObject.SetActive(false);
		gameObject.SetActive(false);
	}

	private void Update()
	{
		if (selectedPieces.Count >= cardData.RequirePieceCnt)
		{
			DoneBtn.interactable = true;
		}
		else DoneBtn.interactable = false;
	}

	public void AddEntity(VisualChessTableTile tile)
	{
		selectedPieces.Add(tile.GetVisualPiece());
		selectedTiles.Add(tile);
		OnAddEntity.RemoveAllListeners();
		OnAddEntity.AddListener((t) =>
		{
			TileSelect(t);
			PieceSelect(t);
		});

		OnAddEntity?.Invoke(tile);
	}

	public void DestroyEntity(VisualChessTableTile tile)
	{
		selectedPieces.Remove(tile.GetVisualPiece());
		selectedTiles.Remove(tile);
		OnDestroyEntity.RemoveAllListeners();
		OnDestroyEntity.AddListener((t) =>
		{
			TileDeselect(t);
			PieceDeselect(t.GetVisualPiece());
		});

		OnDestroyEntity?.Invoke(tile);
	}

	public void DestroyAllEntity()
	{
		List<VisualChessTableTile> list = selectedTiles;
		List<VisualChessPiece> piecelist = selectedPieces;

		for (int i = list.Count - 1; i >= 0; i--)
		{
			TileDeselect(list[i]);
			PieceDeselect(piecelist[i]);
			selectedTiles.RemoveAt(i);
			selectedPieces.RemoveAt(i);
		}
	}

	public void DestroyFirstEntity()
	{
		VisualChessTableTile tile1 = selectedTiles[0];
		VisualChessPiece piece1 = selectedPieces[0];
		selectedTiles.Remove(tile1);
		selectedPieces.Remove(piece1);
		OnDestroyEntity.RemoveAllListeners();
		OnDestroyEntity.AddListener((t) =>
		{
			TileDeselect(tile1);
			PieceDeselect(piece1);
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

	public void PieceAttributeChange()
	{
		List<VisualChessPiece> list = new List<VisualChessPiece>();

		for (int i = 0; i < selectedPieces.Count; i++)
		{
			list.Add(selectedPieces[i]);
		}
		skillLoader.ExecuteSkill(list);
		DisableImage();
	}

	public void PieceSelect(VisualChessTableTile tile)
	{
		SpriteRenderer render = tile.GetVisualPiece().Renderer as SpriteRenderer;
		render.material = Instantiate(OutlineMaterial);
		render.color = Color.white;
		render.material.SetTexture("_MainTex", render.sprite.texture);
		render.material.SetFloat("_OutlineThick", 1f);

		tile.GetVisualPiece().PieceSelect();
	}

	public void PieceDeselect(VisualChessPiece piece)
	{
		SpriteRenderer render = piece.Renderer as SpriteRenderer;
		render.material = Instantiate(OutlineMaterial);
		render.color = Color.white;
		render.material.SetTexture("_MainTex", render.sprite.texture);
		render.material.SetFloat("_OutlineThick", 0f);

		piece.PieceDeselect();
	}

	//탑 체인지 전용
	public bool IsMoveable(ChessTableTile tile)
	{
		TileIndex index = new TileIndex
		{
			x = 7 - tile.TileIndex.x,
			y = 7 - tile.TileIndex.y
		};

		ChessTableTile newTile = tile.Table.GetTile(index);

		if (newTile.GetPiece().Color != tile.GetPiece().Color) return true;
		else return false;
	}

}
