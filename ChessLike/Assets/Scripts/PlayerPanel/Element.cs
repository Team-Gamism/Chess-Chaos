using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Element : MonoBehaviour
{
    public Image image;
    public TMP_Text text;
    public void MoveElement(float y)
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(y, 3f).SetEase(Ease.Linear);
    }
}
