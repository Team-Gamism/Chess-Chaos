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
	private WarningLog warningLog;

	[SerializeField]
	private Button DoneBtn;

	private Image image;

	[HideInInspector]
	public bool Donable = false;
	[Header("Sound")]
	public AudioClip Select;
	public AudioClip Deselect;
	[Header("list")]

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

		IsUse(type);
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
		SoundManager.Instance.SFXPlay("Deselect", Deselect);
		tile.GetComponent<TileHandler>().OnFrontHighlight();
	}
	public void TileDeselect(VisualChessTableTile tile)
	{
		SoundManager.Instance.SFXPlay("Deselect", Deselect);
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
		if (newTile == null) return false; // 좌표가 범위 밖일 경우

		var targetPiece = newTile.GetPiece();
		var sourcePiece = tile.GetPiece();

		// 타겟이 비어 있으면 이동 가능
		if (targetPiece == null) return true;

		// 적 진영의 말이면 이동 가능
		if (targetPiece.Color != sourcePiece.Color)
		{
			// 단, 킹이면 이동 불가 (킹을 보호하려는 목적이면)
			if (targetPiece.GetChessPieceType() == ChessPieceType.King)
				return false;

			return true;
		}

		// 나와 같은 색 말이 있으면 이동 불가
		return false;
	}


	public void IsUse(PieceSkillType type)
	{
		switch (type)
		{
			case PieceSkillType.TopChange:
				if (FindObjectsOfType<VisualChessPiece>().Where(v => v.Piece.GetChessPieceType() == ChessPieceType.Rook &&
				v.Piece.Color == SelectColor).Count() <= 0)
				{
					warningLog.log = "기물이 존재하지 않습니다!";
					warningLog.gameObject.SetActive(true);
					DisableImage();
				}
				break;
			case PieceSkillType.ChaosKnight:
				if (FindObjectsOfType<VisualChessPiece>().Where(v => v.Piece.GetChessPieceType() == ChessPieceType.Knight &&
				v.Piece.Color == SelectColor).Count() <= 0)
				{
					warningLog.log = "기물이 존재하지 않습니다!";
					warningLog.gameObject.SetActive(true);
					DisableImage();
				}
				break;
			case PieceSkillType.DimensionBreak:
				if (FindObjectsOfType<VisualChessPiece>().Where(v => v.Piece.Color == SelectColor &&
				v.Piece.GetChessPieceType() != ChessPieceType.King).Count() < 2)
				{
					warningLog.log = "상대 기물이 부족합니다!";
					warningLog.gameObject.SetActive(true);
					DisableImage();
				}
				break;
		}
	}

}
