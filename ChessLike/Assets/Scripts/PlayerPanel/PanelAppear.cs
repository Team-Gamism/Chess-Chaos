using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class PanelAppear : MonoBehaviour
{
    public DOTweenAnimation animation;
    public void Start()
    {
        animation.DOPlay();
    }
}
