#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.StorageWrappers;

namespace Models.TabRelated
{
    /// <summary>
    /// Models to represent tab information needed to manipulate tabs
    /// </summary>
    public sealed partial class TabModel : ObservableObject
    {
        /// <summary>
        /// Selected menu option in this tab
        /// </summary>
        [ObservableProperty]
        private object? selected;

        /// <summary>
        /// Directory that is opened in the tab
        /// </summary>
        [ObservableProperty]
        private DirectoryWrapper tabDirectory;

        /// <summary>
        /// Navigation history of the tab
        /// </summary>
        public TabNavigationHistoryModel TabHistory { get; }

        public TabModel(DirectoryWrapper directory)
        {
            tabDirectory = directory;
            TabHistory = new TabNavigationHistoryModel();
        }
    }
}
