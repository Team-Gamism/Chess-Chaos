using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CheckmateIconTween : MonoBehaviour
{
    public Image image;
    void OnEnable()
    {
        image.color = Color.white;

        Sequence seq = DOTween.Sequence();
        seq.Append(image.DOFade(1f, 0.1f));
        seq.Append(image.DOFade(0f, 1f));

        seq.Play();
    }

    public void DisableThis()
    {
        image.color = Color.red;
        Sequence seq = DOTween.Sequence();
        seq.Append(image.DOFade(0.7f, 0.1f));
        seq.Append(image.DOFade(0f, 1f));

        seq.Play();
    }
}

