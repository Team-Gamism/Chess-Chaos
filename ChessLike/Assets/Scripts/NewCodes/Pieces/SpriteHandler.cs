using DG.Tweening;
using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    public SpriteRenderer Renderer;
    [Header("사슬 효과")]
    public GameObject ChainObj;
    public SpriteMask mask;
    [Header("방어막 효과")]
    public GameObject obj1;

    public void ChainEffectOn()
    {
        Debug.Log("호출됨");
        ChainObj.SetActive(true);
        mask.sprite = Renderer.sprite;
    }

    public void ChainEffectOff()
    {
        ChainObj.SetActive(false);
    }

    public void ShieldEffectOn()
    {
        obj1.SetActive(true);
    }
    public void ShieldEffectOff()
    {
        obj1.SetActive(false);
    }
}
