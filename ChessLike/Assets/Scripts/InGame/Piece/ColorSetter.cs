using ChessEngine;
using ChessEngine.Game;
using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    private VisualChessPiece chessPiece;
    [SerializeField]
    private PieceType piece;

    [SerializeField]
    private Material White;

    [SerializeField]
    private SpriteRenderer randerer;
    public void ColorChange()
    {
        chessPiece = gameObject.GetComponent<VisualChessPiece>();

        Change(chessPiece.Piece.Color == ChessColor.White);
    }

    private void Change(bool value)
    {
        if (value)
        {
            randerer.sprite = AtlasManager.instance.GetCurrentSkinSprite(true, piece);
        }
        else
        {
            randerer.sprite = AtlasManager.instance.GetCurrentSkinSprite(false, piece);
        }
    }
}
