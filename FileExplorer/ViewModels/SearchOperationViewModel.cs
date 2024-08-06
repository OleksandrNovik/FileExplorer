#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Models.Contracts;
using Models.General;
using Models.Messages;
using Models.StorageWrappers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels
{
    public sealed partial class SearchOperationViewModel : ObservableRecipient
    {
        private ISystemSearchCatalog<DirectoryItemWrapper> searchCatalog;

        private IEnqueuingCollection<DirectoryItemWrapper> destination;

        private CachedSearchResult<DirectoryItemWrapper>? cachedSearch;

        private CancellationTokenSource searchCancellation;

        [ObservableProperty]
        private bool isSearching;

        public void InitializeSearchData(ISystemSearchCatalog<DirectoryItemWrapper> searchCatalog, IEnqueuingCollection<DirectoryItemWrapper> destination)
        {
            this.searchCatalog = searchCatalog;
            this.destination = destination;
        }

        public SearchOperationViewModel()
        {
            Messenger.Register<SearchOperationViewModel, SearchOperationRequiredMessage>(this, OnSearchMessage);

            Messenger.Register<SearchOperationViewModel, ContinueSearchMessage>(this, ContinueSearchAsync);

            Messenger.Register<SearchOperationViewModel, StopSearchMessage>(this, (_, _) =>
            {
                CancelSearchIfNeeded();
            });
        }

        private async void ContinueSearchAsync(SearchOperationViewModel recipient, ContinueSearchMessage message)
        {
            await SearchAsync();
        }

        private async void OnSearchMessage(SearchOperationViewModel _, SearchOperationRequiredMessage message)
        {
            Debug.WriteLine("Search started");

            cachedSearch = new CachedSearchResult<DirectoryItemWrapper>(searchCatalog, destination, message.Options)
            {
                RootCatalog = new CachedCatalogSearch<DirectoryItemWrapper>(searchCatalog)
            };

            Messenger.Send(new SearchStartedMessage<DirectoryItemWrapper>(cachedSearch));

            await SearchAsync();
        }

        private async Task SearchAsync()
        {
            CancelSearchIfNeeded();
            Debug.Assert(cachedSearch is not null);

            //If this search has already completed just get the result collection form it by navigating to the search result
            if (cachedSearch.HasCompleted)
            {
                Messenger.Send(new NavigateToSearchResult<DirectoryItemWrapper>(cachedSearch));
                return;
            }

            try
            {
                Messenger.Send(new DirectoryNavigationInfo(cachedSearch));

                destination.Clear();

                cachedSearch.InProgress = true;

                await cachedSearch.RootCatalog.SearchAsync(destination, cachedSearch.SearchOptions,
                    searchCancellation.Token);

                //TODO: Raise search completed message
                cachedSearch.HasCompleted = true;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Search CANCELLED");
            }
            finally
            {
                cachedSearch.InProgress = false;
                // When search is over or interrupted we can save found items into another collection
                cachedSearch.SearchResultItems = destination.ToArray();
            }
        }
        public void CancelSearchIfNeeded()
        {
            if (searchCancellation is not null)
            {
                searchCancellation.Cancel();
            }
            ;
            searchCancellation = new CancellationTokenSource();
        }

        public void UnregisterAll()
        {
            Messenger.UnregisterAll(this);
        }
    }
}
