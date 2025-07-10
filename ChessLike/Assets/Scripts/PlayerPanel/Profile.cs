using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    public Image Image;
    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        Image.sprite = PlayerManager.instance.PlayerIcon;
    }
}
