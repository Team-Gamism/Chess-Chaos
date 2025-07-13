using ChessEngine.Game;
using UnityEngine;
using ChessEngine;
using UnityEngine.UI;

public class BlockCanvas : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup group;
    private ChessGameManager manager;

    private void Start()
    {
        manager = FindObjectOfType<ChessGameManager>();
    }

    private void Update()
    {
        if (manager.ChessInstance.turn == ChessColor.White)
        {
            group.blocksRaycasts = false;
        }
        else
        {
            group.blocksRaycasts = true;
        }
    }
}
