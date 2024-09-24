using FileExplorer.ViewModels.General;

namespace FileExplorer.ViewModels
{
    public sealed class ViewModelsLocator
    {
        static ViewModelsLocator()
        {
            StorageNaming = App.GetService<StorageItemsNamingViewModel>();
            FileOperations = App.GetService<FileOperationsViewModel>();
            ViewOptions = App.GetService<ViewOptionsViewModel>();
        }
        public static StorageItemsNamingViewModel StorageNaming { get; }
        public static FileOperationsViewModel FileOperations { get; }
        public static ViewOptionsViewModel ViewOptions { get; }
    }
}
