#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using Models.Contracts.Storage;
using Models.General;
using Models.Messages;
using Models.Storage.Additional;
using Models.Storage.Windows;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels.Search
{
    public sealed class SearchOperationViewModel : ObservableRecipient
    {
        private SearchOptions currentSearchOptions;
        public CachedSearchResult<DirectoryItemWrapper> CachedSearch { get; private set; }
        private CancellationTokenSource searchCancellation;

        public void InitializeSearchData(IStorage<DirectoryItemWrapper> searchCatalog, SearchOptions searchOptions)
        {
            currentSearchOptions = searchOptions;
            CachedSearch = new(searchCatalog, searchOptions.Destination);
            Messenger.Send(new StorageNavigatedMessage(CachedSearch));
        }
        public SearchOperationViewModel()
        {
            Messenger.Register<SearchOperationViewModel, StopSearchMessage>(this, (_, _) =>
            {
                CancelSearchIfNeeded();
            });
        }

        public async Task SearchAsync()
        {
            CancelSearchIfNeeded();

            // Initializing cancellation token 
            searchCancellation = new CancellationTokenSource();
            currentSearchOptions.Token = searchCancellation.Token;

            // Creating cached search result and setting it as currently viewed storage 
            Messenger.Send(new StorageNavigatedMessage(CachedSearch));

            try
            {
                CachedSearch.Filter = currentSearchOptions.Filter;

                await CachedSearch.SearchAsync(currentSearchOptions);

                Messenger.Send(new ShowInfoBarMessage(InfoBarSeverity.Success, "Search is completed!"));

                CachedSearch.HasCompleted = true;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Search CANCELLED");
            }
            finally
            {
                Messenger.Send(new StopSearchMessage());
            }

        }
        public void CancelSearchIfNeeded()
        {
            if (searchCancellation is not null)
            {
                searchCancellation.Cancel();
            }
        }
    }
}
