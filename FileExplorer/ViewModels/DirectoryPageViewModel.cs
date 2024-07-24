#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Contracts;
using FileExplorer.Helpers;
using FileExplorer.Models;
using FileExplorer.Models.StorageWrappers;
using FileExplorer.Services;
using FileExplorer.ViewModels.Messages;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DirectoryItemWrapper = FileExplorer.Models.StorageWrappers.DirectoryItemWrapper;

namespace FileExplorer.ViewModels
{
    public partial class DirectoryPageViewModel : ObservableRecipient, INavigationAware
    {
        private readonly IMenuFlyoutFactory menuFactory;

        private readonly ContextMenuMetadataBuilder menuBuilder;

        public CommonFileOperationsViewModel CommonFileOperationsViewModel { get; }

        [ObservableProperty]
        private DirectoryItemAdditionalInfo _selectedDirectoryItemAdditionalDetails;

        [ObservableProperty]
        private bool isDetailsShown;

        [ObservableProperty]
        private DirectoryWrapper currentDirectory;

        [ObservableProperty]
        private ObservableCollection<DirectoryItemWrapper> directoryItems;
        public ObservableCollection<DirectoryItemWrapper> SelectedItems { get; }
        public bool HasCopiedFiles { get; private set; }

        public DirectoryPageViewModel(CommonFileOperationsViewModel operationsVM, IMenuFlyoutFactory factory)
        {
            menuFactory = factory;
            CommonFileOperationsViewModel = operationsVM;
            menuBuilder = new ContextMenuMetadataBuilder(this);
            SelectedItems = [];
            SelectedItems.CollectionChanged += NotifyCommandsCanExecute;

            Messenger.Register<DirectoryPageViewModel, NavigationRequiredMessage>(this, HandleNavigationMessage);
            Messenger.Register<DirectoryPageViewModel, FileOpenRequiredMessage>(this, HandlerFileOpen);

            //TODO: Maybe Storage API can handle directory change
            Messenger.Register<DirectoryPageViewModel, DirectoryChangedMessage>(this, (_, message) =>
            {
                DirectoryItems.AppendFront(message.Added);
                DirectoryItems.RemoveCollection(message.Removed);
            });
        }

        private async void HandlerFileOpen(DirectoryPageViewModel recipient, FileOpenRequiredMessage message)
        {
            await message.OpenFile.LaunchAsync();
        }


        /// <summary>
        /// Handles navigation messages from <see cref="DirectoriesNavigationViewModel"/>
        /// and decides how to execute new navigation command
        /// </summary>
        /// <param name="receiver"> Message receiver (this) </param>
        /// <param name="massage"> Navigation message that contains new path </param>
        private async void HandleNavigationMessage(DirectoryPageViewModel receiver, NavigationRequiredMessage massage)
        {
            await MoveToDirectoryAsync(massage.NavigatedDirectory);
        }

