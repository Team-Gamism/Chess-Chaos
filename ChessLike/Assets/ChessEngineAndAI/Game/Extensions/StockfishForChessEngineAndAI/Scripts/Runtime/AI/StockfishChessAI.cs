using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;

namespace ChessEngine.AI.Stockfish
{
    /// <summary>
    /// Stockfish chess AI.
    /// 
    /// Implements the Stockfish chess engine.
    /// </summary>
	/// Author: Intuitive Gaming Solutions
	public class StockfishChessAI : ChessAI
	{
        #region Public Properties
        /// <summary>Tracks whether or not stockfish is currently thinking of a move.</summary>
        public bool IsThinking { get; private set; }
        /// <summary>Returns true if 'bestmove' was the last line received and the next valid line is expected to be the actual move.</summary>
        public bool IsAwaitingBestMove { get; private set; }
        public MoveData BestMoveData { get; private set; }
        #endregion

        #region Constructor(s)
        public StockfishChessAI(ChessColor pTeam) : base(pTeam) { }
        #endregion

        #region Public Override Callback(s)
        /// <summary>
        /// Invoked when the AI is first initialized by the game.
        /// Recommended to subscribe to any relevant events here.
        /// </summary>
        public override void OnInitialized()
        {
            // Set defaults.
            IsThinking = false;
            IsAwaitingBestMove = false;
            BestMoveData = null;

            // Set the grace period to account for delays in receiving moves from Stockfish (the AI still won't get extra 'think' time).
            BestMoveSubmitGrace = 0.25f;

            // Subscribe to event(s).
            StockfishWrapper.OutputLineReceived += OnOutputLineReceived;
        }

        /// <summary>
        /// Invoked when the AI is deinitialized by the game.
        /// Recommended to unsubscribe to any relevant events here. (Doing so in the destructor is unreliable.)
        /// </summary>
        public override void OnDeinitialized()
        {
            // Unsubscribe from event(s).
            StockfishWrapper.OutputLineReceived -= OnOutputLineReceived;
        }

        /// <summary>
        /// Invoked after every 'OnUpdate' in any frame where 'IsBestMovePending == true' and 'IsBestMoveDelayed == false'.
        /// Logic to determine best moves should not be executed in this loop as a submission delay causes it to not be executed.
        /// It is best practice to submit ready best moves in this callback unless they were demanded, this will prevent any non-demanded submissions while there is a best move submission delay set.
        /// </summary>
        public override void OnBestMoveRequestUpdate()
        {
            // If no longer thinking then submit the best move.
            if (!IsThinking)
                SubmitBestMove();
        }

        /// <summary>Invoked when a best move is requested.</summary>
        /// <param name="pMaxDepth"></param>
        /// <param name="pMaxTime"></param>
        public override void OnBestMoveRequested(int pMaxDepth, float pMaxTime)
        {
            // Clear any current threads.
            StopThinking();

            // Thinking.
            IsThinking = true;

            // No best move.
            BestMoveData = null;

            // Update game state.
            StockfishWrapper.UCI.ProcessCommand("position fen " + ChessInstance.GenerateFENString());

            if (pMaxTime == 0)
            {
                StockfishWrapper.UCI.ProcessCommand("go depth " + pMaxDepth);
            }
            else { StockfishWrapper.UCI.ProcessCommand("go depth " + pMaxDepth + " movetime " + ConvertSecondsToMilliseconds(pMaxTime)); }
        }

        /// <summary>Invoked every frame that the AI is updated.</summary>
        public override void OnUpdate()
        {
            // Process the event queue for stockfish every update.
            StockfishWrapper.ProcessEventQueue();
        }

        /// <summary>Invoked after the AI demands a best move be submitted immediately.</summary>
        public override void OnBestMoveDemanded() { SubmitBestMove(); }

        /// <summary>Invoked after the AI submits a best move.</summary>
        /// <param name="pFrom"></param>
        /// <param name="pTo"></param>
        public override void OnBestMoveSubmitted(TileIndex pFrom, TileIndex pTo) { }
        #endregion

        #region Protected Method(s)
        /// <summary>Forces the AI to stop thinking about a move.</summary>
        protected void StopThinking()
        {
            // Not thinking.
            IsThinking = false;
        }
        
