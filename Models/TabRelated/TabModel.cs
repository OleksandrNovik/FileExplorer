#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Storage.Windows;

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
        /// Name of the tab that depends on current tab's directory
        /// </summary>
        [ObservableProperty]
        private string tabName;

        private DirectoryWrapper? tabDirectory;

        /// <summary>
        /// Full prop to change tab name when directory is changed
        /// </summary>
        public DirectoryWrapper? TabDirectory
        {
            get => tabDirectory;
            set
            {
                if (tabDirectory != value)
                {
                    tabDirectory = value;
                    TabName = tabDirectory?.Name ?? "Home";
                }
            }
        }

        /// <summary>
        /// Navigation history of the tab
        /// </summary>
        public TabNavigationHistoryModel TabHistory { get; }

        public TabModel(DirectoryWrapper? directory)
        {
            tabDirectory = directory;
            tabName = tabDirectory?.Name ?? "Home";
            TabHistory = new TabNavigationHistoryModel();
        }
    }
}
