using ChessEngine.Game;
using UnityEngine;

public class VFXAnimation : MonoBehaviour
{
    VisualChessPiece piece;
    void Start()
    {
        piece = transform.parent.parent.GetComponent<VisualChessPiece>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!piece.Piece.IsShield)
        {
            piece.ShieldHandler(false);
        }
    }
}
