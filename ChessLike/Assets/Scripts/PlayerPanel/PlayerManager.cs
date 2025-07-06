using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public string PlayerName;
    public int commonPercent = 40;
    public int uncommonPrecent = 30;
    public int rarePrecent = 15;
    public int mythicPercent = 10;
    public int legendaryPrecent = 5;
    public int CardCount = 3;
    public Queue<BuffInfo> BuffQueue = new Queue<BuffInfo>();
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
}
