using ChessEngine.Game;
using UnityEngine;
using DG.Tweening;

public class TileHandler : MonoBehaviour
{
    public VisualChessTableTile Tile;
    private VisualChessPiece piece;
    public TileSpriter spriter;
    public TileSpriter frontSpriter;

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

    public void OnFrontHighlight()
    {
        frontSpriter.SpriteOn(HighlightType.Select);
    }
    public void OnFrontUnhilight()
    {
        frontSpriter.SpriteOff();
    }
}
