#nullable enable
using Microsoft.UI.Xaml.Controls;


namespace FileExplorer.Core.Contracts.General
{
    /// <summary>
    /// Base interface for a navigation services, that contains all needed components to execute navigation
    /// </summary>
    /// <typeparam name="TParam"> Navigation parameter type </typeparam>
    public interface IBasicNavigationService<in TParam>
    {
        /// <summary>
        /// Frame element to perform navigation of pages
        /// </summary>
        public Frame? Frame { get; set; }

        /// <summary>
        /// Navigates to a parameter if it is provided. If not - navigates to a default page
        /// </summary>
        /// <param name="value"> Navigation parameter </param>
        /// <param name="parameter"> Parameter for navigation operation  </param>
        public void NavigateTo(TParam? value = default, object? parameter = null);
    }
}
