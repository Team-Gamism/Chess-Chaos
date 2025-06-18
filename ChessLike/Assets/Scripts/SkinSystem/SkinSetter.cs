using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SkinSetter : MonoBehaviour
{
    public SpriteAtlas altas;
    public SkinPiece[] skinPieces;
    public bool isUsing;
    public bool isWhite;
    private Image image;
    private Toggle colorToggle;

    private void Start()
    {
        image = GetComponent<Image>();
        colorToggle = GetComponentInParent<SkinManager>().colorToggle;
    }
    public void SetSprites(SpriteAtlas newAtlas)
    {
        altas = newAtlas;

        for (int i = 0; i < skinPieces.Length; i++)
        {
            // skinPiece에 이미지를 적용
            Sprite sprite = AtlasManager.instance.GetSkinSprite(
                altas, isWhite, skinPieces[i].piecetype);
            skinPieces[i].UpdateImage(sprite);
        }
        UpdateSkinSetter();
        if (!isUsing) gameObject.SetActive(false);
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

        if (isUsing)
        {
            GetComponentInParent<SkinManager>().outline.effectColor = new Color(136f / 255f, 1f, 1f, 1f);
        }
        else
        {
            GetComponentInParent<SkinManager>().outline.effectColor = Color.white;
        }
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
