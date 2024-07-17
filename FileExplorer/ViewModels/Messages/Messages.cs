using FileExplorer.Models;

namespace FileExplorer.ViewModels.Messages
{
    public record NavigationRequiredMessage(DirectoryWrapper NavigatedDirectory);

    public record FileOpenRequiredMessage(FileWrapper OpenFile);
    public record NewTabOpened(
        DirectoryNavigationInfo TabDirectoryInfo,
        TabNavigationHistoryModel TabNavigationHistory);
}
