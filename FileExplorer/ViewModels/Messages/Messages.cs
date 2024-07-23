using FileExplorer.Models;
using FileExplorer.Models.StorageWrappers;

namespace FileExplorer.ViewModels.Messages
{
    public record NavigationRequiredMessage(DirectoryWrapper NavigatedDirectory);

    public record FileOpenRequiredMessage(FileWrapper OpenFile);
    public record NewTabOpened(
        DirectoryNavigationInfo TabDirectoryInfo,
        TabNavigationHistoryModel TabNavigationHistory);

    public record OpenTabMessage(DirectoryWrapper TabDirectory);
}
