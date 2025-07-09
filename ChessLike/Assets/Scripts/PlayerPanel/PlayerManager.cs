using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [Header("플레이어 정보")]
    public string PlayerName;
    public Sprite PlayerIcon;
    [Header("인게임 플레이어 정보")]
    public int commonPercent = 40;
    public int uncommonPrecent = 30;
    public int rarePrecent = 15;
    public int mythicPercent = 10;
    public int legendaryPrecent = 5;
    public int CardCount = 3;
    public int MaxCardCount = 4;

    //기본 값
    public readonly int DefaultcommonPercent = 40;
    public readonly int DefaultuncommonPrecent = 30;
    public readonly int DefaultrarePrecent = 15;
    public readonly int DefaultmythicPercent = 10;
    public readonly int DefaultlegendaryPrecent = 5;
    public readonly int DefaultCardCount = 3;
    public readonly int DefaultMaxCardCount = 4;

    //버프 증가 수열
    public readonly int[] CommonSeq = { 2, 3, 5, 8 };
    public readonly int[] UnCommonSeq = { 1, 2, 4, 6 };
    public readonly int[] RareSeq = { 1, 2, 5 };
    public readonly int[] MythicSeq = { 3, 5 };
    public readonly int[] LegendarySeq = { 1, 3 };

    //버프 플래그
    public int CommonIndex = 0;
    public int UnCommonIndex = 0;
    public int RareIndex = 0;
    public int MythicIndex = 0;
    public int LegendaryIndex = 0;
    
    //지금까지 적용된 버프를 보여주는 큐
    public Queue<BuffInfo> BuffDataQueue = new Queue<BuffInfo>();

    //실질적으로 실행될 버프가 들어가는 큐
    public List<UnityEvent> BuffQueue = new List<UnityEvent>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SetDefault();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void SetDefault()
    {
        InitPercent();

        MaxCardCount = DefaultMaxCardCount;

        InitFlag();
    }
    private void InitPercent()
    {
        commonPercent = DefaultcommonPercent;
        uncommonPrecent = DefaultuncommonPrecent;
        rarePrecent = DefaultrarePrecent;
        mythicPercent = DefaultmythicPercent;
        legendaryPrecent = DefaultlegendaryPrecent;
        CardCount = DefaultCardCount;
    }

    private void InitFlag()
    {
        CommonIndex = 0;
        UnCommonIndex = 0;
        RareIndex = 0;
        MythicIndex = 0;
        LegendaryIndex = 0;
    }
    public void ApplyBuff()
    {
        
    }


    public void AddBuff(CardTier cardTier)
    {
        UnityEvent buff = new UnityEvent();
        buff.AddListener(() => { AddTier(cardTier); });
        
        BuffQueue.Add(buff);
    }

    public void AddTier(CardTier cardTier)
    {
        switch ((int)cardTier)
        {
            case 0:
                AddCommon(); break;
            case 1:
                AddUnCommon(); break;
            case 2:
                AddRare(); break;
            case 3:
                AddMythic(); break;
            case 4:
                AddLegendary(); break;
        }
    }

    public void AddCommon()
    {
        if (CommonIndex >= CommonSeq.Length) return;

        commonPercent += CommonSeq[CommonIndex];
        CommonIndex++;
    }

    public void AddUnCommon()
    {
        if (UnCommonIndex >= UnCommonSeq.Length) return;

        uncommonPrecent += UnCommonSeq[UnCommonIndex];
        UnCommonIndex++;
    }

    public void AddRare()
    {
        if (RareIndex >= RareSeq.Length) return;

        rarePrecent += RareSeq[RareIndex];
        RareIndex++;
    }

    public void AddMythic()
    {
        if (MythicIndex >= MythicSeq.Length) return;

        mythicPercent += MythicSeq[MythicIndex];
        MythicIndex++;
    }

    public void AddLegendary()
    {
        if (LegendaryIndex >= LegendarySeq.Length) return;

        legendaryPrecent += LegendarySeq[LegendaryIndex];
        LegendaryIndex++;
    }

}
