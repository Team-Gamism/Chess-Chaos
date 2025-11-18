using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [Header("상대 정보")]
    public string EnemyName;
    public Sprite EnemyIcon;
    [Header("인게임 상대 정보")]
    public int ELO = 1200;

    //지금까지 적용된 버프를 보여주는 큐
    public Queue<BuffInfo> BuffDataQueue = new Queue<BuffInfo>();

    //실질적으로 실행될 버프가 들어가는 큐
    public List<UnityEvent> BuffQueue = new List<UnityEvent>();

    public readonly int DefaultELO = 700;

    //디버프 수열
    public readonly int[] DecSeq = { 10, 20, 40, 50, 100 };

    //디버프 플래그
    public int DecIndex = 0;

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
    public void AddELO()
    {
        if (DecIndex >= DecSeq.Length) return;

        ELO -= DecSeq[DecIndex];
        DecIndex++;
    }

    public void SetDefault()
    {
        ELO = DefaultELO;

        DecIndex = 0;
    }
    public void SetLevel(int n)
    {
        int[] ELOs = { 1000, 1400, 1800 };
        string[] names = { "이지 레벨 봇", "노멀 레벨 봇", "하드 레벨 봇" };
        ELO = ELOs[n];
        EnemyName = names[n];
    }
    public void GameEnd()
    {
        Destroy(gameObject);
    }
}
