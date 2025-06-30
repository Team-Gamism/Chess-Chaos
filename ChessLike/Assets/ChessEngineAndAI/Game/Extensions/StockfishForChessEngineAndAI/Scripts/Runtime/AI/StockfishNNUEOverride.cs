using System;
using UnityEngine;

namespace ChessEngine.AI.Stockfish
{
    /// <summary>
    /// A component that works with the StockfishChessAIManager component to override the 'EvalFIle' setting for NNUE data.
    /// Don't forget to re-initialize the engine using the 'uci' command or similiar after changing NNUE data (if not changed automatically PostUciInitialzied to nnuePath).
    /// </summary>
	/// Author: Intuitive Gaming Solutions
	[RequireComponent(typeof(StockfishChessAIManager))]
	public class StockfishNNUEOverride  : MonoBehaviour
	{
        #region Editor Serialized Setting(s)
        [Header("Settings")]
        [Tooltip("The override path for NNUE data eval file relative to 'Application.dataPath'.")]
        public string nnuePath;
        [Tooltip("The relative path mode to look for NNUE data eval file in.\n\nData - relative to Application.dataPath.\nPersistent - relative to Application.persistentDataPath.\nStreamingAssets - relative to Application.streamingAssetsPath.")]
        public StockfishPathMode nnuePathMode = StockfishPathMode.Data;
        #endregion
        #region Public Properties
        /// <summary>A reference to the StockfishChessAIManager component that is driving this component.</summary>
        public StockfishChessAIManager AIManager { get; private set; }
        #endregion

        #region Unity Callback(s)
        void Awake()
        {
            // Find AI manager reference.
            AIManager = GetComponent<StockfishChessAIManager>();

            // Subscribe to relevant AI manager event(s).
            AIManager.PostUciInitialized.AddListener(OnPostUciInitialized);
        }

        void OnDestroy()
        {
            // Unsubscribe from relevant AI manager event(s).
            AIManager.PostUciInitialized.RemoveListener(OnPostUciInitialized);
        }
        #endregion

        #region Public NNUE Override Method(s)
        /// <summary>Simply invokes 'OverrideNNUEPath(nnuePath)'.</summary>
        public void OverrideNNUEPath()
        {
            OverrideNNUEPath(nnuePath, nnuePathMode);
        }

        /// <summary>
        /// Overrides the currently NNUE path (using UCI commands) to the full resolved path of pPath, which is the path relative to 'Application.dataPath' of the new NNUE file.
        /// Minimal NNUE file validation is performed, ensure you're providing a path to a compatible NNUE file. Consider looking here for a test file: https://data.stockfishchess.org/nn/nn-0000000000a0.nnue
        /// </summary>
        /// <param name="pPath"></param>
        /// <param name="pPathMode"></param>
        public void OverrideNNUEPath(string pPath, StockfishPathMode pPathMode)
        {
            // Get full path.
            string fullPath = StockfishWrapper.GetFullNNUEPath(pPath, pPathMode);

            // Check if the file exists and has the .nnue extension.
            if (System.IO.File.Exists(fullPath) && System.IO.Path.GetExtension(fullPath).Equals(".nnue", StringComparison.OrdinalIgnoreCase))
            {
                // Set the 'EvalFile' option as soon as possible.
                StockfishWrapper.UCI.ProcessCommand($"setoption name EvalFile value {fullPath}");
            }
            else
            {
                // Log a warning if the file does not exist or has a different extension.
                Debug.LogWarning("Unable to override NNUE path using StockfishNNUEOverride component! The specified NNUE file does not exist or has an incorrect extension.", gameObject);
            }
        }
        #endregion

        #region Private AI Manager Callback(s)
        /// <summary>Invoked after uci is initialized. This is the earliest time UCI commands are guarenteed to be available.</summary>
        void OnPostUciInitialized()
        {
            // Override the NNUE path.
            OverrideNNUEPath();
        }
        #endregion
    }
}
