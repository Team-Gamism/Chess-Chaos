using DG.Tweening;
using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    public SpriteRenderer Renderer;
    [Header("고정 효과")]
    public SpriteRenderer locker;
    public SpriteRenderer shade;
    public CardData Pin;
    [Header("방어막 효과")]
    public GameObject obj1;
    public CardData Shield;
    [Header("복수 효과")]
    public GameObject Effect;
    public CardData Revenge;
    [Header("고정 효과")]
    public PieceUI pieceUI;
    [Header("복귀 효과")]
    public GameObject obj2;
    public IconUI iconUI;
    public CardData Return;
    [Header("사선 이동")]
    public GameObject obj3;
    public CardData SideMove;
    [Header("두칸 이동")]
    public GameObject obj4;
    public CardData TwoMove;
    [Header("암습의 폰")]
    public GameObject obj5;
    public CardData KnightMove;
    [Header("효과음")]
    public AudioClip ShieldOnSFX;
    public AudioClip ShieldOffSFX;
    public AudioClip RevengeOnSFX;
    public AudioClip RevengeOffSFX;
    public AudioClip ReturnSFX;
    public AudioClip ReturnOffSFX;
    public AudioClip PinSFX;
    public AudioClip ApplySFX;

    private CardEffectSpawner spawner;

    private void Start()
    {
        shade.sprite = Renderer.sprite;
        spawner = FindFirstObjectByType<CardEffectSpawner>();
    }

    public void ChainEffectOn()
    {
        SoundManager.Instance.SFXPlay("pin", PinSFX);
        locker.DOFade(1f, 0.5f);
        shade.DOFade(1f, 0.5f);
    }

    public void ChainEffectOff()
    {
        SoundManager.Instance.SFXPlay("pinOff", PinSFX);
		spawner.SpawnEffector(Pin, SpawnType.BreakCard);
		locker.DOFade(0f, 0.5f);
        shade.DOFade(0f, 0.5f);
    }

    public void ShieldEffectOn()
    {
        SoundManager.Instance.SFXPlay("shieldOn", ShieldOnSFX);
        obj1.SetActive(true);
    }
    public void ShieldEffectOff()
    {
        SoundManager.Instance.SFXPlay("shieldOff", ShieldOffSFX);
        spawner.SpawnEffector(Shield, SpawnType.BreakCard);
        obj1.SetActive(false);
    }
    public void ReturnEffectOn()
    {
        SoundManager.Instance.SFXPlay("return", ReturnSFX);
		obj2.SetActive(true);
    }
    public void ReturnEffectOff()
    {
        SoundManager.Instance.SFXPlay("re", ReturnOffSFX);
		spawner.SpawnEffector(Return, SpawnType.BreakCard);
		obj2.SetActive(false);
    }
    public void RevengeOn()
    {
        SoundManager.Instance.SFXPlay("RevengeOn", RevengeOnSFX);
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
                obj3.SetActive(true);
                SoundManager.Instance.SFXPlay("apply", ApplySFX);
                break;
            case 1:
                obj4.SetActive(true);
                SoundManager.Instance.SFXPlay("apply", ApplySFX);
                break;
            case 2:
                obj5.SetActive(true);
                SoundManager.Instance.SFXPlay("apply", ApplySFX);
                break;
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
