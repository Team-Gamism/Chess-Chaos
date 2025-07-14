using ChessEngine;
using ChessEngine.Game;
using UnityEngine;

public class CardCanvas : MonoBehaviour
{
    public GameObject obj;
    private ChessGameManager manager;
    void Start()
    {
        manager = FindObjectOfType<ChessGameManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.visualTable.Table.IsInCheck(ChessColor.White))
        {
            obj.SetActive(true);
        }
        else
        {
            obj.SetActive(false);
        }
    }
}
