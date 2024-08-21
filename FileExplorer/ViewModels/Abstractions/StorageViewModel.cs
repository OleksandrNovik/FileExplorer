using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.ViewModels.General;
using Models.Contracts.Storage;
using Models.Messages;
using Models.Storage.Windows;
using System;

namespace FileExplorer.ViewModels.Abstractions
{
    /// <summary>
    /// View model that does every operation that is needed to provide functionality to a storage page (directory or drives for example)
    /// </summary>
    public abstract class StorageViewModel : ContextMenuCreatorViewModel, INavigationAware, ISearchingViewModel
    {
        /// <summary>
        /// Storage that is opened in storage page
        /// </summary>
        public IStorage<DirectoryItemWrapper> Storage { get; set; }
        protected StorageViewModel(IMenuFlyoutFactory factory) : base(factory)
        {
            Messenger.Register<StorageViewModel, SearchOperationRequiredMessage>(this, HandleSearchMessage);
        }

        public virtual void OnNavigatedTo(object parameter)
        {
            if (parameter is IStorage<DirectoryItemWrapper> storage)
            {
                Storage = storage;
                Messenger.Send(new TabStorageChangedMessage(storage));
            }
        }

        public abstract void OnNavigatedFrom();

        public virtual void HandleSearchMessage(ObservableRecipient recipient, SearchOperationRequiredMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
