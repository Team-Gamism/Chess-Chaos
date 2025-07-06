using ChessEngine.Game;
using UnityEngine;

public class CheckmateDec : MonoBehaviour, ISkill
{
    public void Execute()
    {
        if(canExecute())
            FindObjectOfType<ChessGameManager>().isCheckmateDec = true;
    }

    public bool canExecute()
    {
        return FindObjectOfType<ChessGameManager>() != null;
    }
}

