#nullable enable
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.Core.Services.General;
using Microsoft.UI.Xaml.Navigation;

namespace FileExplorer.Core.Services.Settings
{
    public sealed class SettingsNavigationService : BaseNavigationService, ISettingsNavigationService
    {
        private readonly IBasicPageService pagesService;

        public SettingsNavigationService(IBasicPageService pagesService)
        {
            this.pagesService = pagesService;
        }

        public void NavigateTo(string? tag)
        {
            var pageType = pagesService.GetPage(tag);
            UseNavigationFrame(pageType);
        }

        protected override void OnNavigated(object sender, NavigationEventArgs e)
        {
            base.OnNavigated(sender, e);
            Navigated?.Invoke(sender, e);
        }

        public event NavigatedEventHandler? Navigated;
    }
}
