#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Dispatching;
using Models;
using Models.Contracts;
using Models.Messages;
using Models.Ranges;
using Models.StorageWrappers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PathHelper = Helpers.StorageHelpers.PathHelper;

namespace FileExplorer.ViewModels
{
    public sealed partial class SearchOperationsViewModel : ObservableRecipient
    {
        public SearchOperationsViewModel()
        {
            Messenger.Register<SearchOperationsViewModel, SearchDirectoryMessage>(this, OnSearchSourceReceived);
        }

        private async void OnSearchSourceReceived(SearchOperationsViewModel _, SearchDirectoryMessage message)
        {
            //await SearchItemsAsync(message.SearchedCatalog);
            //await SearchAsync(message.SearchedCatalog);
        }

        private CancellationTokenSource? cancellation;
        public IEnumerable<MenuFlyoutItemViewModel> DateSearchOptions => new List<MenuFlyoutItemViewModel>
        {
            new MenuFlyoutItemViewModel("Any")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.Any
            },
            new MenuFlyoutItemViewModel("Today")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.TodayRange
            },
            new MenuFlyoutItemViewModel("Yesterday")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.YesterdayRange
            },
            new MenuFlyoutItemViewModel("This week")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.ThisWeekRange
            },
            new MenuFlyoutItemViewModel("Last week")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.LastWeekRange
            },
            new MenuFlyoutItemViewModel("This month")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.ThisMonthRange
            },
            new MenuFlyoutItemViewModel("Last month")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.LastMonthRange,
            },
            new MenuFlyoutItemViewModel("This year")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.ThisYearRange,
            },
            new MenuFlyoutItemViewModel("Last year")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.LastYearRange,
            },
        };
        public IEnumerable<MenuFlyoutItemViewModel> SizeSearchOptions => new List<MenuFlyoutItemViewModel>
        {
            new MenuFlyoutItemViewModel("Last year")
            {
                Command = SetDateOptionCommand,
                CommandParameter = DateRange.LastYearRange,
            },

        };

        [ObservableProperty]
        private string searchQuery;
        public SearchOptionsModel Options => SearchOptionsModel.Default;

        [RelayCommand]
        private void SetDateOption(DateRange range)
        {
            Options.AccessDateRange = range;
        }

        [RelayCommand]
        private void SetTypeOption(Predicate<string> extensionFilter)
        {
            Options.ExtensionFilter = extensionFilter;
        }

        [RelayCommand]
        private void InitiateSearch()
        {
            if (cancellation is not null)
            {
                cancellation.Cancel();
                cancellation.Dispose();
            }

            cancellation = new CancellationTokenSource();

            Options.SearchPattern = PathHelper.CreatePattern(SearchQuery);

            // If we cannot generate more precise pattern we should search by name
            if (Options.SearchPattern == "*")
            {
                Options.SearchName = SearchQuery;
            }

            Messenger.Send(new SearchOperationRequiredMessage(cancellation, Options));
        }

        private async Task SearchItemsAsync(ISearchable<DirectoryItemWrapper> searchCatalog)
        {
            var dispatcher = DispatcherQueue.GetForCurrentThread();

            Debug.Assert(cancellation is not null);
            var token = cancellation.Token;

            await Task.Run(() =>
            {
                Options.SearchPattern = PathHelper.CreatePattern(SearchQuery);

                // If we cannot generate more precise pattern we should search by name
                if (Options.SearchPattern == "*")
                {
                    Options.SearchName = SearchQuery;
                }

                var found = searchCatalog.SearchParallel(Options).GetEnumerator();
                int operatedItemsCount = 50;

                while (true)
                {
                    var bunch = new List<DirectoryItemWrapper>();

                    for (int i = 0; i < operatedItemsCount && found.MoveNext(); i++)
                    {
                        bunch.Add(found.Current);
                    }

                    if (token.IsCancellationRequested)
                        break;

                    dispatcher.TryEnqueue(() =>
                    {
                        //Messenger.Send(new SearchIterationMessage(bunch, bunch.Count < operatedItemsCount));
                    });

                    if (bunch.Count < operatedItemsCount)
                        break;
                }

                Debug.WriteLine("------------------ Task DONE -------------------");

            }, token);
        }

        //private async Task SearchAsync(ISearchable<DirectoryItemWrapper> searchCatalog)
        //{
        //    var watch = new Stopwatch();

        //    watch.Start();
        //    var dispatcher = DispatcherQueue.GetForCurrentThread();

        //    Debug.Assert(cancellation is not null);
        //    var token = cancellation.Token;

        //    await Task.Run(async () =>
        //    {
        //        while (true)
        //        {
        //            var searchIteration = await searchCatalog.ContinueSearchAsync();

        //            if (searchIteration is null)
        //                break;

        //            dispatcher.TryEnqueue(() =>
        //            {
        //                Messenger.Send(new SearchIterationMessage(searchIteration));
        //            });
        //        }
        //    }, token);
        //    watch.Stop();
        //    Debug.WriteLine("------------------- Elapsed: {0} -------------------", watch.ElapsedMilliseconds);
        //}
    }
}
