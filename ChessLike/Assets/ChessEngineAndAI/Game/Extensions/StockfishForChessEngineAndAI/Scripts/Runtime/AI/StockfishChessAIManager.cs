using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using ChessEngine.Delegates;
using ChessEngine.Game.AI;
using System.IO;

namespace ChessEngine.AI.Stockfish
{
    /// <summary>
    /// Stockfish chess AI ELO setting.
    /// 
    /// Allows difficulty (ELO) to be set for the Stockfish Chess AI.
    /// TurnEnded and PostGameReset events are used instead of turn started to give Stockfish maximum time to perform any changes it needs after setting ELO between turns.
    /// </summary>
	/// Author: Intuitive Gaming Solutions
	[RequireComponent(typeof(ChessAIGameManager))]
	public class StockfishChessAIManager : MonoBehaviour
	{
        #region Editor Serialized Setting(s)
        [Header("Settings - EvalFile")]
        [Tooltip("The default eval file path (relative to the default path mode) to use if not overrided via 'OverrideDefaultEvalFilePathCallback'.")]
        public string defaultEvalFilePath = "nn-0000000000a0.nnue";
        [Tooltip("The relative path mode to use when determing the full eval file path.\n\nData - relative to Application.dataPath.\nPersistent - relative to Application.persistentDataPath.\nStreamingAssets - relative to Application.streamingAssetsPath.")]
        public StockfishPathMode defaultPathMode = StockfishPathMode.Data;

        [Header("Settings - White")]
        [Tooltip("The target 'elo' value for the Stockfish AI on white team. (NOTE: This value will be clamped between the engine's reported minimum and maximum ELO automatically before being submitted.)")]
        public ushort whiteELO = 1600;

        [Header("Settings - Black")]
        [Tooltip("The target 'elo' value for the Stockfish AI on black team. (NOTE: This value will be clamped between the engine's reported minimum and maximum ELO automatically before being submitted.)")]
        public ushort blackELO = 1600;
        #endregion
        #region Editor Serialized Event(s)
        [Header("Events")]
        [Tooltip("An event that is invoked after UCI is initialized. Use this event to invoke things at the earliest time UCI commands are guarenteed to be available.")]
        public UnityEvent PostUciInitialized;
        #endregion
        #region Public Properties
        /// <summary>The ChessAIGameManager that is driving the games AI.</summary>
        public ChessAIGameManager GameManager { get; protected set; }
        /// <summary>The default ELO as parsed by the last processed Stockfish AI.</summary>
        public ushort DefaultELO { get; protected set; } = 1600;
        /// <summary>The minimum ELO as parsed by the last processed Stockfish AI.</summary>
        public ushort MinimumELO { get; protected set; } = 1250;
        /// <summary>The maximum ELO as parsed by the last processed Stockfish AI.</summary>
        public ushort MaximumELO { get; protected set; } = 2850;
        #endregion
        #region Public ActionRef Event(s)
        /// <summary>
        /// An event that is invoked to provide listeners a chance to override the default full
        /// NNUE EvalFile path used by stockfish.
        /// 
        /// Arg0: StockfishChessAIManager   - this Stockfish chess AI manager involved in the event.
        /// Arg1: ref string                - a reference to the full eval file path.
        /// </summary>
        public event ValueActionRef<StockfishChessAIManager, string> OverrideDefaultEvalFilePathCallback;
        #endregion

        private NNUEFileManager nNUEFileManager;

        #region Unity Callback(s)
        void Awake()
        {
            // Find AI manager reference.
            GameManager = GetComponent<ChessAIGameManager>();
            nNUEFileManager = GetComponent<NNUEFileManager>();

            // Initialize stockfish UCI first.
            StockfishWrapper.InitializeUci();

            // Set default eval file.
            StockfishWrapper.SetOption("EvalFile", GetDefaultEvalFilePath());

            // Invoke the 'PostUCIInitialized' Unity event.
            PostUciInitialized?.Invoke();

            // Initialize stockfish.
            StockfishWrapper.Initialize();

            // Send the UCI command to get settings.
            StockfishWrapper.UCI.ProcessCommand("uci");
        }

        void OnEnable()
        {
            // Subscribe to relevant game event(s).
            GameManager.PreGameReset.AddListener(OnPreGameReset);
            GameManager.PostGameReset.AddListener(OnPostGameReset);
            GameManager.TurnEnded.AddListener(OnTurnEnded);

            // Subscribe to relevant Stockfish event(s).
            StockfishWrapper.OutputLineReceived += OnOutputLineReceived;
        }

