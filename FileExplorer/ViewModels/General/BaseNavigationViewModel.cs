#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;

namespace FileExplorer.ViewModels.General
{
    /// <summary>
    /// Base code for a view model that needs to perform navigation
    /// </summary>
    public abstract partial class BaseNavigationViewModel : ObservableRecipient
    {
        /// <summary>
        /// Selected navigation item
        /// </summary>
        [ObservableProperty]
        protected object? selected;
    }
}
