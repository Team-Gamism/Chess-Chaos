using ChessEngine.Game;
using UnityEngine;

public class ReturnVFX : MonoBehaviour
{
    VisualChessPiece piece;
    void Start()
    {
        //세상에서 제일 ㅄ같은 코드
        piece = transform.parent.parent.parent.GetComponent<VisualChessPiece>(); 
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
