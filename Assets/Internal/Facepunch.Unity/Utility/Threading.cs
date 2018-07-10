using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facepunch
{
    public static class Threading
    {
        private static int mainThread;
        private static List<Action> actions = new List<Action>();

        /// <summary>
        /// Returns true if this is the main thread
        /// </summary>
        public static bool IsMainThread
        {
            get { return System.Threading.Thread.CurrentThread.ManagedThreadId == mainThread; }
        }

        /// <summary>
        /// Run a function on the main thread. This will run on the next update.
        /// </summary>
        /// <param name="action"></param>
        public static void QueueOnMainThread( Action action )
        {
            if ( IsMainThread )
            {
                action();
                return;
            }

            lock ( actions )
            {
                actions.Add( action );
            }
        }

        /// <summary>
        /// Should get called every frame to run queue'd functions
        /// </summary>
        internal static void RunQueuedFunctionsOnMainThread()
        {
            mainThread = System.Threading.Thread.CurrentThread.ManagedThreadId;

            lock ( actions )
            {
                foreach ( var a in actions )
                {
                    a();
                }

                actions.Clear();
            }
        }
    }
}
