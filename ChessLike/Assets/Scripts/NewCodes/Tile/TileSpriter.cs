using System;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriter : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    private SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = 100;
    }
    public void SpriteOn(HighlightType type)
    {
        sr.sprite = sprites[(int)type];
    }
    public void SpriteOff()
    {
        sr.sprite = null;
    }
}

public enum HighlightType
{
    Select = 0,
    Move,
    Attack
}
