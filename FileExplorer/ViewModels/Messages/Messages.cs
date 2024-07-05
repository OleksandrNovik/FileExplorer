using FileExplorer.Models;

namespace FileExplorer.ViewModels.Messages
{
    public record NavigationRequiredMessage(string NavigationPath);

    public record NewTabOpened(
        DirectoryNavigationModel TabDirectoryInfo,
        TabNavigationHistoryModel TabNavigationHistory);
}
