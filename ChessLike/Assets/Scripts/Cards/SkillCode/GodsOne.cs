using System.Linq;
using UnityEngine;

public class GodsOne : MonoBehaviour, ICardSkill
{
    private TableManager tableManager;
    private RectTransform rectTransform;
    [SerializeField]
    private MultiPieceSelector selector;

    public PieceData pieceData;

    public GameObject Queen;

    public PieceType[] EnablePieces;

    private CardData cardData;
    private void Start()
    {
        tableManager = FindObjectOfType<TableManager>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetpieceData(PieceData pieceData)
    {
        this.pieceData = pieceData;
        selector.QueueManage(pieceData);
    }

    public void Execute()
    {
        GameManager.instance.GodsOne = true;
        cardData = GetComponent<NCard>().cardData;
        selector.cardData = cardData;
        selector.gameObject.SetActive(true);

        selector.EnableImage(EnablePieces);
    }

    public void Execute(PieceData none)
    {
        pieceData = selector.ReturnPiece();

        Change(pieceData);

        GameManager.instance.GodsOne = false;
    }
    
    public void Change(PieceData p)
	{
		Vector3 SpawnPos = p.transform.position;
		Vector2Int coord = p.coordinate;
		bool isPlayerPiece = p.IsPlayerPiece;
		TableData cutTable = p.curTable;

		Canvas canvas = p.GetComponent<PieceHandler>().canvas;
		bool isDraggable = p.GetComponent<PieceHandler>().canvas;

		var piece = Instantiate(Queen);
		piece.transform.SetParent(FindObjectOfType<PieceSpawner>().transform, false);
		piece.transform.position = SpawnPos;

		piece.transform.position = pieceData.transform.position;

		PieceData newPieceData = piece.GetComponent<PieceData>();
		PieceHandler newPIeceHandler = piece.GetComponent<PieceHandler>();
		Debug.Log("복사한 PieceData 가져오기");

		if (newPieceData == null || newPIeceHandler == null) return;

		newPieceData.coordinate = coord;
		newPieceData.IsPlayerPiece = isPlayerPiece;
		newPieceData.curTable = newPieceData.GetComponent<TableData>();

		newPIeceHandler.enabled = p.GetComponent<PieceHandler>().enabled;
		newPIeceHandler.isDragable = p.GetComponent<PieceHandler>().isDragable;
		newPIeceHandler.canvas = p.GetComponent<PieceHandler>().canvas;


		newPieceData.IsStatic = p.IsStatic;
		newPieceData.UpdateField();
		
		newPieceData.SetSpriteColor();
		Destroy(p.gameObject);

		newPieceData.UpdateTableCoordinate();
		Debug.Log("상태 구성 완료");

        GameManager.instance.GodsOne = false;

		GameManager.instance.SortPieceSibling();
	}

}
