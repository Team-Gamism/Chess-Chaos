using DG.Tweening;
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
        Debug.Log("실행1");
        pieceUI.SetRotation();
    }
}
