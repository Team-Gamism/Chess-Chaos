using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    private SkinLoader skinLoader;
    private SkinSetter[] skinSetters = new SkinSetter[20];
    [SerializeField]
    private GameObject Setter;
    public Outline outline;
    public Toggle colorToggle;
    private int currentIdx = 0;

    private void Start()
    {
        skinLoader = GetComponentInParent<SkinLoader>();

        for (int i = 0; i < skinLoader.atlasList.Count; i++)
        {
            var setter = Instantiate(Setter);
            setter.transform.SetParent(gameObject.transform, false);

            skinSetters[i] = setter.GetComponent<SkinSetter>();
            skinSetters[i].SetSprites(skinLoader.atlasList[i]);
        }
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
            skinSetters[i].UpdateSkinSetter();
        }
    }
}
