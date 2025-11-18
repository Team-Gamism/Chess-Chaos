using UnityEngine;
using DG.Tweening;

public class TileLock : TileAnimator
{
    public SpriteRenderer ShutterRenderer;
    public SpriteRenderer Lock;

    public override void EndSequence()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(Lock.DOFade(0f, 0.4f));
        seq.Append(transform.DOScaleY(0f, 0.5f).SetEase(Ease.OutQuint));
        seq.Join(transform.DOLocalMoveY(0.5f, 0.5f).SetEase(Ease.OutQuint));
        seq.Play();
    }

    public override void Sequence()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScaleY(1f, 0.5f).SetEase(Ease.OutQuint));
        seq.Join(transform.DOLocalMoveY(0f, 0.5f).SetEase(Ease.OutQuint));
        seq.Append(Lock.DOFade(1f, 0.4f));
        seq.Play();
    }

}
