using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int commonPercent = 40;
    public int uncommonPrecen = 30;
    public int rarePrecent = 15;
    public int mythicPercent = 10;
    public int legendaryPrecent = 5;
    public Queue<BuffInfo> BuffQueue = new Queue<BuffInfo>();
}
