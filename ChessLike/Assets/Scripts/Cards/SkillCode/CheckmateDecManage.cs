using System.Collections.Generic;
using ChessEngine;
using ChessEngine.Game;
using ChessEngine.Game.AI;
using UnityEngine;

public class CheckmateDecManage : MonoBehaviour
{
    private ChessGameManager chessManager;
    private ChessAIGameManager chessAIManager;
    [SerializeField]
    private int turnCount = 0;
    public int MaxTurnCount = 5;
    public int MaxDestroyPieceCount = 5;
    public bool IsInCheck = false;
    private ChessColor color;
    private ChessColor enemyColor;
    private void Start()
    {
        chessAIManager = FindObjectOfType<ChessAIGameManager>();
        chessManager = FindObjectOfType<ChessGameManager>();
        color = chessAIManager.IsBlackAIEnabled ? ChessColor.Black : ChessColor.White;
        enemyColor = chessAIManager.IsBlackAIEnabled ? ChessColor.White : ChessColor.Black;
    }

    // Update is called once per frame
    void Update()
    {
        IsInCheck = chessManager.visualTable.Table.IsInCheck(color);
        if (IsInCheck &&
            turnCount <= MaxTurnCount &&
            chessManager.isCheckmateDec)
        {
            Debug.Log("체크메이트");
            List<ChessPiece> pieces = chessManager.visualTable.Table.GetColorPiece(enemyColor);
            //상대 기물 랜덤하게 파괴하기
            for (int i = 0; i < MaxDestroyPieceCount; i++)
            {
                chessManager.visualTable.Table.DestroyPiece(pieces[Random.Range(0, pieces.Count)]);
            }
            turnCount = 0;
            chessManager.isCheckmateDec = false;
        }
        else if (turnCount > MaxTurnCount)
        {
            turnCount = 0;
            chessManager.isCheckmateDec = false;
        }
    }
    public void AddTurn()
    {
        if(chessManager.isCheckmateDec)
            turnCount++;
    }
}
