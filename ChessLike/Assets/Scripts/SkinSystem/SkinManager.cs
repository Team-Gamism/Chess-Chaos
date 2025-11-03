using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    private SkinLoader skinLoader;
    private SkinSetter[] skinSetters = new SkinSetter[20];
    private int currentIdx;

    [SerializeField] private GameObject Setter;
    [SerializeField] private TMP_Text Title;

    public Outline outline;
    public Toggle colorToggle;
    public Button ChooseBtn;

    public void Init()
    {
        currentIdx = AtlasManager.instance.skinIdx;
        skinLoader = GetComponentInParent<SkinLoader>();

        for (int i = 0; i < skinLoader.atlasList.Count; i++)
        {
            var setter = Instantiate(Setter);
            setter.transform.SetParent(gameObject.transform, false);

            skinSetters[i] = setter.GetComponent<SkinSetter>();
            skinSetters[i].SetSprites(skinLoader.atlasList[i]);
        }

        SetTitle();
        Title.gameObject.transform.SetAsLastSibling();
    }

    public void ChangeCurSkin()
    {
        AtlasManager.instance.ChangeSkin(currentIdx);
        UpdateSkinSetter();
    }


    private void UpdateSkinSetter()
    {
        for (int i = 0; i < skinSetters.Length; i++)
        {
            if (!skinSetters[i]) break;
            skinSetters[i].UpdateSkinSetter();
        }
    }
    private void SetSkinSetter(bool value)
    {
        skinSetters[currentIdx].gameObject.SetActive(value);
    }

    public void MoveLeft()
    {
        SetSkinSetter(false);
        currentIdx = Mathf.Clamp(--currentIdx, 0, skinLoader.atlasList.Count - 1);
        SetSkinSetter(true);
        SetTitle();
    }
    public void MoveRight()
    {
        SetSkinSetter(false);
        currentIdx = Mathf.Clamp(++currentIdx, 0, skinLoader.atlasList.Count - 1);
        SetSkinSetter(true);
        SetTitle();
    }
    private void SetTitle()
    {
        List<String> strings = AtlasManager.instance.SkinDictionary.Keys.ToList();
        Title.text = strings[currentIdx];
    }
}
