using System.Collections.Generic;
using ChessEngine;
using ChessEngine.Game;
using ChessEngine.Game.AI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AllChange : MonoBehaviour, ISkill
{
    public void Execute()
    {
        ChessGameManager manager = FindObjectOfType<ChessGameManager>();
        ChessAIGameManager AImanager = FindObjectOfType<ChessAIGameManager>();

        ChessColor player = AImanager.IsBlackAIEnabled ? ChessColor.Black : ChessColor.White;
        ChessColor enemy = AImanager.IsBlackAIEnabled ? ChessColor.White : ChessColor.Black;

        if (canExecute())
        {
            manager.visualTable.Table.ChangeAllPiece(player, enemy);
            List<ChessPiece> playerPiece = manager.visualTable.Table.GetColorPieceAll(player);
            List<ChessPiece> enemyPiece = manager.visualTable.Table.GetColorPieceAll(enemy);

            foreach (ChessPiece p in playerPiece)
            {
                manager.visualTable.GetVisualPiece(p).UpdatePosition();
                manager.visualTable.GetVisualPiece(p).UpdateVisuals();
                if (p.GetChessPieceType() == ChessPieceType.Pawn && p.MoveCount <= 1)
                {
                    p.MoveCount = 0;
                }
            }
            
            foreach (ChessPiece e in enemyPiece)
            {
                manager.visualTable.GetVisualPiece(e).UpdatePosition();
                manager.visualTable.GetVisualPiece(e).UpdateVisuals();
                if (e.GetChessPieceType() == ChessPieceType.Pawn && e.MoveCount <= 1)
                {
                    e.MoveCount = 0;
                }
            }
        }
    }

    public bool canExecute()
    {
        return FindObjectOfType<ChessGameManager>() != null;
    }
}

