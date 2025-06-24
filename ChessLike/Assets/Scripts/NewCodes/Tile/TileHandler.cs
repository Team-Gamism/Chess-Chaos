using ChessEngine.Game;
using UnityEngine;
using DG.Tweening;

public class TileHandler : MonoBehaviour
{
    public VisualChessTableTile Tile;
    public float yValue = 0.5f;
    private VisualChessPiece piece;

    public void OnHighlight()
    {
        piece = Tile.GetVisualPiece();
        Vector3 endPoint = piece.gameObject.transform.position;
        piece.gameObject.transform.DOMoveY(endPoint.y + yValue, 0.3f).SetEase(Ease.OutQuint);
    }
}
