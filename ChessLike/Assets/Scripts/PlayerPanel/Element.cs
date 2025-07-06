using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Element : MonoBehaviour
{
    public Image image;
    public TMP_Text text;
    void OnEnable()
    {

    }
    private void LoadData(BuffInfo info)
    {

    }
    public void MoveElement(float y)
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(y, 3f).SetEase(Ease.Linear);
    }
}
