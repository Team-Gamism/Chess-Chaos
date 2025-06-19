using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChaosKnight : MonoBehaviour, ICardSkill
{
    private TableManager tableManager;
    private RectTransform rectTransform;

    private PieceSelector selector;

    private PieceHandler handler;
    [HideInInspector]
    public List<Vector2Int> moveOffsetList = new List<Vector2Int>
        {
        new Vector2Int(2, -1),
        new Vector2Int(2, 1),
        new Vector2Int(1, -2),
        new Vector2Int(1, 2),
        new Vector2Int(-1, -2),
        new Vector2Int(-1, 2),
        new Vector2Int(-2, -1),
        new Vector2Int(-2, 1),
        
        new Vector2Int(1, 0), new Vector2Int(-1, 0),
        new Vector2Int(0, 1), new Vector2Int(0, -1),
        new Vector2Int(1, 1), new Vector2Int(1, -1),
        new Vector2Int(-1, 1), new Vector2Int(-1, -1),

        new Vector2Int(0, 2), new Vector2Int(0, -2),   
        new Vector2Int(2, 0), new Vector2Int(-2, 0)
        };
    private void Start()
    {
        tableManager = FindObjectOfType<TableManager>();
        rectTransform = GetComponent<RectTransform>();

        selector = FindObjectOfType<PieceSelector>();
    }

    public void Execute()
    {
        GameManager.instance.ChaosKnight = true;
        selector.EnableImageAndCheckCoord(PieceType.Knight);
    }

    public void SetHandler(PieceHandler handler)
    {
        this.handler = handler;
    }


    public void Execute(PieceData piece)
    {
        Vector2Int coord = RandomOffset(handler.GetComponent<PieceData>());

        handler.MovePieceByCoordinate(coord);

        FindObjectOfType<PieceSelector>().DisableImage();
    }

    private Vector2Int RandomOffset(PieceData piece)
    {
        int n = 0;
        Vector2Int coord = Vector2Int.zero;
        do
        {
            n = Random.Range(0, moveOffsetList.Count-1);
            coord += piece.coordinate + moveOffsetList[n];

        } while (coord.x > 7 && coord.x < 0 &&
                coord.y > 7 && coord.y < 0);

        return coord;
    }
}
