#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Models.Contracts.Storage;

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

        private IStorage openedStorage;

        /// <summary>
        /// Storage (or something that stores items) that is opened in the tab
        /// </summary>
        public IStorage OpenedStorage
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

        public TabModel(IStorage opened)
        {
            TabHistory = new TabNavigationHistoryModel();
            OpenedStorage = opened;
        }

    }
}
