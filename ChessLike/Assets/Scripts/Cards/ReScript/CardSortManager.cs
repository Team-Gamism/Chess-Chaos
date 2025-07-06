using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CardSortManager : MonoBehaviour
{
    public List<NCard> cardList = new List<NCard>();
    public CardRandomize cardRandomize;

    public List<NCard> curCard = new List<NCard>();

    private float minValue = -375;
    private float maxValue = 375;

    private void Start()
    {
        cardList = GetComponentsInChildren<NCard>().ToList();
        SetRandomCard();

        CardLoad();

        CardSortNoneAnim();
    }
    private void CardLoad()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            for (int j = 0; j < curCard.Count; j++)
            {
                if (curCard.Any(c => c.cardData == cardList[i].cardData))
                {
                    cardList[i].gameObject.SetActive(true);
                }
                else
                {
                    cardList[i].gameObject.SetActive(false);
                }
            }
        }
    }
    [ContextMenu("Add New Card")]
    public void GetNewCard()
    {
        NCard n = null;
        if (curCard.Count >= 4) return;
        do
        {
            //특정한 가중치를 가진 랜덤한 카드 티어를 가져오기
            WeightedItem item = cardRandomize.GetRandomItem();
            //Debug.Log($"item : {item.cardTier}");

            //카드 티어에 맞는 랜덤한 카드 기믹을 가져오기
            n = GetNCardByWeightItem(item);
            //Debug.Log(n.cardData.name);
        } while (curCard.Exists(c => c == n));
        curCard.Add(n);
        
        n.gameObject.SetActive(true);
        CardSort();
    }
    private void SetRandomCard()
    {
        for (int i = 0; i < PlayerManager.instance.CardCount; i++)
        {
            NCard n = null;
            do
            {
                //특정한 가중치를 가진 랜덤한 카드 티어를 가져오기
                WeightedItem item = cardRandomize.GetRandomItem();
                Debug.Log($"item : {item.cardTier}");

                //카드 티어에 맞는 랜덤한 카드 기믹을 가져오기
                n = GetNCardByWeightItem(item);
                Debug.Log(n.cardData.name);
            } while (curCard.Exists(c => c == n));
            curCard.Add(n);
        }
    }

    public void CardSort()
    {
        int count = curCard.Count;
        if (count == 0) return;

        float spacing = 250f; // 카드 간 간격 (원하는 값으로 조절)
        float totalWidth = spacing * (count - 1);
        float startX = -totalWidth / 2f; // 중앙 기준으로 좌우로 퍼지게

        for (int i = 0; i < count; i++)
        {
            float x = startX + spacing * i;
            RectTransform rt = curCard[i].gameObject.GetComponent<RectTransform>();
            rt.DOAnchorPos(new Vector2(x, rt.anchoredPosition.y), 0.3f);
        }
    }
    private void CardSortNoneAnim()
    {
        int count = curCard.Count;
        if (count == 0) return;

        float spacing = 250f; // 카드 간 간격 (원하는 값으로 조절)
        float totalWidth = spacing * (count - 1);
        float startX = -totalWidth / 2f; // 중앙 기준으로 좌우로 퍼지게

        for (int i = 0; i < count; i++)
        {
            float x = startX + spacing * i;
            RectTransform rt = curCard[i].gameObject.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(x, rt.anchoredPosition.y);
        }
    }

    private NCard GetNCardByWeightItem(WeightedItem item)
    {
        List<NCard> list = cardList.FindAll(c => c.cardData.cardTier == item.cardTier);

        int rand = Random.Range(0, list.Count);

        return list[rand];
    }
}
