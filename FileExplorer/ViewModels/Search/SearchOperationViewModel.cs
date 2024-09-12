#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Models.Contracts.Storage;
using FileExplorer.Models.General;
using FileExplorer.Models.Messages;
using FileExplorer.Models.Storage.Additional;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels.Search
{
    /// <summary>
    /// View model that executes search operations
    /// </summary>
    public sealed class SearchOperationViewModel : ObservableRecipient
    {
        /// <summary>
        /// Search options for current search operation
        /// </summary>
        private SearchOptions currentSearchOptions;

        /// <summary>
        /// Cached search result, which contains all information about search (found items, root Directory etc.)
        /// </summary>
        public CachedSearchResult CachedSearch { get; private set; }

        /// <summary>
        /// Cancellation token to cancel search operation
        /// </summary>
        private CancellationTokenSource searchCancellation;

        public SearchOperationViewModel()
        {
            Messenger.Register<SearchOperationViewModel, StopSearchMessage>(this, (_, _) =>
            {
                CancelSearchIfNeeded();
            });
        }

        /// <summary>
        /// Sets up information for an incoming search 
        /// </summary>
        /// <param name="searchCatalog"> Where we are searching items </param>
        /// <param name="searchOptions"> Data needed to execute search </param>
        public void InitializeSearchData(IStorage searchCatalog, SearchOptions searchOptions)
        {
            currentSearchOptions = searchOptions;
            CachedSearch = new(searchCatalog, searchOptions.Destination);
        }

        /// <summary>
        /// Searches provided in <see cref="InitializeSearchData"/> method catalog with selected options
        /// </summary>
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

        /// <summary>
        /// Cancels search if it was not already cancelled 
        /// </summary>
        public void CancelSearchIfNeeded()
        {
            if (searchCancellation is not null)
            {
                searchCancellation.Cancel();
            }
        }
    }
}
