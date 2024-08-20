#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Contracts.Storage;
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

        [ObservableProperty]
        private string title;

        private IStorage<DirectoryItemWrapper> openedStorage;

        /// <summary>
        /// Directory (or something that stores items) that is opened in the tab
        /// </summary>
        public IStorage<DirectoryItemWrapper> OpenedStorage
        {
            get => openedStorage;
            set
            {
                if (openedStorage != value)
                {
                    openedStorage = value;
                    Title = openedStorage.Name;
                }
            }
        }

        /// <summary>
        /// Navigation history of the tab
        /// </summary>
        public TabNavigationHistoryModel TabHistory { get; }

        public TabModel(IStorage<DirectoryItemWrapper> opened)
        {
            OpenedStorage = opened;
        }

    }
}
