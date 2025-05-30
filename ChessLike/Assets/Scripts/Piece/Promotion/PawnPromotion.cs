using UnityEngine;
using DG.Tweening;

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
		PromotionUI.AddButtonEvent(this);
	}

	public void Promotion(int n)
	{
		Debug.Log(n);
		var piece = Instantiate(promotionObjects[n]);
		piece.transform.SetParent(FindObjectOfType<PieceSpawner>().transform, false);

		piece.transform.position = pieceData.transform.position;

		PieceData newPieceData = piece.GetComponent<PieceData>();
		PieceHandler newPIeceHandler = piece.GetComponent<PieceHandler>();
		Debug.Log("복사한 PieceData 가져오기");

		if (newPieceData == null || newPIeceHandler == null) return;

		//이전 기물의 상태와 동일하게 구성하기
		newPieceData.coordinate = pieceData.coordinate;
		newPieceData.IsPlayerPiece = pieceData.IsPlayerPiece;
		newPieceData.curTable = pieceData.curTable;

		newPIeceHandler.enabled = pieceHandler.enabled;
		newPIeceHandler.isDragable = pieceHandler.isDragable;
		newPIeceHandler.canvas = pieceHandler.canvas;


		newPieceData.IsStatic = pieceData.IsStatic;
		newPieceData.UpdateField();


		Destroy(gameObject);

		newPieceData.UpdateTableCoordinate();
		Debug.Log("상태 구성 완료");

		GameManager.instance.SortPieceSibling();

		PromotionUI.ClosePanel();
	}
}
