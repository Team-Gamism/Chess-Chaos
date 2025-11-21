using ChessEngine;
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
    public ChessColor PlayerColor;
    public int commonPercent = 40;
    public int uncommonPrecent = 30;
    public int rarePrecent = 15;
    public int mythicPercent = 10;
    public int legendaryPrecent = 5;
    public int CardCount = 3;
    public int MaxCardCount = 4;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateIcon(Sprite icon)
    {
        PlayerIcon = icon;
    }

}
