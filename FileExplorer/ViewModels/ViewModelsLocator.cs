using FileExplorer.ViewModels.General;

namespace FileExplorer.ViewModels
{
    public sealed class ViewModelsLocator
    {
        static ViewModelsLocator()
        {
            StorageNamingViewModel = App.GetService<StorageItemsNamingViewModel>();
        }

        public static StorageItemsNamingViewModel StorageNamingViewModel { get; }
    }
}
