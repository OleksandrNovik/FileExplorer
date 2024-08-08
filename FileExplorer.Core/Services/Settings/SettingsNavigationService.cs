#nullable enable
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.Core.Services.General;
using Helpers.Application;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace FileExplorer.Core.Services.Settings
{
    public sealed class SettingsNavigationService : BaseNavigationService, ISettingsNavigationService
    {
        private readonly ISettingsPagesService pagesService;

        public SettingsNavigationService(ISettingsPagesService pagesService)
        {
            this.pagesService = pagesService;
        }

        public void NavigateTo(string tag)
        {
            var pageType = pagesService.GetPage(tag);
            NavigateToPage(pageType);
        }

        public void NavigateToDefault()
        {
            var defaultPageType = pagesService.GetDefaultPage();
            NavigateToPage(defaultPageType);
        }

        private void NavigateToPage(Type pageType)
        {
            ArgumentNullException.ThrowIfNull(Frame);

            var previousViewModel = Frame.GetPageViewModel();

            bool navigated = Frame.Navigate(pageType);

            if (navigated)
            {
                if (previousViewModel is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedFrom();
                }
            }
        }

        protected override void OnNavigated(object sender, NavigationEventArgs e)
        {
            base.OnNavigated(sender, e);
            Navigated?.Invoke(sender, e);
        }

        public event NavigatedEventHandler? Navigated;
    }
}
