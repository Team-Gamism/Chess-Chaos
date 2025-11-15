using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    private SkinLoader skinLoader;
    private Image outline;
    private int currentIdx;

    [SerializeField] private SkinSetter skinSetter;
    [SerializeField] private TMP_Text Title;

    public Toggle colorToggle;
    public Button ChooseBtn;

    List<String> strings;

    private int selectedIndex;

	public void Init()
    {
        currentIdx = PlayerPrefs.GetInt("skinIdx");
		strings = AtlasManager.instance.SkinDictionary.Keys.ToList();

		outline = GetComponent<Image>();
        skinLoader = GetComponentInParent<SkinLoader>();

        skinSetter.SetSprites(skinLoader.atlasList[0], 0);

        SetTitle();
        Title.gameObject.transform.SetAsLastSibling();
    }

    public void ChangeCurSkin()
    {
        skinSetter.SetSprites(skinLoader.atlasList[currentIdx], currentIdx);
        skinSetter.UpdateSkinSetter();
    }

    public void SetOutlineColor(int idx)
    {
        if(idx == selectedIndex)
            outline.color = Color.white;
        else
            outline.color = Color.black;
    }


    public void MoveLeft()
    {
        currentIdx = Mathf.Clamp(--currentIdx, 0, skinLoader.atlasList.Count - 1);
        ChangeCurSkin();
        SetTitle();
    }
    public void MoveRight()
    {
        currentIdx = Mathf.Clamp(++currentIdx, 0, skinLoader.atlasList.Count - 1);
        ChangeCurSkin();
        SetTitle();
    }
    private void SetTitle()
    {
        Title.text = strings[currentIdx];
    }
}
