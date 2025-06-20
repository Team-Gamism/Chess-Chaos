using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardSortManager : MonoBehaviour
{
    public List<NCard> cardList = new List<NCard>();
    public CardRandomize cardRandomize;

    public List<NCard> curCard = new List<NCard>();

    private void Start()
    {
        cardList = GetComponentsInChildren<NCard>().ToList();
        SetRandomCard();

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
    private void SetRandomCard()
    {
        for (int i = 0; i < (int)GameManager.instance.level; i++)
        {
            WeightedItem item = cardRandomize.GetRandomItem();
            Debug.Log($"item : {item.cardTier}");

            // 1. 해당 티어의 카드들 중에서
            List<NCard> randCards = cardList.FindAll(p => p.cardData.cardTier == item.cardTier);

            // 2. curCard에 이미 들어간 것 제거 (역순으로 안전하게)
            for (int j = randCards.Count - 1; j >= 0; j--)
            {
                if (curCard.Exists(c => c.cardData == randCards[j].cardData))
                {
                    randCards.RemoveAt(j);
                }
            }

            // 3. 남은 카드가 있다면 무작위로 하나 추가
            if (randCards.Count > 0)
            {
                int randIndex = Random.Range(0, randCards.Count);
                curCard.Add(randCards[randIndex]);
            }
            else
            {
                Debug.LogWarning("중복 제외 후 선택 가능한 카드가 없습니다.");
            }
        }
    }


}
