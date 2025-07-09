using TMPro;
using UnityEngine;

public class IconUI : MonoBehaviour
{
    public Canvas canvas;
    private void Start()
    {
        canvas.worldCamera = Camera.main;
    }
    public void SetRotation()
    {
        canvas.transform.parent.localEulerAngles = new Vector3(0f, 180f, 0f);
        Debug.Log(canvas.transform.parent.rotation.y);
    }
}
