using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SkinLoader : MonoBehaviour
{
    public List<SpriteAtlas> atlasList = new List<SpriteAtlas>();
    private SkinManager skinManager;

    private void Start()
    {
        skinManager = GetComponentInChildren<SkinManager>();
        LoadAtlas();
    }
    private void LoadAtlas()
    {
        atlasList = AtlasManager.instance.GetAllSkins();
        skinManager.Init();
    }

    public SpriteAtlas GetAtalsAt(int n) { return atlasList[n]; }

}
