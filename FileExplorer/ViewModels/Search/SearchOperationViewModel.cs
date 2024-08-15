#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using Models.Contracts;
using Models.General;
using Models.Messages;
using Models.StorageWrappers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels.Search
{
    public sealed class SearchOperationViewModel : ObservableRecipient
    {
        private ISystemSearchCatalog<DirectoryItemWrapper> searchCatalog;

        private IEnqueuingCollection<DirectoryItemWrapper> destination;

        private CachedSearchResult<DirectoryItemWrapper>? cachedSearch;

        private CancellationTokenSource searchCancellation;

        public void InitializeSearchData(ISystemSearchCatalog<DirectoryItemWrapper> searchCatalog, IEnqueuingCollection<DirectoryItemWrapper> destination, CachedSearchResult<DirectoryItemWrapper>? cachedSearch = null)
        {
            this.searchCatalog = searchCatalog;
            this.destination = destination;
            this.cachedSearch = cachedSearch;
        }

        public SearchOperationViewModel()
        {
            Messenger.Register<SearchOperationViewModel, SearchOperationRequiredMessage>(this, OnSearchMessage);

            Messenger.Register<SearchOperationViewModel, StopSearchMessage>(this, (_, _) =>
            {
                CancelSearchIfNeeded();
            });
        }

        private async void OnSearchMessage(SearchOperationViewModel _, SearchOperationRequiredMessage message)
        {
            Debug.WriteLine("Search started");

            cachedSearch = new CachedSearchResult<DirectoryItemWrapper>(searchCatalog, destination, message.Options);

            Messenger.Send(new SearchStartedMessage<DirectoryItemWrapper>(cachedSearch));

            await SearchAsync();
        }

        private async Task SearchAsync()
        {
            CancelSearchIfNeeded();
            searchCancellation = new CancellationTokenSource();
            Debug.Assert(cachedSearch is not null);

            //If this search has already completed just get the result collection form it by navigating to the search result
            if (cachedSearch.HasCompleted)
            {
                Messenger.Send(new NavigateToSearchResult<DirectoryItemWrapper>(cachedSearch));
                return;
            }

            try
            {
                destination.Clear();

                await cachedSearch.RootCatalog.SearchAsync(destination, cachedSearch.SearchOptions,
                    searchCancellation.Token);

                Messenger.Send(new ShowInfoBarMessage(InfoBarSeverity.Success, "Search is completed!"));
                cachedSearch.HasCompleted = true;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Search CANCELLED");
            }
            finally
            {
                // When search is over or interrupted we can save found items into another collection
                cachedSearch.SearchResultItems = destination.ToArray();
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

        public void UnregisterAll()
        {
            Messenger.UnregisterAll(this);
        }
    }
}
