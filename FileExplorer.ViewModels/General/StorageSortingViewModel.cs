using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts.Storage;
using FileExplorer.Helpers;
using FileExplorer.Models;
using FileExplorer.Models.Contracts.Storage.Directory;
using FileExplorer.Models.General;
using FileExplorer.Models.Messages;
using FileExplorer.Models.Storage.Additional;
using System;
using System.Collections.Generic;

namespace FileExplorer.ViewModels.General
{
    public sealed partial class StorageSortingViewModel : ObservableRecipient
    {
        private static class SortingOptions
        {
            public static readonly SortProperty<IDirectoryItem, string> Name;
            public static readonly SortProperty<IDirectoryItem, DateTime> AccessDate;
            public static readonly SortProperty<IDirectoryItem, ByteSize> Size;

            static SortingOptions()
            {
                Name = new SortProperty<IDirectoryItem, string>(item => item.Name);
                AccessDate = new SortProperty<IDirectoryItem, DateTime>(item => item.LastAccess);
                Size = new SortProperty<IDirectoryItem, ByteSize>(item => item.Size ?? ByteSize.Empty);
            }
        }

        private readonly IStorageSortingService sortingService;

        private ISortProperty CurrentProperty;

        public StorageSortingViewModel(IStorageSortingService sortingService)
        {
            this.sortingService = sortingService;
        }

        public ICollection<IDirectoryItem> SortByDefault(IDirectory storage)
        {
            // TODO: Save default sorting value
            return Sort(storage, SortingOptions.Name);
        }

        [RelayCommand]
        private void SortByName(IDirectory directory)
        {
            var items = Sort(directory, SortingOptions.Name);
            Messenger.Send(new SortExecutedMessage(items));
        }

        [RelayCommand]
        private void SortByDate(IDirectory directory)
        {
            var items = Sort(directory, SortingOptions.AccessDate);
            Messenger.Send(new SortExecutedMessage(items));
        }

        [RelayCommand]
        private void SortBySize(IDirectory directory)
        {
            var items = Sort(directory, SortingOptions.Size);
            Messenger.Send(new SortExecutedMessage(items));
        }

        private ICollection<IDirectoryItem> Sort<TKey>(IDirectory directory, SortProperty<IDirectoryItem, TKey> property)
        {
            ICollection<IDirectoryItem> sorted;

            if (property != CurrentProperty)
            {
                sorted = sortingService.SortByKey(directory, property.Func);
            }
            else
            {
                sorted = sortingService.SortByKeyDescending(directory, property.Func);
            }
            CurrentProperty = property;
            return sorted;
        }

        public MenuFlyoutItemViewModel BuildSortOptions(IDirectory directory)
        {
            return new MenuFlyoutItemViewModel("Sort")
            {
                Items =
                    [
                        new MenuFlyoutItemViewModel("Name")
                        {
                            Command = SortByNameCommand,
                            CommandParameter = directory,
                            IconGlyph = SortingOptions.Name == CurrentProperty ?
                                    Constants.FluentIcons.Down : Constants.FluentIcons.Up
                        },
                        new MenuFlyoutItemViewModel("Last access")
                        {
                            Command = SortByDateCommand,
                            CommandParameter = directory,
                            IconGlyph = SortingOptions.AccessDate == CurrentProperty ?
                                    Constants.FluentIcons.Down : Constants.FluentIcons.Up
                        },
                        new MenuFlyoutItemViewModel("Size")
                        {
                            Command = SortBySizeCommand,
                            CommandParameter = directory,
                            IconGlyph = SortingOptions.Size == CurrentProperty ?
                                    Constants.FluentIcons.Down : Constants.FluentIcons.Up
                        }
                    ],
            };
        }
    }
}
