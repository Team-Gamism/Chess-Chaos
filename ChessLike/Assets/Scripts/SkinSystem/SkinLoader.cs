using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SkinLoader : MonoBehaviour
{
    public List<SpriteAtlas> atlasList = new List<SpriteAtlas>();
    private void Start()
    {
        LoadAtlas();
    }
    private void LoadAtlas() => atlasList = AtlasManager.instance.GetAllSkins();

    public SpriteAtlas GetAtalsAt(int n) { return atlasList[n]; }

}
