using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
