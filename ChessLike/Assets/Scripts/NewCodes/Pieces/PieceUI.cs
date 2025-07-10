using TMPro;
using UnityEngine;

public class PieceUI : MonoBehaviour
{
    public Canvas canvas;
    public TMP_Text number;
    private void Start()
    {
        canvas.worldCamera = Camera.main;
    }
    public void SetNumber(int n)
    {
        if (n <= 0)
            number.text = "";
        else
            number.text = n.ToString();
    }
    public void SetRotation()
    {
        //canvas.transform.parent.localEulerAngles = new Vector3(0f, 180f, 0f);
    }
}
