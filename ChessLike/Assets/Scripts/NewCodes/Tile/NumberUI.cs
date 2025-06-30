using TMPro;
using UnityEngine;

public class NumberUI : MonoBehaviour
{
    private Canvas canvas;
    [SerializeField]
    private TMP_Text text;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        text.text = "";
    }
    public void SetText(int n)
    {
        text.text = n.ToString();
    }
}
