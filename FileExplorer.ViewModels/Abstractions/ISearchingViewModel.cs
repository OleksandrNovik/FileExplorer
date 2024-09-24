using CommunityToolkit.Mvvm.ComponentModel;
using FileExplorer.Models.Messages;

namespace FileExplorer.ViewModels.Abstractions
{
    /// <summary>
    /// Interface that requires view models to handle search message
    /// </summary>
    public interface ISearchingViewModel
    {
        /// <summary>
        /// Search message handler
        /// </summary>
        protected void HandleSearchMessage(ObservableRecipient recipient, SearchOperationRequiredMessage message);
    }
}
