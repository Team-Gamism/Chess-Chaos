using ChessEngine.Game;
using UnityEngine;
using UnityEngine.Events;

public class TileState : MonoBehaviour
{
    [HideInInspector]
    public VisualChessTableTile tile;
    private int TileBlockCnt = 0;
    private int MaxTileBlockCnt;
    [SerializeField]
    private VisualTileState visualTileState;
    private void Start()
    {
        tile = GetComponent<VisualChessTableTile>();
    }

    public void SetTileState(int MaxTileBlockCnt)
    {
        if (tile.isTileblock)
            this.MaxTileBlockCnt = MaxTileBlockCnt;
        visualTileState.SetNumber(MaxTileBlockCnt);
    }
    public void UpdateTileState()
    {
        if (tile.isTileblock)
        {
            TileBlockCnt++;
            CheckTileState();

            if (MaxTileBlockCnt - TileBlockCnt <= 0)
            {
                visualTileState.DeleteNumber();
            }
            else
            {
                visualTileState.SetNumber(MaxTileBlockCnt - TileBlockCnt);
            }
        }
    }

    private void CheckTileState()
    {
        if (TileBlockCnt >= MaxTileBlockCnt)
        {
            tile.isTileblock = false;

            tile.CloseTileBlock();

            TileBlockCnt = 0;
            MaxTileBlockCnt = 0;
        }
    }
}
