using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    public SpriteRenderer Renderer;
    [Header("고정 효과")]
    public SpriteRenderer locker;
    public SpriteRenderer shade;
    [Header("방어막 효과")]
    public GameObject obj1;
    [Header("복수 효과")]
    public GameObject Effect;
    [Header("고정 효과")]
    public PieceUI pieceUI;
    [Header("복귀 효과")]
    public GameObject obj2;
    public IconUI iconUI;
    [Header("사선 이동")]
    public GameObject obj3;
    [Header("두칸 이동")]
    public GameObject obj4;
    [Header("암습의 폰")]
    public GameObject obj5;
    private void Start()
    {
        shade.sprite = Renderer.sprite;
    }

    public void ChainEffectOn()
    {
        locker.DOFade(1f, 0.5f);
        shade.DOFade(1f, 0.5f);
    }

    public void ChainEffectOff()
    {
        locker.DOFade(0f, 0.5f);
        shade.DOFade(0f, 0.5f);
    }

    public void ShieldEffectOn()
    {
        obj1.SetActive(true);
    }
    public void ShieldEffectOff()
    {
        obj1.SetActive(false);
    }
    public void ReturnEffectOn()
    {
        obj2.SetActive(true);
    }
    public void ReturnEffectOff()
    {
        obj2.SetActive(false);
    }
    public void RevengeOn()
    {
        Effect.SetActive(true);
    }
    public void UpdatePieceUI(int value)
    {
        pieceUI.SetNumber(value);
    }
    public void CanvasRot()
    {
        pieceUI.SetRotation();
        iconUI.SetRotation();
    }

    public void IconOn(IconType iconType)
    {
        switch ((int)iconType)
        {
            case 0:
                obj3.SetActive(true); break;
            case 1:
                obj4.SetActive(true); break;
            case 2:
                obj5.SetActive(true); break;
        }
    }
    public void IconOff(IconType iconType)
    {
        switch ((int)iconType)
        {
            case 0:
                obj3.SetActive(false); break;
            case 1:
                obj4.SetActive(false); break;
            case 2:
                obj5.SetActive(false); break;
        }
    }

}
public enum IconType
{
    SideMove = 0,
    TwoMove,
    SnakePawn
}
