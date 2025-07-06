using UnityEngine;

public class PawnPromotion : MonoBehaviour
{
	public GameObject[] promotionObjects = new GameObject[4];

	private PieceData pieceData;
	private PieceHandler pieceHandler;
	private PromotionUIManager PromotionUI;


	private void Start()
	{
		pieceData = GetComponent<PieceData>();
		pieceHandler = GetComponent<PieceHandler>();
		PromotionUI = FindObjectOfType<PromotionUIManager>();

	}

	public void StartPromotion()
	{
		Debug.Log("프로모션 시작");
	}

	public void Promotion(int n)
	{
		if (pieceData == null) return;

		Vector3 SpawnPos = pieceData.transform.position;
		Vector2Int coord = pieceData.coordinate;
		bool isPlayerPiece = pieceData.IsPlayerPiece;
		TableData cutTable = pieceData.curTable;

		Canvas canvas = pieceHandler.canvas;
		bool isDraggable = pieceHandler.isDragable;

		var piece = Instantiate(promotionObjects[n]);
		piece.transform.SetParent(FindObjectOfType<PieceSpawner>().transform, false);
		piece.transform.position = SpawnPos;

		piece.transform.position = pieceData.transform.position;
		piece.GetComponent<PieceData>().SetOutlineMaterial();
		piece.GetComponent<PieceData>().DownAnimation = false;

		PieceData newPieceData = piece.GetComponent<PieceData>();
		PieceHandler newPIeceHandler = piece.GetComponent<PieceHandler>();

		Debug.Log("복사한 PieceData 가져오기");

		if (newPieceData == null || newPIeceHandler == null) return;

		//이전 기물의 상태와 동일하게 구성하기
		newPieceData.coordinate = coord;
		newPieceData.IsPlayerPiece = isPlayerPiece;
		newPieceData.curTable = newPieceData.GetComponent<TableData>();

		newPIeceHandler.enabled = pieceHandler.enabled;
		newPIeceHandler.isDragable = pieceHandler.isDragable;
		newPIeceHandler.canvas = pieceHandler.canvas;


		newPieceData.IsStatic = pieceData.IsStatic;
		newPieceData.UpdateField();
		
		newPieceData.SetSpriteColor();

		//기존 폰 삭제
		Destroy(gameObject);

		newPieceData.UpdateTableCoordinate();
		Debug.Log("상태 구성 완료");

		//프로모션 상태 비활성화
		GameManager.instance.IsPromotion = false;

		GameManager.instance.SortPieceSibling();

		//PromotionUI.ClosePanel();
	}
}
