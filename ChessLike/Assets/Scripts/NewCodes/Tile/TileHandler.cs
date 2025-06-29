using ChessEngine.Game;
using UnityEngine;
using DG.Tweening;

public class TileHandler : MonoBehaviour
{
    public VisualChessTableTile Tile;
    private VisualChessPiece piece;
    private TileSpriter spriter;
    private void Start()
    {
        spriter = GetComponentInChildren<TileSpriter>();
    }

    public void OnHighlight()
    {
        spriter.SpriteOn(HighlightType.Select);
    }
    public void OnMoveHighlight()
    {
        spriter.SpriteOn(HighlightType.Move);
    }
    public void OnAttackHighlight()
    {
        spriter.SpriteOn(HighlightType.Attack);
    }
    public void OnTileBlock()
    {
        spriter.SpriteOn(HighlightType.NotEnter);
    }
    public void OnUnhilight()
    {
        spriter.SpriteOff();
    }
}
