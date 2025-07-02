using DG.Tweening;
using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    public SpriteRenderer Renderer;
    [Header("사슬 효과")]
    public GameObject ChainObj;
    public Transform[] chains;
    public Transform[] FromPos;
    public Transform[] EndPos;
    public SpriteMask mask;
    [Header("방어막 효과")]
    public GameObject obj1;

    public void ChainEffectOn()
    {
        Debug.Log("호출됨");
        ChainObj.SetActive(true);
        mask.sprite = Renderer.sprite;
        for (int i = 0; i < chains.Length; i++)
        {
            chains[i].position = FromPos[i].position;
            chains[i].DOMove(EndPos[i].position, 1f).SetEase(Ease.OutQuint);
        }
        
    }

    public void ChainEffectOff()
    {
        chains[0].DOMove(FromPos[0].position, 1f).SetEase(Ease.OutQuint);
        chains[1].DOMove(FromPos[1].position, 1f).SetEase(Ease.OutQuint).OnComplete(() => { ChainObj.SetActive(false); });
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
