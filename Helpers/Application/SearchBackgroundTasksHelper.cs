using Windows.ApplicationModel.Background;

namespace Helpers.Application
{
    public static class SearchBackgroundTasksHelper
    {
        private const string SearchTask = "SearchBackgroundTask";
        private static bool isSearchTaskRegistered;
        public static void Register(ApplicationTrigger taskTrigger)
        {
            foreach (var task in BackgroundTaskRegistration.AllTasks.Values)
            {
                if (task.Name == SearchTask)
                {
                    isSearchTaskRegistered = true;
                    break;
                }
            }

            if (!isSearchTaskRegistered)
            {
                var builder = new BackgroundTaskBuilder
                {
                    Name = SearchTask,
                    TaskEntryPoint = $"FileExplorer.BackgroundTasks.{SearchTask}",
                };

                builder.SetTrigger(taskTrigger);
                //builder.AddCondition(new SystemCondition(SystemConditionType.BackgroundWorkCostNotHigh));
                builder.Register();
            }
        }
    }
}
