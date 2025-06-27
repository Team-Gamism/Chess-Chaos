using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileSpriter : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    private SpriteRenderer sr;
    private UnityEvent OnSpriteAppear = new UnityEvent();
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = 100;
    }
    public void SpriteOn(HighlightType type)
    {
        sr.sprite = sprites[(int)type];

        OnSpriteAppear.AddListener(AppearAnimation);

        OnSpriteAppear?.Invoke();
    }
    public void SpriteOff()
    {
        OnSpriteAppear.RemoveAllListeners();

        sr.sprite = null;
    }

    private void AppearAnimation()
    {
        transform.localScale = Vector3.one * 0.5f;
        transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutQuint);
    }
}

public enum HighlightType
{
    Select = 0,
    Move,
    Attack
}