        /// <summary>
        /// Notifies each command that require at least one selected item if they can execute 
        /// </summary>
        private void NotifyCommandsCanExecute(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OpenSelectedItemCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Method that should be called every time we navigate to a new directory
        /// It initializes collections of data in view model
        /// </summary>
        private async Task InitializeDirectoryAsync()
        {
            DirectoryItems = [];
            var directoryContent = CurrentDirectory.EnumerateItems()
                                                            .Where(i => (i.Attributes & FileAttributes.System) == 0)
                                                            .ToArray();
            await AddDirectoryItemsAsync(directoryContent);
            SelectedItems.Clear();
        }

        private async Task AddDirectoryItemsAsync(ICollection<DirectoryItemWrapper> items)
        {
            DirectoryItems.AddRange(items);

            foreach (var item in items)
            {
                if ((item.Attributes & FileAttributes.Hidden) == 0)
                {
                    await item.UpdateThumbnailAsync();
                }
            }
        }

        /// <summary>
        /// Changes current directory and initializes its items
        /// </summary>
        /// <param name="directory"> Given directory that is opened </param>
        private async Task MoveToDirectoryAsync(DirectoryWrapper directory)
        {
            CurrentDirectory = directory;
            CommonFileOperationsViewModel.CurrentDirectory = directory;
            await InitializeDirectoryAsync();
            Messenger.Send(directory);
        }

        #region Open logic

        [RelayCommand(CanExecute = nameof(HasSelectedItems))]
        private async Task OpenSelectedItem()
        {
            await Open(SelectedItems[0]);
        }

        [RelayCommand]
        private async Task Open(DirectoryItemWrapper item)
        {
            await EndRenamingIfNeededCommand.ExecuteAsync(item);

            switch (item)
            {
                case FileWrapper fileWrapper:
                    await fileWrapper.LaunchAsync();
                    break;
                case DirectoryWrapper directoryWrapper:
                    await MoveToDirectoryAsync(directoryWrapper);
                    var navigationModel = new DirectoryNavigationInfo(directoryWrapper);
                    Messenger.Send(navigationModel);
                    break;
                default:
                    throw new ArgumentException("Cannot open provided item. It is not a directory or file.", nameof(item));
            }
        }

        [RelayCommand]
        private void OpenInNewTab(DirectoryWrapper directory)
        {
            Messenger.Send(new OpenTabMessage(directory));
        }

        #endregion

        #region Renaming logic

        [RelayCommand]
        private void BeginRenamingItem(DirectoryItemWrapper item) => item.BeginEdit();

        private bool HasSelectedItems() => SelectedItems.Count > 0;


        /// <summary>
        /// Ends renaming item if it is actually possible
        /// </summary>
        /// <param name="item"> Item that has to be given new name </param>
        [RelayCommand]
        private async Task EndRenamingItem(DirectoryItemWrapper item)
        {
            await CommonFileOperationsViewModel.EndRenamingItemCommand.ExecuteAsync(item);
            //TODO: Sort Items
        }

        public IAsyncRelayCommand<DirectoryItemWrapper> EndRenamingIfNeededCommand =>
            CommonFileOperationsViewModel.EndRenamingIfNeededCommand;

        #endregion

        [RelayCommand]
        private async Task RecycleItem(DirectoryItemWrapper item)
        {
            await CommonFileOperationsViewModel.TryDeleteItem(item, false);
        }

        [RelayCommand]
        private async Task CreateFile()
        {
            var file = await CommonFileOperationsViewModel.CreateFile();
            OnItemCreation(file);
        }
        [RelayCommand]
        private async Task CreateDirectory()
        {
            var dir = await CommonFileOperationsViewModel.CreateDirectory();
            OnItemCreation(dir);
        }

        private void OnItemCreation(DirectoryItemWrapper item)
        {
            DirectoryItems.Insert(0, item);
            BeginRenamingItem(item);
        }

        [RelayCommand]
        private void PasteItems() { }

        [RelayCommand]
        private void CopyItem(DirectoryItemWrapper item) { }

        [RelayCommand]
        private void CutItem(DirectoryItemWrapper item) { }

        [RelayCommand]
        private async Task ShowDetails(DirectoryItemWrapper item)
        {
            SelectedDirectoryItemAdditionalDetails = await CommonFileOperationsViewModel.ShowDetailsAsync(item);
            IsDetailsShown = true;
        }

        [RelayCommand]
        private void CloseDetailsMenu() => IsDetailsShown = false;

        public async void OnNavigatedTo(object parameter)
        {
            if (parameter is TabModel tab)
            {
                await MoveToDirectoryAsync(tab.TabDirectory);
                var navigationInfo = new DirectoryNavigationInfo(tab.TabDirectory);
                Messenger.Send(new NewTabOpened(navigationInfo, tab.TabHistory));
                Messenger.Send(new InitializeToolBarMessage(CurrentDirectory, SelectedItems));
            }
        }

        [RelayCommand]
        private async Task Refresh()
        {
            //TODO: Fix this later
            await MoveToDirectoryAsync(CurrentDirectory);
        }

        public void OnNavigatedFrom()
        {
            throw new NotImplementedException();
        }

        public List<MenuFlyoutItemBase> OnContextMenuRequired()
        {
            List<MenuFlyoutItemViewModel> menu;

            if (HasSelectedItems())
            {
                menu = menuBuilder.BuildMenuForItem(SelectedItems[0]);
            }
            else
            {
                menu = menuBuilder.BuildDefaultMenu();
            }

            return menuFactory.Create(menu);
        }

    }
}
