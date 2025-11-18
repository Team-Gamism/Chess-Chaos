using ChessEngine.Game;
using UnityEngine;

public class ReturnVFX : MonoBehaviour
{
    VisualChessPiece piece;
    void Start()
    {
        piece = transform.parent.parent.GetComponent<VisualChessPiece>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!piece.Piece.IsEmerReturn)
        {
            piece.ReturnHandler(false);
        }
    }
}
