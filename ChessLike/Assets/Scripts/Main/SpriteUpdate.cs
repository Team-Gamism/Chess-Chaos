using UnityEngine;
using UnityEngine.UI;

public class SpriteUpdate : MonoBehaviour
{
    private Image image;

    [SerializeField]
    private bool PlayerPiece;
    [SerializeField]
    private PieceType piece;
    void Start()
    {
        image = GetComponent<Image>();
        AtlasManager.OnChangeSkin += UpdateSprite;
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        image.sprite = AtlasManager.instance.GetCurrentSkinSprite(PlayerPiece, piece);
    }
}
