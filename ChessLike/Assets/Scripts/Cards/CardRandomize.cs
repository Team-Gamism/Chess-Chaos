using System;
using System.Collections.Generic;
using UnityEngine;

public class CardRandomize : MonoBehaviour
{
    public List<WeightedItem> weightedItem = new List<WeightedItem>();
    public void Start()
    {
        weightedItem[0].weight = PlayerManager.instance.commonPercent;
        weightedItem[1].weight = PlayerManager.instance.uncommonPrecent;
        weightedItem[2].weight = PlayerManager.instance.rarePrecent;
        weightedItem[3].weight = PlayerManager.instance.mythicPercent;
        weightedItem[4].weight = PlayerManager.instance.legendaryPrecent;
    }

    public WeightedItem GetRandomItem()
    {
        float total = 0;
        foreach (var item in weightedItem)
            if (item.weight > 0)
                total += item.weight;

        float rand = UnityEngine.Random.Range(0, total);
        float sum = 0;

        foreach (var item in weightedItem)
        {
            sum += item.weight;
            if (rand < sum)
            {
                return item;
            }
        }
        return null;
    }
}

[Serializable]
public class WeightedItem
{
    public CardTier cardTier;
    public float weight;
}
