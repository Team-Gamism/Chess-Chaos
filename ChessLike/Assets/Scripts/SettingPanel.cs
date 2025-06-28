using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private Toggle musicOff;
    [SerializeField]
    private Sprite[] musicSprites;
    [SerializeField]

    private Toggle sfxOff;
    [SerializeField]
    private Sprite[] sfxSprites;
    private void OnEnable()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        musicSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.AddListener((float value) =>
        {
            SoundManager.Instance.BGSoundVolume(value);
        });

        sfxSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.AddListener((float value) =>
        {
            SoundManager.Instance.SFXSoundVolume(value);
        });
    }

    public void SetMusicSlider()
    {
        Image image = musicOff.GetComponentInChildren<Image>();
        image.sprite = musicSprites[musicOff.isOn ? 1 : 0];

        SoundManager.Instance.IsSFXSound(musicOff.isOn ? 1 : 0);
        musicSlider.value = 0.0001f;

        musicSlider.interactable = musicOff.isOn;
    }

    public void SetSFXSlider()
    {
        Image image = sfxOff.GetComponentInChildren<Image>();
        image.sprite = sfxSprites[sfxOff.isOn ? 1 : 0];

        SoundManager.Instance.IsSFXSound(sfxOff.isOn ? 1 : 0);
        sfxSlider.value = 0.0001f;

        sfxSlider.interactable = sfxOff.isOn;
    }
}
