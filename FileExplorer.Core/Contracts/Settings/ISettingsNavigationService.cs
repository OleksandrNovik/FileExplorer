#nullable enable
using FileExplorer.Core.Contracts.General;

namespace FileExplorer.Core.Contracts.Settings
{
    /// <summary>
    /// Special interface for navigation service in settings window
    /// </summary>
    public interface ISettingsNavigationService : IBasicNavigationService<string>;
}
