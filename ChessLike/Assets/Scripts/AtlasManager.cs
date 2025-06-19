using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager : MonoBehaviour
{
    public static AtlasManager instance;
    public static event Action OnChangeSkin;

    [SerializedDictionary("이름", "아틀라스")]
    public SerializedDictionary<String, SpriteAtlas> SkinDictionary = new SerializedDictionary<string, SpriteAtlas>();

    public SpriteAtlas curSkin { get { return currentSkin; } }
    private SpriteAtlas currentSkin;
    private String currentSkinName;
    public int skinIdx = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (PlayerPrefs.HasKey("currentSkin") && PlayerPrefs.HasKey("skinIdx"))
            {
                currentSkinName = LoadCurrentSkin();
                Debug.Log(currentSkinName);
                skinIdx = LoadSkinIdx();
            }
            else
            {
                currentSkinName = "Normal";
                skinIdx = 0;
            }
            SkinDictionary.TryGetValue(currentSkinName, out currentSkin);
        }
        else
        {
            Destroy(this);
        }
    }


    public void ChangeSkin(int n)
    {
        var list = SkinDictionary.ToList();
        currentSkin = list[n].Value;
        currentSkinName = list[n].Key;

        SaveCurrentSkin();
        SetSkinIdx(n);

        OnChangeSkin?.Invoke();
    }

    private void SaveCurrentSkin()
    {
        PlayerPrefs.SetString("currentSkin", currentSkinName);
        PlayerPrefs.Save();
    }
    private String LoadCurrentSkin()
    {
        return PlayerPrefs.GetString("currentSkin");
    }

    public void SetSkinIdx(int n)
    {
        skinIdx = n;
        PlayerPrefs.SetInt("skinIdx", skinIdx);
        PlayerPrefs.Save();
    }
    private int LoadSkinIdx()
    {
        return PlayerPrefs.GetInt("skinIdx");
    }

    public void SetParticleTexture(ParticleSystem particleSystem)
    {
        if (particleSystem == null)
        {
            Debug.LogError("SetParticleTexture: particleSystem이 null입니다.");
            return;
        }

        if (currentSkin == null)
        {
            Debug.LogError("SetParticleTexture: currentSkin이 null입니다.");
            return;
        }

        var tsa = particleSystem.textureSheetAnimation;
        int spriteSlotCount = tsa.spriteCount;

        for (int i = 0; i < currentSkin.spriteCount; i++)
        {
            string spriteName = (i > 5) ? $"B_{i % 6}" : $"W_{i % 6}";
            Sprite sprite = currentSkin.GetSprite(spriteName);

            if (sprite == null)
            {
                Debug.LogWarning($"SetParticleTexture: 스프라이트 '{spriteName}'가 currentSkin에 없습니다.");
                continue;
            }

            if (i >= tsa.spriteCount)
            {
                tsa.AddSprite(sprite); // 자동으로 슬롯 확장
            }
            else
            {
                tsa.SetSprite(i, sprite);
            }
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

    /// <summary>
    /// 지정한 스킨에서 스프라이트를 가져옴
    /// </summary>
    /// <param name="color">falsle = 검정, true = 흰색</param>
    /// <param name="piece">기물 타입</param>
    /// <returns></returns>
    public Sprite GetSkinSprite(SpriteAtlas atlas, bool color, PieceType piece)
    {
        if (color)
        {
            return atlas.GetSprite("W_" + (int)piece);
        }
        else
        {
            return atlas.GetSprite("B_" + (int)piece);
        }
    }

    public List<SpriteAtlas> GetAllSkins()
    {
        List<SpriteAtlas> list = new List<SpriteAtlas>();

        for (int i = 0; i < SkinDictionary.Count; i++)
        {
            list.Add(SkinDictionary.ElementAt(i).Value);
        }

        return list;
    }
}
