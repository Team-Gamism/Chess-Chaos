using System;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using ChessEngine.Game.AI;

namespace ChessEngine.AI.Stockfish
{
    /// <summary>
    /// Stockfish chess AI ELO setting.
    /// 
    /// Allows difficulty (ELO) to be set for the Stockfish Chess AI.
    /// TurnEnded and PostGameReset events are used instead of turn started to give Stockfish maximum time to perform any changes it needs after setting ELO between turns.
    /// 
	/// This component remotely downloads NNUE data.
	/// Note that this is an alternative to StockfishChessAIManager.
	/// </summary>
	/// Author: Intuitive Gaming Solutions
	[RequireComponent(typeof(ChessAIGameManager))]
    public class RemoteNNUEStockfishChessAIManager : MonoBehaviour
    {
        #region Editor Serialized Setting(s)
        [Header("Settings")]
        [Tooltip("Should this component automatically initailzie Stockfish after confirming an NNUE file is available?")]
        public bool autoInit = true;
        [Tooltip("Should this component automatically start a new chess game once initialized after confirming an NNUE file is available? Irrelevant if 'autoInit' is false.")]
        public bool autoStart = true;

        [Header("Settings - NNUE")]
        public string nnueFileName = "nn-0000000000a0.nnue";
        [Tooltip("The default eval file path (relative to the default path mode) to use if not overrided via 'OverrideDefaultEvalFilePathCallback'.")]
        public string nnueDownloadUrl = "https://raw.githubusercontent.com/official-stockfish/networks/master/nn-0000000000a0.nnue";

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
        /// <summary>The full path to a valid NNUE file, or string.Empty.</summary>
        public string ValidNNUEPath { get; protected set; } = string.Empty;
        /// <summary>
        /// Tracks if there is an active NNUE download for this component.
        /// </summary>
        public bool IsDownloadingNNUE { get; protected set; }
        #endregion
        #region Public Event(s)
        /// <summary>
        /// Invoked when the NNUE data file is found to be available.
        /// 
        /// Arg0: RemoteNNUEStockfishChessAIManager - this component involved in the event.
        /// Arg1: string                            - the full path of the available NNUE file.
        /// </summary>
        public event Action<RemoteNNUEStockfishChessAIManager, string> NNUEFileAvailable;
        /// <summary>
        /// Invoked when the NNUE data file has been successfully downloaded.
        /// 
        /// Arg0: RemoteNNUEStockfishChessAIManager - this component involved in the event.
        /// Arg1: string                            - the full path of the available NNUE file.
        /// Arg2: string                            - the download url.
        /// </summary>
        public event Action<RemoteNNUEStockfishChessAIManager, string, string> NNUEFileDownloaded;
        /// <summary>
        /// Invoked after the NNUE file download fails.
        /// 
        /// Arg0: RemoteNNUEStockfishChessAIManager - this component involved in the event.
        /// Arg1: string                            - the download url.
        /// </summary>
        public event Action<RemoteNNUEStockfishChessAIManager, string> NNUEFileDownloadFailed;
        #endregion

        #region Unity Callback(s)
        void Awake()
        {
            // Not downloading yet.
            IsDownloadingNNUE = false;

            // Find AI manager reference.
            GameManager = GetComponent<ChessAIGameManager>();

            // Start downloading NNUE data immediately.         
            FindNNUEFile();
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

        #region Protected NNUE Method(s)
        /// <summary>
        /// Checks for an NNUE file and invokes 'OnNNUEFileAvailable' if found,
        /// otherwise starts a download if one is not already active.
        /// </summary>
        protected void FindNNUEFile()
        {
            string filePath = Path.Combine(Application.persistentDataPath, nnueFileName);
            // If the file already exists invoke 'OnNNUEFileAvailable' with the path.
            if (File.Exists(filePath))
            {
                OnNNUEFileAvailable(filePath);
            }
            // Otherwise start a download.
            else if (!IsDownloadingNNUE)
            {
                StartCoroutine(DownloadNNUEFile());
            }
        }

        /// <summary>
        /// A coroutine method that (enumerable) that downloads the NNUE file from the
        /// nnueDownloadUrl specified for this component.
        /// </summary>
        /// <returns>an enumerator.</returns>
        protected IEnumerator DownloadNNUEFile()
        {
            IsDownloadingNNUE = true;
            if (Application.isEditor)
                Debug.Log($"NNUE file missing. Downloading from '{nnueDownloadUrl}'...", gameObject);

            using (UnityWebRequest request = UnityWebRequest.Get(nnueDownloadUrl))
            {
                string filePath = Path.Combine(Application.persistentDataPath, nnueFileName);
                request.downloadHandler = new DownloadHandlerFile(filePath);
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    OnNNUEFileDownloadFailed(nnueDownloadUrl);
                    Debug.LogError($"Failed to download NNUE file: {request.error}", gameObject);
                }
                else
                {
                    if (Application.isEditor)
                        Debug.Log($"NNUE file downloaded successfully to '{filePath}'.");
                    OnNNUEFileDownloaded(filePath, nnueDownloadUrl);
                    OnNNUEFileAvailable(filePath);
                }
            }

            IsDownloadingNNUE = false;
        }
        #endregion
        #region Protected NNUE Callback(s)
        /// <summary>
        /// Invoked after the system downloaded a NNUE file successfully.
        /// </summary>
        /// <param name="pFullPath">The full path to the downloaded file.</param>
        /// <param name="pDownloadUrl">The download URL.</param>
        protected void OnNNUEFileDownloaded(string pFullPath, string pDownloadUrl)
        {
            // Invoke the 'NNUEFileDownloaded' event.
            NNUEFileDownloaded?.Invoke(this, pFullPath, pDownloadUrl);
        }

        /// <summary>
        /// Invoked after the system fails to download an NNUE file.
        /// </summary>
        /// <param name="pDownloadUrl"></param>
        protected void OnNNUEFileDownloadFailed(string pDownloadUrl)
        {
            // Invoke the 'NNUEFileDownloadFailed' event.
            NNUEFileDownloadFailed?.Invoke(this, pDownloadUrl);
        }

        /// <summary>
        /// Invoked when the system found an NNUE file that is available.
        /// </summary>
        /// <param name="pFullPath">The full path to the available NNUE file.</param>
        protected void OnNNUEFileAvailable(string pFullPath)
        {
            // Invoke the 'NNUEFileAvailable' event.
            NNUEFileAvailable?.Invoke(this, pFullPath);

            // Initialize stockfish if auto initialize is true.
            if (autoInit)
            {
                // Initialize stockfish UCI first.
                StockfishWrapper.InitializeUci();

                // Set default eval file.
                StockfishWrapper.SetOption("EvalFile", pFullPath);

                // Invoke the 'PostUCIInitialized' Unity event.
                PostUciInitialized?.Invoke();

                // Initialize stockfish.
                StockfishWrapper.Initialize();

                // Send the UCI command to get settings.
                StockfishWrapper.UCI.ProcessCommand("uci");

                // Start the game if set to auto start.
                if (autoStart)
                    GameManager.NewGame();
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
