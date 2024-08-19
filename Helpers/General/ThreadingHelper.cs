using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using System;
using System.Threading.Tasks;

namespace Helpers.General
{
    public static class ThreadingHelper
    {
        private static readonly DispatcherQueue dispatcher = DispatcherQueue.GetForCurrentThread();

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
