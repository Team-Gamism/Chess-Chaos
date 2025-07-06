using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElementSpawner : MonoBehaviour
{
    public float spacing = 20;
    public CanvasGroup canvasGroup;
    public GameObject Element;

    [ContextMenu("Spawn Elements")]
    public void SpawnElements()
    {
        canvasGroup.DOFade(1f, 0.5f);
        Vector3 spawnPos = Vector3.zero;
        List<Element> elements = new List<Element>();
        for (int i = 0; i < 3; i++)
        {
            var element = Instantiate(Element);
            element.transform.SetParent(this.transform, false);
            element.GetComponent<RectTransform>().anchoredPosition = spawnPos;

            spawnPos.y -= element.GetComponent<RectTransform>().sizeDelta.y + spacing;
            elements.Add(element.GetComponent<Element>());
        }
        MoveElements(elements.ToArray());
    }
    private void MoveElements(Element[] arr)
    {
        float ySize = arr[0].GetComponent<RectTransform>().sizeDelta.y + spacing;
        for (int i = arr.Length - 1; i >= 0; i--)
        {
            arr[i].MoveElement((arr.Length - 1 - i) * ySize);
        }
    }
}
