using UnityEngine;

public class VisualTileState : MonoBehaviour
{
    private NumberUI textUI;

    private void Start()
    {
        textUI = GetComponent<NumberUI>();
    }

    public void SetNumber(int n)
    {
        textUI.SetText(n.ToString());
    }
    public void DeleteNumber()
    {
        textUI.DeleteText();
    }
}
