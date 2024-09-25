using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using System;
using System.Threading.Tasks;

namespace FileExplorer.Helpers.General
{
    public static class ThreadingHelper
    {
        private static DispatcherQueue dispatcher;

        public static void InitializeForMainThread()
        {
            dispatcher = DispatcherQueue.GetForCurrentThread();
        }

        public static bool TryEnqueue(Action action)
        {
            return dispatcher.TryEnqueue(new DispatcherQueueHandler(action));
        }

        /// <summary>
        /// Runs asynchronous call on UI thread
        /// </summary>
        /// <param name="func"> Callback that is run on main (UI) thread </param>
        public static async Task EnqueueAsync(Func<Task> func)
        {
            await dispatcher.EnqueueAsync(func);
        }
    }
}
