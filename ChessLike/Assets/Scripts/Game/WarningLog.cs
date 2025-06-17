using TMPro;
using UnityEngine;

public class WarningLog : MonoBehaviour
{
    public string log;
    private TMP_Text logText;
    private TweenFader tweenFader;
    private void OnEnable()
    {
        tweenFader = GetComponent<TweenFader>();
        logText = GetComponentInChildren<TMP_Text>();
        logText.text = log;
    }
}
