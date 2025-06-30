using System;
using System.Collections.Concurrent;

namespace ChessEngine.AI.Stockfish.Threading
{
    /// <summary>
    /// A component that is responsible for dispatching events on the main thread that were received not on the main thread.
    /// </summary>
    /// Author: Intuitive Gaming Solutions
    public class StockfishMainThreadDispatcher
    {
        /// <summary>The concurrent queue that holds undispatched data.</summary>
        ConcurrentQueue<string> m_Queue = new ConcurrentQueue<string>();

        // C# events.
        /// <summary>
        /// An event that is dispatched when an event from the another thread is received and processed on the main thread.
        /// Arg0: string - the data that was received.
        /// </summary>
        public Action<string> EventDispatched;

        // Public method(s).
        /// <summary>Enqueues data to the main thread dispatcher.</summary>
        /// <param name="pData"></param>
        public void Enqueue(string pData)
        {
            m_Queue.Enqueue(pData);
        }

        /// <summary>Process the queue from the main thread.</summary>
        public void ProcessQueue()
        {
            while (m_Queue.TryDequeue(out string pData))
            {
                // Dispatch the pData on the main thread
                DispatchOnMainThread(pData);
            }
        }

        /// <summary>Clears the queue. Only to be invoked from the main thread.</summary>
        public void ClearQueue()
        {
            m_Queue = new ConcurrentQueue<string>();
        }

        // Private method(s).
        /// <summary>Dispatches an event on the main thread.</summary>
        /// <param name="pData"></param>
        void DispatchOnMainThread(string pData)
        {
            // Invoke the 'EventDispatched' event.
            EventDispatched?.Invoke(pData);
        }
    }
}
