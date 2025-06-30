using System;
using System.Runtime.InteropServices;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using ChessEngine.AI.Stockfish.Threading;

namespace ChessEngine.AI.Stockfish
{
    public static class StockfishWrapper
    {
        #region Cross Platform Constant(s)
#if UNITY_IOS
		public const string TARGET = "__Internal";
#else
        public const string TARGET = "stockfish";
#endif
        #endregion
        #region DLL Delegates
        public delegate void LineProcessedDelegate(string pLine);
        public delegate void SetOptionDelegate(string pOption, string pValue);
        #endregion
        #region DLL Import
        // SECTION: Stockfish wrapper method(s).
        [DllImport(TARGET, CallingConvention = CallingConvention.Cdecl)]
        static extern bool is_initialized();

        [DllImport(TARGET, CallingConvention = CallingConvention.Cdecl)]
        static extern void initialize();

        [DllImport(TARGET, CallingConvention = CallingConvention.Cdecl)]
        static extern void deinitialize();

        [DllImport(TARGET, CallingConvention = CallingConvention.Cdecl)]
        static extern bool is_uci_initialized();

        [DllImport(TARGET, CallingConvention = CallingConvention.Cdecl)]
        static extern void initialize_uci();

        [DllImport(TARGET, CallingConvention = CallingConvention.Cdecl)]
        static extern void set_option([MarshalAs(UnmanagedType.LPStr)] string pOption, [MarshalAs(UnmanagedType.LPStr)] string pValue);

        [DllImport(TARGET, CallingConvention = CallingConvention.Cdecl)]
        static extern void register_output_line_callback(LineProcessedDelegate pCallback);
        #endregion

        #region Static Constructor
        static StockfishWrapper()
        {
            // If there is a dispatcher this has already been initialized, initialize only if there is no dispatcher.
            if (m_Dispatcher == null)
            {
                // Setup the dispatcher.
                m_Dispatcher = new StockfishMainThreadDispatcher();
                m_Dispatcher.EventDispatched += OnEventDispatched;

                // Register output line callback.
                RegisterOutputLineCallback(OnOutputLineReceived);
            }
        }
        #endregion

        #region Static Event(s)
        /// <summary>An event that is invoked whenever an output line is received by the 'static' callback.</summary>
        public static Action<string> OutputLineReceived;
        #endregion

        #region Static Stockfish Callback(s)
        /// <summary>WARNING: This callback is generally invoked on a background thread, hence the use of the thread dispatcher.</summary>
        /// <param name="pLine"></param>
#if UNITY_ANDROID || UNITY_IOS
        [AOT.MonoPInvokeCallback(typeof(LineProcessedDelegate))]
#endif
        static void OnOutputLineReceived(string pLine)
        {
            if (pLine != null)
                m_Dispatcher.Enqueue(pLine);
        }

        /// <summary>Invoked whenever an event from the main thread dispatcher is processed. This is invoked on the main thread.</summary>
        /// <param name="pLine"></param>
        static void OnEventDispatched(string pLine)
        {
            // Invoke the 'OutputLineReceived' event on the main thread.
            OutputLineReceived?.Invoke(pLine);
        }
        #endregion

        #region Private Static field(s)
        /// <summary>The main thread dispatcher used to dispatch commands from a background thread to the main thread.</summary>
        static StockfishMainThreadDispatcher m_Dispatcher;
        #endregion

        #region Public Properties
        /// <summary>Returns true if the stockfish wrapper is initialized, otherwise false.</summary>
        public static bool IsInitialized { get { return is_initialized(); } }
        /// <summary>Returns true if the stockfish wrapper has initialized UCI, otherwise false.</summary>
        public static bool IsUciInitialized { get { return is_uci_initialized(); } }
        #endregion
        #region Public Method(s)
        /// <summary>Initializes the stockfish wrapper if not already initialized. Starts a new game.</summary>
        public static void Initialize()
        {
            if (!IsInitialized)
            {
                initialize();

                // Assign a stockfish clean up event for when the program exits.
                if (!Application.isEditor)
                {
                    Application.quitting += () => { Deinitialize(); };
                }
#if UNITY_EDITOR
                else { EditorApplication.quitting += () => { Deinitialize(); }; }
#endif
            }
        }

        /// <summary>Deinitializes stockfish. Keep in mind that Stockfish was not designed to be reinitialized after being 'cleaned up', this should be done before application exit.</summary>
        public static void Deinitialize() 
        { 
            if (IsInitialized)
                deinitialize();
        }

        /// <summary>Initializes UCI if it hasn't already been initialized.</summary>
        public static void InitializeUci()
        {
            if (!IsUciInitialized)
                initialize_uci();
        }

        /// <summary>
        /// Sets a Stockfish Options option by name and value.
        /// </summary>
        /// <param name="pOption"></param>
        /// <param name="pValue"></param>
        public static void SetOption(string pOption, string pValue)
        {
            set_option(pOption, pValue);
        }

        /// <summary>
        /// Converts the relative NNUE path given as pPath to a full
        /// path relative to Application.dataPath and returns the value.
        /// </summary>
        /// <param name="pPath"></param>
        /// <param name="pPathMode">The StockfishPathMode to use.</param>
        /// <returns>pPath as a full path relative to Application.dataPath.</returns>
        public static string GetFullNNUEPath(string pPath, StockfishPathMode pPathMode)
        {
            // Combine Application.dataPath with nnuePath to get the full path.
            string fullPath = System.IO.Path.Combine(StockfishFileSystem.GetPathByMode(pPathMode), pPath);

            // Convert backslashes to forward slashes (Stockfish and many systems prefer forward slashes).
            fullPath = fullPath.Replace("\\", "/");

            return fullPath;
        }

        /// <summary>Should be invoked at least once per frame while the wrapper is active.</summary>
        public static void ProcessEventQueue()
        {
            // Process the output queue.
            m_Dispatcher.ProcessQueue();
        }

        /// <summary>Clearsthe event queue discarding any pending events.</summary>
        public static void ClearEventQueue()
        {
            // Clear the output queue.
            m_Dispatcher.ClearQueue();
        }

        /// <summary>Registers the callback that receives output lines from stockfish.</summary>
        /// <param name="pCallback"></param>
        public static void RegisterOutputLineCallback(LineProcessedDelegate pCallback) { register_output_line_callback(pCallback); }
        #endregion

        // SECTION: Stockfish UCI wrapper.
        public static class UCI
        {
            #region UCI DLL Import
            [DllImport(TARGET, CallingConvention = CallingConvention.Cdecl)]
            static extern void process_command([MarshalAs(UnmanagedType.LPStr)] string cmd);
            #endregion

            #region UCI Public Method(s)
            /// <summary>Sends a command to Stockfish to be processed.</summary>
            /// <param name="pCommand"></param>
            public static void ProcessCommand([MarshalAs(UnmanagedType.LPStr)] string pCommand) { process_command(pCommand); }
            #endregion
        }
    }
}
