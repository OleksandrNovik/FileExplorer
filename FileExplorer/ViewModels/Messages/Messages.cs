using FileExplorer.Models;
using Windows.Storage;

namespace FileExplorer.ViewModels.Messages
{
    public record NavigationRequiredMessage(DirectoryItemWrapper OpenedItem);

    public record FileOpenRequiredMessage(StorageFile OpenFile);
    public record NewTabOpened(
        DirectoryNavigationInfo TabDirectoryInfo,
        TabNavigationHistoryModel TabNavigationHistory);
}
