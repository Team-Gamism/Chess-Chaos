using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
    [Header("초기 입력값")]
    public Sprite sprite;
    public String title;

    [Header("적용 컴포넌트")]
    public PlayerProfile profile;
    private TMP_Text tmp;
    private Image image;
    public UnityEvent OnIconUpdated = new UnityEvent();

    private void Start()
    {
        tmp = GetComponentInChildren<TMP_Text>();
        image = GetComponent<Image>();

        tmp.text = title;
        image.sprite = sprite;
    }

    public void ApplySprite()
    {
        PlayerManager.instance.PlayerIcon = sprite;
        profile.UpdateUI();
        OnIconUpdated?.Invoke();
    }
}
