using UnityEngine;

namespace ChessEngine.AI.Stockfish
{
    /// <summary>
    /// Helper methods for working with file systems across
    /// platforms and Stockfish.
    /// </summary>
    /// Author: Intuitive Gaming Solutions
    public static class StockfishFileSystem
    {
        /// <summary>
        /// Returns the application path associated with
        /// the specified mode on the current platform.
        /// </summary>
        /// <param name="pMode"></param>
        /// <returns>the application path associated with the specified mode.</returns>
        public static string GetPathByMode(StockfishPathMode pMode)
        {
            if (pMode == StockfishPathMode.Data)
                return Application.dataPath;
            if (pMode == StockfishPathMode.Persistent)
                return Application.persistentDataPath;
            if (pMode == StockfishPathMode.StreamingAssets)
                return Application.streamingAssetsPath;

            Debug.LogWarning($"StockfishFileSystem.GetPathByMode({pMode}) invoked using an unhandled StockfishPathMode!");
            return string.Empty;
        }
    }
}
