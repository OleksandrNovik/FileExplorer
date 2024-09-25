using CommunityToolkit.Mvvm.ComponentModel;
using FileExplorer.Core.Contracts.Factories.SearchProperties;
using FileExplorer.Core.Services.Factories.SearchProperties;
using FileExplorer.Models.Storage.Additional;
using FileExplorer.Services.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System;

namespace FileExplorer.Helpers
{
    public static class ServiceConfigurationExtensions
    {
        /// <summary>
        /// Configures transient service for a page and view model
        /// </summary>
        /// <typeparam name="TPage"> Page we are adding as service </typeparam>
        /// <typeparam name="TViewModel"> View model for the page </typeparam>
        /// <param name="services"> Service collection to add new services </param>
        public static void AddPageAndViewModel<TPage, TViewModel>(this IServiceCollection services)
            where TPage : Page
            where TViewModel : ObservableRecipient
        {
            services.AddTransient<TPage>();
            services.AddTransient<TViewModel>();
        }

        public static void AddSearchMenuBuilders(this IServiceCollection services)
        {
            // Adding service that uses other factories to build search menu items 
            // It provides functionality depending on what property is built
            services.AddTransient<ISearchOptionsMenuBuilder, SearchMenuBuilder>();

            // Adding factories to build menu for a certain property
            services.AddTransient<ISearchPropertyMenuFactory<ByteSize>, SizeMenuBuilder>();
            services.AddTransient<ISearchPropertyMenuFactory<DateTime>, DateMenuBuilder>();
        }
    }
}
