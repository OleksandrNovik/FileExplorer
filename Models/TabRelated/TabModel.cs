using CommunityToolkit.Mvvm.ComponentModel;
using Models.StorageWrappers;
using System;

namespace Models.TabRelated
{
    /// <summary>
    /// Models to represent tab information needed to manipulate tabs
    /// </summary>
    public sealed partial class TabModel : ObservableObject
    {
        /// <summary>
        /// Type of page that is opened in the tab 
        /// </summary>
        public Type TabType { get; }

        /// <summary>
        /// Directory that is opened in the tab
        /// </summary>
        [ObservableProperty]
        private DirectoryWrapper tabDirectory;

        /// <summary>
        /// Navigation history of the tab
        /// </summary>
        public TabNavigationHistoryModel TabHistory { get; }

        public TabModel(DirectoryWrapper directory, Type tabType)
        {
            TabType = tabType;
            TabDirectory = directory;
            TabHistory = new TabNavigationHistoryModel();
        }
    }
}
