using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.ViewModels.General;
using FileExplorer.ViewModels.Search;
using Models.Contracts.Storage;
using Models.Messages;

namespace FileExplorer.ViewModels.Abstractions
{
    /// <summary>
    /// View model that does every operation that is needed to provide functionality to a storage page (Directory or drives for example)
    /// </summary>
    public abstract class StorageViewModel : ContextMenuCreatorViewModel, INavigationAware, ISearchingViewModel
    {
        /// <summary>
        /// View model that contains file operation logic
        /// </summary>
        public FileOperationsViewModel FileOperations { get; }

        /// <summary>
        /// Storage that is opened in storage page
        /// </summary>
        public IStorage Storage { get; set; }
        protected StorageViewModel(FileOperationsViewModel fileOperations, IMenuFlyoutFactory factory) : base(factory)
        {
            FileOperations = fileOperations;
            Messenger.Register<StorageViewModel, SearchOperationRequiredMessage>(this, HandleSearchMessage);
        }

        /// <summary>
        /// Gets <see cref="Storage"/> from navigation parameter if possible
        /// </summary>
        /// <param name="parameter"> Navigation parameter that is provided by navigation service </param>
        public virtual void OnNavigatedTo(object parameter)
        {
            if (parameter is IStorage storage)
            {
                Messenger.Send(new StopSearchMessage());
                NavigateStorage(storage);
            }
        }

        /// <summary>
        /// Navigates storage item and sends message for tab to changed tab's storage item
        /// </summary>
        /// <param name="storage"> Storage that is navigated </param>
        protected void NavigateStorage(IStorage storage)
        {
            Storage = storage;
            Messenger.Send(new TabStorageChangedMessage(storage));
        }

        /// <summary>
        /// By default, when storage view model is navigated from it unregisters from all messages
        /// </summary>
        public virtual void OnNavigatedFrom()
        {
            Messenger.UnregisterAll(this);
        }

        /// <summary> 
        /// Sends message for a <see cref="SearchOperationViewModel"/> to start new search. Search will be started in <see cref="Storage"/>
        /// </summary>
        public virtual void HandleSearchMessage(ObservableRecipient recipient, SearchOperationRequiredMessage message)
        {
            Messenger.Send(new SearchStorageMessage(Storage, message.Options));
        }
    }
}
