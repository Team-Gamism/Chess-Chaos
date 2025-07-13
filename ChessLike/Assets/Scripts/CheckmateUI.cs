using TMPro;
using UnityEngine;

public class CheckmateUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public CanvasGroup group { get { return canvasGroup; } set{ value = canvasGroup; } } 
    public TMP_Text numberText;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void UpdateUI(int num)
    {
        if (num <= 0)
        {
            canvasGroup.alpha = 0f;
            numberText.text = "";
        }
        else
        {
            numberText.text = num.ToString();
        }
    }
}
