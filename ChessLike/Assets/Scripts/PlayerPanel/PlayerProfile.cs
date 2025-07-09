using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
    public Image Profile;
    public TMP_Text Name;
    void Start()
    {
        Profile.sprite = PlayerManager.instance.PlayerIcon;
        Name.text = PlayerManager.instance.PlayerName;
    }
}
