using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SkinSetter : MonoBehaviour
{
    public SpriteAtlas altas;
    public SkinPiece[] skinPieces;
    public bool isUsing;
    public bool isWhite;
    public SkinManager skinManager;

    private Toggle colorToggle;

    private void Start()
    {
        colorToggle = skinManager.colorToggle;
    }
    public void SetSprites(SpriteAtlas newAtlas, int n)
    {
        altas = newAtlas;

        skinManager.SetOutlineColor(n);

        for (int i = 0; i < skinPieces.Length; i++)
        {
            // skinPiece에 이미지를 적용
            Sprite sprite = AtlasManager.instance.GetSkinSprite(
                altas, isWhite, skinPieces[i].piecetype);
            skinPieces[i].UpdateImage(sprite);
        }
        UpdateSkinSetter();
    }


    public void UpdateSkinSetter()
    {
        isUsing = isAtlasUsing();
    }
    private bool isAtlasUsing()
    {
        return AtlasManager.instance.curSkin.Equals(altas);
    }

    private void Update()
    {
        isWhite = colorToggle.isOn;
        
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        for (int i = 0; i < skinPieces.Length; i++)
        {
            // skinPiece에 이미지를 적용
            Sprite sprite = AtlasManager.instance.GetSkinSprite(
                altas, isWhite, skinPieces[i].piecetype);
            skinPieces[i].UpdateImage(sprite);
        }
    }
}
