using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EndTween : MonoBehaviour
{
    public Image image;
    public UnityEvent SequenceComplete;

    private void OnEnable()
    {
        Sequence seq = DOTween.Sequence();
        image.color = new Color(0f, 0f, 0f, 0f);

        seq.PrependInterval(0.5f);
        seq.Append(image.GetComponent<RectTransform>().DOShakeAnchorPos(0.5f).OnStart(()=>{image.color = new Color(1f, 1f, 1f, 1f);}));
        seq.AppendInterval(0.3f);

        seq.OnComplete(() => { SequenceComplete?.Invoke(); });

        seq.Play();
    }
}
