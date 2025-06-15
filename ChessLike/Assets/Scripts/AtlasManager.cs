using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager : MonoBehaviour
{
    public static AtlasManager instance;

    [SerializedDictionary("이름", "아틀라스")]
    public SerializedDictionary<String, SpriteAtlas> SkinDictionary = new SerializedDictionary<string, SpriteAtlas>();

    private SpriteAtlas currentSkin;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (currentSkin == null)
        {
            currentSkin = SkinDictionary["Normal"];
        }
    }

    public void ChangeCurrentSkin()
    {
        
    }

    public void SetParticleTexture(ParticleSystem particleSystem)
    {
        for (int i = 0; i < currentSkin.spriteCount; i++)
        {
            if (i > 5)
                particleSystem.textureSheetAnimation.SetSprite(i, currentSkin.GetSprite("B_" + (i % 6)));
            else
                particleSystem.textureSheetAnimation.SetSprite(i, currentSkin.GetSprite("W_" + (i % 6)));
        }
    }

    /// <summary>
    /// 현재 스킨에서 스프라이트를 가져옴
    /// </summary>
    /// <param name="color">falsle = 검정, true = 흰색</param>
    /// <param name="piece">기물 타입</param>
    /// <returns></returns>
    public Sprite GetCurrentSkinSprite(bool color, PieceType piece)
    {
        if (color)
        {
            return currentSkin.GetSprite("W_" + (int)piece);
        }
        else
        {
            return currentSkin.GetSprite("B_" + (int)piece);
        }
    }
}