        void OnDisable()
        {
            // Unsubscribe from relevant game event(s).
            GameManager.PreGameReset.RemoveListener(OnPreGameReset);
            GameManager.PostGameReset.RemoveListener(OnPostGameReset);
            GameManager.TurnEnded.RemoveListener(OnTurnEnded);

            // Unsubscribe from relevant Stockfish event(s).
            StockfishWrapper.OutputLineReceived -= OnOutputLineReceived;
        }
        #endregion

        #region Public ELO Method(s)
        /// <summary>Sets the difficulty of Stockfish chess AI by ELO.</summary>
        /// <param name="pELO"></param>
        public void SetELO(int pELO)
		{
			// Process the UCI command to set ELO.
            StockfishWrapper.UCI.ProcessCommand("setoption name UCI_Elo value " + ClampInt(pELO, MinimumELO, MaximumELO));
		}
        #endregion
        #region Public EvalFile Method(s)
        /// <summary>
        /// Returns the full default EvalFile path to use for Stockfish.
        /// </summary>
        /// <returns>the full default EvalFile path to use for Stockfish.</returns>
        public string GetDefaultEvalFilePath()
        {
            string path = StockfishWrapper.GetFullNNUEPath(defaultEvalFilePath, defaultPathMode);
            
            // Invoke the 'OverrideDefaultEvalFilePathCallback' event to allow listeners a chance to override path.
            OverrideDefaultEvalFilePathCallback?.Invoke(this, ref path);

            return path;
        }
        #endregion

        #region Private Game Callback(s)
        /// <summary>Invoked just before the game is reset.</summary>
        void OnPreGameReset()
        {
            // Inform Stockfish of a new game.
            StockfishWrapper.UCI.ProcessCommand("ucinewgame");
            StockfishWrapper.UCI.ProcessCommand("position startpos");
        }

        /// <summary>Invoked just after the chess game is reset.</summary>
        void OnPostGameReset()
        {
            // White turn.
            if (GameManager.ChessInstance.turn == ChessColor.White)
            {
                if (GameManager.IsWhiteAIEnabled && GameManager.WhiteAIInstance is StockfishChessAI)
                    SetELO(whiteELO);
            }
            else
            {
                if (GameManager.IsBlackAIEnabled && GameManager.BlackAIInstance is StockfishChessAI)
                    SetELO(blackELO); 
            }
        }

        /// <summary>Invoked when a turn ends.</summary>
        /// <param name="pEndedTurn"></param>
        /// <param name="pMoveInfo"></param>
        void OnTurnEnded(ChessColor pEndedTurn, MoveInfo pMoveInfo)
        {
            // White turn ended, set to black ELO.
            if (pEndedTurn == ChessColor.White)
            {
                if (GameManager.IsBlackAIEnabled && GameManager.BlackAIInstance is StockfishChessAI)
                    SetELO(blackELO);
            }
            // Otherwise black turn ended, set to white ELO.
            else
            {
                if (GameManager.IsWhiteAIEnabled && GameManager.WhiteAIInstance is StockfishChessAI)
                    SetELO(whiteELO); 
            }
        }
        #endregion
        #region Private Stockfish Callback(s)
        /// <summary>Invoked after an output line is received from Stockfish.</summary>
        void OnOutputLineReceived(string pLine)
        {
            // SECTION: Use Regex to listen for UCI settings.
            string pattern = @"option name UCI_Elo type spin default (\d+) min (\d+) max (\d+)";
            Match match = Regex.Match(pLine, pattern);
            if (match.Success)
            {
                // Extract default, min, and max values from the match groups.
                string defaultStr = match.Groups[1].Value;
                string minStr = match.Groups[2].Value;
                string maxStr = match.Groups[3].Value;

                // Convert the extracted strings to ushort.
                if (ushort.TryParse(defaultStr, out ushort defaultElo) &&
                    ushort.TryParse(minStr, out ushort minElo) &&
                    ushort.TryParse(maxStr, out ushort maxElo))
                {
                    DefaultELO = defaultElo;
                    MinimumELO = minElo;
                    MaximumELO = maxElo;

                    // Log valid ELO range.
                    Debug.Log("ELO Settings detected. [Default: " + DefaultELO + "|Min: " + MinimumELO + "|Max: " + MaximumELO + "]", gameObject);
                }
            }
        }
        #endregion

        #region Public Static Method(s)
        /// <summary>Clamps pValue between pMin and pMax and returns the clamped value.</summary>
        /// <param name="pValue"></param>
        /// <param name="pMin"></param>
        /// <param name="pMax"></param>
        /// <returns>pValue clamped between pMin and pMax.</returns>
        public static int ClampInt(int pValue, int pMin, int pMax)
        {
            // Ensure that value is within the specified range.
            return Math.Max(pMin, Math.Min(pMax, pValue));
        }
        #endregion
    }
}
