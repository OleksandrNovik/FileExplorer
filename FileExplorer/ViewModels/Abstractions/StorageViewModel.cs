using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.ViewModels.General;
using Models.Contracts.Storage;
using Models.Messages;

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
        public IStorage<IDirectoryItem> Storage { get; set; }
        protected StorageViewModel(IMenuFlyoutFactory factory) : base(factory)
        {
            Messenger.Register<StorageViewModel, SearchOperationRequiredMessage>(this, HandleSearchMessage);
        }

        /// <summary>
        /// Gets <see cref="Storage"/> from navigation parameter if possible
        /// </summary>
        /// <param name="parameter"> Navigation parameter that is provided by navigation service </param>
        public virtual void OnNavigatedTo(object parameter)
        {
            if (parameter is IStorage<IDirectoryItem> storage)
            {
                Messenger.Send(new StopSearchMessage());
                NavigateStorage(storage);
            }
        }

        /// <summary>
        /// Navigates storage item and sends message for tab to changed tab's storage item
        /// </summary>
        /// <param name="storage"> Storage that is navigated </param>
        protected void NavigateStorage(IStorage<IDirectoryItem> storage)
        {
            Storage = storage;
            Messenger.Send(new TabStorageChangedMessage(storage));
        }

        public virtual void OnNavigatedFrom()
        {
            Messenger.UnregisterAll(this);
        }

        public virtual void HandleSearchMessage(ObservableRecipient recipient, SearchOperationRequiredMessage message)
        {
            Messenger.Send(new SearchStorageMessage(Storage, message.Options));
        }
    }
}
