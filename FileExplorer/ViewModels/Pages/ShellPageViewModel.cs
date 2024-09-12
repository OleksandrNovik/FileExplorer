﻿#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.DirectoriesNavigation;
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.Models;
using FileExplorer.Models.Contracts.Storage;
using FileExplorer.Models.Messages;
using FileExplorer.Models.ModelHelpers;
using FileExplorer.Models.Navigation;
using FileExplorer.Models.Storage.Additional;
using FileExplorer.Models.Storage.Windows;
using FileExplorer.Models.TabRelated;
using FileExplorer.ViewModels.General;
using FileExplorer.ViewModels.Search;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NavigationPaneViewModel = FileExplorer.ViewModels.Controls.NavigationPaneViewModel;

namespace FileExplorer.ViewModels.Pages
{
    public sealed partial class ShellPageViewModel : ObservableRecipient, IMenuFlyoutBuilder
    {
        public NavigationPaneViewModel NavigationPaneViewModel { get; } = new();
        public FileOperationsViewModel FileOperationsViewModel { get; }
        public SearchOperationViewModel SearchOperationViewModel { get; } = new();
        public ITabService TabService { get; }
        public INavigationService NavigationService { get; }

        [ObservableProperty]
        private ObservableCollection<TabModel> tabs;

        public ShellPageViewModel(ITabService tabService, INavigationService navigationService, FileOperationsViewModel fileOperations)
        {
            TabService = tabService;
            NavigationService = navigationService;
            tabs = TabService.Tabs;
            FileOperationsViewModel = fileOperations;

            Messenger.Register<ShellPageViewModel, TabStorageChangedMessage>(this, (_, message) =>
            {
                TabService.SelectedTab.OpenedStorage = message.Storage;
            });

            Messenger.Register<ShellPageViewModel, OpenTabMessage>(this, (_, message) =>
            {
                NewTab(message.TabStorage);
            });

            Messenger.Register<ShellPageViewModel, NavigationRequiredMessage>(this, (_, message) =>
            {
                NavigationService.NavigateTo(message.NavigatedStorage.ContentType, message.NavigatedStorage);
            });

            Messenger.Register<ShellPageViewModel, SearchStorageMessage>(this, SearchStorage);
        }

        private async void SearchStorage(ShellPageViewModel recipient, SearchStorageMessage message)
        {
            // Initializing search data before initiating search
            var destination = new ConcurrentWrappersCollection();
            //var searchOptions = SearchOptions.CreateDefault(message.Options, destination);
            var searchOptions = new SearchOptions(message.Options, destination);
            SearchOperationViewModel.InitializeSearchData(message.Storage, searchOptions);

            //Mark cached search result as opened 
            TabService.SelectedTab.OpenedStorage = SearchOperationViewModel.CachedSearch;
            // Create navigation parameter that contains destination collection and navigated storage item
            var navParameter = new SearchStorageTransferObject(SearchOperationViewModel.CachedSearch, destination);

            NavigationService.NavigateTo(SearchOperationViewModel.CachedSearch.ContentType, navParameter);

            await SearchOperationViewModel.SearchAsync();
        }

        [RelayCommand]
        private void OpenNewTab(IStorage storage)
        {
            NewTab(storage);
        }

        private void NewTab(IStorage storage)
        {
            TabService.CreateNewTab(storage);
        }

        private void NavigateToTab(TabModel item)
        {
            TabService.SelectedTab = item;
            NavigationService.NavigateTo(item.OpenedStorage.ContentType, item.OpenedStorage);
            NavigationService.NotifyTabOpened(item);

            Messenger.Send(new TabOpenedMessage(item.OpenedStorage, item.TabHistory));
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

        [RelayCommand]
        private void SelectMenuItem(IStorage selectedStorage)
        {
            Messenger.Send(new StorageNavigatedMessage(selectedStorage));
        }

        public IReadOnlyList<MenuFlyoutItemViewModel> BuildMenu(object? parameter = null)
        {
            List<MenuFlyoutItemViewModel> menu = new();

            if (parameter is NavigationItemModel navigationModel)
            {
                if (string.IsNullOrEmpty(navigationModel.Path))
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

                menu.WithDetails(FileOperationsViewModel.ShowDetailsCommand, directory);
            }

            return menu;
        }
    }
}
