#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.DirectoriesNavigation;
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.ViewModels.General;
using Microsoft.UI.Xaml.Controls;
using Models;
using Models.Messages;
using Models.ModelHelpers;
using Models.Navigation;
using Models.Storage.Windows;
using Models.TabRelated;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FileExplorer.ViewModels
{
    public sealed partial class ShellPageViewModel : ContextMenuCreatorViewModel
    {
        public NavigationPaneViewModel NavigationPaneViewModel { get; } = new();
        public FileOperationsViewModel FileOperationsViewModel { get; } = new();

        public ITabService TabService { get; }
        public INavigationService NavigationService { get; }

        [ObservableProperty]
        private ObservableCollection<TabModel> tabs;

        public ShellPageViewModel(IMenuFlyoutFactory factory, ITabService tabService, INavigationService navigationService)
            : base(factory)
        {
            TabService = tabService;
            NavigationService = navigationService;
            tabs = TabService.Tabs;

            Messenger.Register<ShellPageViewModel, TabDirectoryChangedMessage>(this, (_, message) =>
            {
                TabService.SelectedTab.TabDirectory = message.Directory;
                NavigationPaneViewModel.SetIcons();
            });

            Messenger.Register<ShellPageViewModel, OpenTabMessage>(this, (_, message) =>
            {
                NewTab(message.TabDirectory);
            });
        }

        [RelayCommand]
        private void OpenNewTab(DirectoryWrapper? directory = null)
        {
            NewTab(directory);
        }

        private void NewTab(DirectoryWrapper? directory = null)
        {
            TabService.CreateNewTab(directory);
        }

        private void NavigateToTab(TabModel item)
        {
            TabService.SelectedTab = item;
            NavigationService.NavigateTo(item.TabDirectory?.Path, item.TabDirectory);
            NavigationService.NotifyTabOpened(item);
        }

        [RelayCommand]
        private void RemoveTab(TabModel item)
        {
            Tabs.Remove(item);
        }

        [RelayCommand]
        private void SelectTab(TabModel item)
        {
            NavigateToTab(item);
        }

        /// <inheritdoc />
        public override IList<MenuFlyoutItemBase> BuildContextMenu(object? parameter = null)
        {
            List<MenuFlyoutItemViewModel> menu = new();

            if (parameter is NavigationItemModel navigationModel)
            {
                if (navigationModel.Path is null)
                    return [];

                var directory = new DirectoryWrapper(navigationModel.Path);

                menu.WithOpen(FileOperationsViewModel.OpenCommand, directory)
                    .WithOpenInNewTab(FileOperationsViewModel.OpenInNewTabCommand, directory);

                if (navigationModel.IsPinned)
                {
                    menu.WithUnpin(FileOperationsViewModel.UnpinCommand, directory);
                }
                else
                {
                    menu.WithPin(FileOperationsViewModel.PinCommand, directory);
                }

                menu.WithCopy(FileOperationsViewModel.CopyCommand, directory)
                    .WithDetails(FileOperationsViewModel.ShowDetailsCommand, directory);
            }

            return menuFactory.Create(menu);
        }
    }
}
