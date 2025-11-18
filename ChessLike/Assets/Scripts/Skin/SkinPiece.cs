using UnityEngine;
using UnityEngine.UI;

public class SkinPiece : MonoBehaviour
{
    private Image image;
    public PieceType piecetype;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void UpdateImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
