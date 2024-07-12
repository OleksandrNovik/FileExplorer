using FileExplorer.Models;
using Windows.Storage;

namespace FileExplorer.ViewModels.Messages
{
    public record NavigationRequiredMessage(StorageFolder NavigatedFolder);

    public record FileOpenRequiredMessage(StorageFile OpenFile);
    public record NewTabOpened(
        DirectoryNavigationModel TabDirectoryInfo,
        TabNavigationHistoryModel TabNavigationHistory);
}
