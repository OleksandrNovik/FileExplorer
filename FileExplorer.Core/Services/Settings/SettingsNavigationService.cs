#nullable enable
using FileExplorer.Core.Contracts.General;
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.Core.Services.General;
using FileExplorer.Helpers.Application;

namespace FileExplorer.Core.Services.Settings
{
    /// <summary>
    /// Service that handles navigation inside of settings modal window
    /// </summary>
    public sealed class SettingsNavigationService : GenericNavigationService<string>, ISettingsNavigationService
    {
        public SettingsNavigationService(IBasicPageService<string> pagesService) : base(pagesService) { }

        /// <inheritdoc />
        public void NavigateFromCurrent()
        {
            NavigateFrom(Frame.GetPageViewModel());
        }
    }
}
