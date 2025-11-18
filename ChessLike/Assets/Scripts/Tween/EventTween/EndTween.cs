using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EndTween : MonoBehaviour
{
    public Image image;
    public UnityEvent SequenceComplete;
    public AudioSource BGM;

    private void OnEnable()
    {
        Sequence seq = DOTween.Sequence();
        image.color = new Color(0f, 0f, 0f, 0f);

        BGM.Stop();

        seq.Append(image.GetComponent<RectTransform>().DOShakeAnchorPos(0.5f).OnStart(() => { image.color = new Color(1f, 1f, 1f, 1f); }));
        seq.AppendInterval(0.7f);

        seq.OnComplete(() => { SequenceComplete?.Invoke(); });

        seq.Play();
    }
}
