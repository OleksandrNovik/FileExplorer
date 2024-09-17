using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.Core.Contracts.Storage;
using FileExplorer.Helpers.Application;
using FileExplorer.Models.Contracts.Storage;
using FileExplorer.Models.Messages;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels.General
{
    public sealed partial class StorageItemsNamingViewModel : ObservableRecipient
    {
        /// <summary>
        /// Service to access local settings
        /// </summary>
        private readonly ILocalSettingsService localSettings;

        /// <summary>
        /// Validator for names 
        /// </summary>
        public INameValidator Validator { get; }

        [ObservableProperty]
        private bool showExtensions;

        public StorageItemsNamingViewModel(ILocalSettingsService settingsService, INameValidator validator)
        {
            localSettings = settingsService;

            Validator = validator;
            showExtensions = localSettings.ReadBool(LocalSettings.Keys.ShowFileExtensions) ?? false;

            Messenger.Register<StorageItemsNamingViewModel, ShowExtensionsChangedMessage>(this, (_, message) =>
            {
                ShowExtensions = message.Value;
            });
        }

        /// <summary>
        /// Begins renaming provided object
        /// </summary>
        /// <param name="item"> Object that is renamed </param>
        [RelayCommand]
        private void BeginRenamingItem(IRenameableObject item) => item.BeginEdit();

        [RelayCommand]
        private async Task EndRenamingItemAsync(IRenameableObject item)
        {
            if (Validator.IsInvalid(item.Name))
            {
                await App.MainWindow.ShowMessageDialogAsync(
                    $"Name contains illegal characters: {Validator.IlleagalCharacters}. Or this name is special name that is reserved.",
                    $"Name \"{item.Name}\" is invalid.");

                item.CancelEdit();
            }
            else
            {
                item.Rename();
                item.EndEdit();
            }
        }
    }
}
