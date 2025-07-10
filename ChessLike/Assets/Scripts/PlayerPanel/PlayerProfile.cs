using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
    public Image Profile;
    public TMP_Text Name;
    public GameObject IconPanel;
    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        Profile.sprite = PlayerManager.instance.PlayerIcon;
        Name.text = PlayerManager.instance.PlayerName;
    }
    public void IconPanelTrigger(bool value)
    {
        IconPanel.SetActive(value);
    }
}