        /// <summary>Intended to be used as a fallback. Submits a ranodm best move.</summary>
        protected void SubmitBestMove()
        {
            // If there is valid best move data then submit that.
            if (BestMoveData != null)
            {
                SubmitBestMove(BestMoveData);
            }
            // Otherwise submit a random move.
            else
            {
                // Build a list of chess pieces whose color matches the AI team color in the main ChessInstance.
                List<ChessPiece> friendlyPieces = GetPieces(Team, ChessInstance);

                // Only submit a move if there is at least 1 valid piece.
                if (friendlyPieces.Count > 0)
                {
                    // Make a list of all ValidAttackEntrys that track all possible attacks.
                    List<MoveData> validAttackEntries = GetValidAttackEntries(friendlyPieces);

                    // If there is a valid attack then an attack is prefered over a move by Doofus.
                    if (validAttackEntries.Count > 0)
                    {
                        // Submit one of the attacks at random.
                        int attackEntryIndex = new Random().Next(0, validAttackEntries.Count);
                        SubmitBestMove(validAttackEntries[attackEntryIndex]);
                    }
                    else
                    {
                        // Make a list of ValidMoveEntrys that track all possible moves.
                        List<MoveData> validMoveEntries = GetValidMoveEntries(friendlyPieces);

                        // If there is a valid move then submit it at random.
                        if (validMoveEntries.Count > 0)
                        {
                            // Submit one of the moves at random.
                            int moveEntryIndex = new Random().Next(0, validMoveEntries.Count);
                            SubmitBestMove(validMoveEntries[moveEntryIndex]);
                        }
                    }

                    // Log warning.
                    Console.WriteLine("StockfishChessAI used a fallback random move! Stockfish should have submitted a 'bestmove' by now...");
                }
            }
        }

        /// <summary>Sets the pending 'best move data' using the given move string (i.e: e2e4).</summary>
        protected void SetBestMove(string pMove)
        {
            // No longer thinking.
            StopThinking();

            // Submit Stockfish's best move by parsing the pMove string.
            // Get move IDs.
            string moveFromID = pMove.Substring(0, 2);
            string moveToID = pMove.Substring(2, 2);

            // Get move tiles.
            ChessTableTile moveFromTile = ChessInstance.Table.GetTileByID(moveFromID);
            if (moveFromTile != null)
            {
                ChessTableTile moveToTile = ChessInstance.Table.GetTileByID(moveToID);
                if (moveToTile != null)
                {
                    // No longer awaiting best move.
                    IsAwaitingBestMove = false;

                    // Submit the move and return.
                    BestMoveData = new MoveData() { fromTileIndex = moveFromTile.TileIndex, toTileIndex = moveToTile.TileIndex };
                    return;
                }
            }

            // If we've reached this point in the function (hasn't returned) then pick the move at random.
            // Build a list of chess pieces whose color matches the AI team color in the main ChessInstance.
            List<ChessPiece> friendlyPieces = GetPieces(Team, ChessInstance);
            friendlyPieces.RemoveAll(p => p.IsPin);

            // Only submit a move if there is at least 1 valid piece.
            if (friendlyPieces.Count > 0)
            {
                // Make a list of all ValidAttackEntrys that track all possible attacks.
                List<MoveData> validAttackEntries = GetValidAttackEntries(friendlyPieces);

                // If there is a valid attack then an attack is prefered over a move by Doofus.
                if (validAttackEntries.Count > 0)
                {
                    // Submit one of the attacks at random.
                    int attackEntryIndex = new Random().Next(0, validAttackEntries.Count);
                    BestMoveData = validAttackEntries[attackEntryIndex];
                }
                else
                {
                    // Make a list of ValidMoveEntrys that track all possible moves.
                    List<MoveData> validMoveEntries = GetValidMoveEntries(friendlyPieces);

                    // If there is a valid move then submit it at random.
                    if (validMoveEntries.Count > 0)
                    {
                        // Submit one of the moves at random.
                        int moveEntryIndex = new Random().Next(0, validMoveEntries.Count);
                        BestMoveData = validMoveEntries[moveEntryIndex];
                    }
                }
            }

            // No longer awaiting best move.
            IsAwaitingBestMove = false;
        }

        /// <summary>Submits a 'best move' with the given move data.</summary>
        /// <param name="pMoveData"></param>
        protected void SubmitBestMove(MoveData pMoveData)
        {
            // No longer thinking.
            StopThinking();

            // Submit pMoveData.
            SubmitBestMove(pMoveData.fromTileIndex, pMoveData.toTileIndex);
        }
        #endregion

        #region Protected Virtual Callback(s)
        protected virtual void OnOutputLineReceived(string pLine)
        {
            // Only handle lines received during our turn.
            if (IsBestMovePending || IsBestMoveDelayed)
            {
                // Handle not awaiting best move (waiting for 'bestmove' line.)
                string trimmedLine = pLine.Trim();
                if (!IsAwaitingBestMove)
                {
                    if (trimmedLine == "bestmove")
                        IsAwaitingBestMove = true;
                }
                // Already received 'bestmove' line, waiting for actual valid move.
                else { SetBestMove(trimmedLine); }
            }
        }
        #endregion

        #region Public Static Conversion Method(s)
        /// <summary>Given a time as a float in seconds returns the unsigned long representation in milliseconds.</summary>
        /// <param name="pSeconds"></param>
        /// <returns></returns>
        public static ulong ConvertSecondsToMilliseconds(float pSeconds) { return (ulong)(pSeconds * 1000f); }
        #endregion
    }
}
