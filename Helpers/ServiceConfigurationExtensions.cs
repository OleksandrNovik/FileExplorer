using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace Helpers
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

    }
}
