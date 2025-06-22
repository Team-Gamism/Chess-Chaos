using System.Linq;
using DG.Tweening;
using UnityEngine;

public class DimensionBreak : MonoBehaviour, ICardSkill
{
    private TableManager tableManager;
    private RectTransform rectTransform;
    [SerializeField]
    private MultiPieceSelector selector;

    public PieceData[] pieceData;
    private CardData cardData;
    private NCard ncard;
    private void Start()
    {
        tableManager = FindObjectOfType<TableManager>();
        rectTransform = GetComponent<RectTransform>();
        ncard = GetComponent<NCard>();
    }

    public void SetpieceData(PieceData pieceData)
    {
        selector.QueueManage(pieceData);
    }

    public void Execute()
    {
        GameManager.instance.DimensionBreak = true;
        cardData = GetComponent<NCard>().cardData;
        selector.cardData = cardData;
        selector.gameObject.SetActive(true);

        selector.EnableImage(false);
    }

    public void Execute(PieceData none)
    {
        pieceData = selector.ReturnPieces();

        MovePieces();

        GameManager.instance.DimensionBreak = false;
        ncard.DOEndAnimation();
    }
    private void MovePieces()
    {
        for (int i = 0; i < pieceData.Length; i++)
        {
            Vector2Int coord = tableManager.ReturnRandomCoordinate();
            Debug.Log(coord);
            pieceData[i].GetComponent<PieceHandler>().MovePieceByCoordinate(coord);
        }

        GameManager.instance.SortPieceSibling();
    }
}
