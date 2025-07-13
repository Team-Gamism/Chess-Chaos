using System.Collections.Generic;
using ChessEngine;
using ChessEngine.Game;
using ChessEngine.Game.AI;
using UnityEngine;
using UnityEngine.Events;

public class CheckmateDecManage : MonoBehaviour
{
    private ChessGameManager chessManager;
    private ChessAIGameManager chessAIManager;
    [SerializeField]
    private int turnCount = 0;
    public int MaxTurnCount = 5;
    public int MaxDestroyPieceCount = 5;
    public bool IsInCheck = false;
    public UnityEvent<int> OnTurnAdded = new UnityEvent<int>();
    public CheckmateUI ui;
    public CheckmateIconTween tween;
    public AudioClip CheckmateSFX;
    public AudioClip CheckmateEndSFX;
    private ChessColor color;
    private ChessColor enemyColor;
    private void Start()
    {
        chessAIManager = FindObjectOfType<ChessAIGameManager>();
        chessManager = FindObjectOfType<ChessGameManager>();
        color = chessAIManager.IsBlackAIEnabled ? ChessColor.Black : ChessColor.White;
        enemyColor = chessAIManager.IsBlackAIEnabled ? ChessColor.White : ChessColor.Black;

        OnTurnAdded.AddListener(ui.UpdateUI);
    }

    public void TweenEnable()
    {
        SoundManager.Instance.SFXPlay("c", CheckmateSFX);
        tween.gameObject.SetActive(true);
        ui.group.alpha = 1f;
        int t = MaxTurnCount + 1 - turnCount;
        OnTurnAdded?.Invoke(t);
    }

    // Update is called once per frame
    void Update()
    {
        IsInCheck = chessManager.visualTable.Table.IsInCheck(color);
        if (IsInCheck &&
            turnCount <= MaxTurnCount &&
            chessManager.isCheckmateDec)
        {
            SoundManager.Instance.SFXPlay("play", CheckmateEndSFX);
            List<ChessPiece> pieces = chessManager.visualTable.Table.GetColorPiece(enemyColor);
            //상대 기물 랜덤하게 파괴하기
            for (int i = 0; i < MaxDestroyPieceCount; i++)
            {
                chessManager.visualTable.Table.DestroyPiece(pieces[Random.Range(0, pieces.Count)]);
            }
            turnCount = 0;
            tween.DisableThis();
            OnTurnAdded?.Invoke(0);
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
        if (chessManager.isCheckmateDec)
        {
            turnCount++;
            int t = MaxTurnCount + 1 - turnCount;
            OnTurnAdded?.Invoke(t);
        }
    }
}
