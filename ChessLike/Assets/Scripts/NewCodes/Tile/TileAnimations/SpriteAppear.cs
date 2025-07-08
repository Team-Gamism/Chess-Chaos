using UnityEngine;
using DG.Tweening;

public class SpriteAppear : TileAnimator
{
    public Sprite sprite;
    public SpriteRenderer Renderer;
    public override void EndSequence()
    {
        Renderer.sprite = null;
    }

    public override void Sequence()
    {
        Renderer.sprite = sprite;
        Renderer.gameObject.transform.localScale = Vector3.one * 0.5f;
        Renderer.gameObject.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutQuint);
    }

}
