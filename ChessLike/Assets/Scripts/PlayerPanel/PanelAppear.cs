using DG.Tweening;
using UnityEngine;

public class PanelAppear : MonoBehaviour
{
    public DOTweenAnimation animation;
    public void Start()
    {
        animation.DOPlay();
    }
}
