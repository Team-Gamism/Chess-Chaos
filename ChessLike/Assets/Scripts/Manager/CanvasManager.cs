using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 메인 화면의 캔버스를 쉽게 관리할 수 있는 클래스입니다.

public class CanvasManager : MonoBehaviour
{
    public GameObject[] Canvases = new GameObject[4];

    private GameObject curCanvas;
    private void Start()
    {
        curCanvas = Canvases[1];
    }

    public void LoadCanvas(CanvasType type)
    {
        curCanvas.SetActive(false);
        curCanvas = Canvases[(int)type];
        curCanvas.SetActive(true);
    }
}

public enum CanvasType
{
    start = 0,
    main,
    skin,
    ranking
}
