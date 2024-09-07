using CommunityToolkit.Mvvm.ComponentModel;
using Models.Contracts.Storage.Properties;

namespace Models.Storage.Abstractions
{
    /// <summary>
    /// Abstract class that contains basic properties of storage item
    /// </summary>
    public abstract partial class BasicStorageItemProperties : ObservableObject, IBasicStorageItemProperties
    {
        /// <summary>
        /// Name of item
        /// </summary>
        [ObservableProperty]
        private string name;

        /// <summary>
        /// Path to the item
        /// </summary>
        public string Path { get; protected set; }
    }
}
