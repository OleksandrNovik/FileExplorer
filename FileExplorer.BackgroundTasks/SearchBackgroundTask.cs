using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;

namespace FileExplorer.BackgroundTasks
{
    public sealed class SearchBackgroundTask : IBackgroundTask
    {
        //public const string SourceKey = "List",
        //                    CancellationKey = "Cancel",
        //                    OptionsKey = "SearchOptions";

        private BackgroundTaskDeferral deferral;
        private AppServiceConnection connection;
        private CancellationTokenSource operationCancellation;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnCanceled;

            Task.Delay(5000);

            deferral.Complete();
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            //TODO: Get user notified that system has low battery or some other reason
            Cancel();
        }

        private void Cancel()
        {
            connection.Dispose();
            operationCancellation.Cancel();
            deferral.Complete();
        }
        private void InitiateSearch()
        {
        }
    }

}
