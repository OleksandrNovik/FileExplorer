using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FileExplorer.Core.Contracts;
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

        private readonly IMessageDialogService dialogService;

        /// <summary>
        /// Validator for names 
        /// </summary>
        public INameValidator Validator { get; }

        /// <summary>
        /// Decides if system needs to show file's extension
        /// </summary>
        [ObservableProperty]
        private bool showExtensions;

        public StorageItemsNamingViewModel(ILocalSettingsService settingsService, INameValidator validator, IMessageDialogService messageDialogService)
        {
            localSettings = settingsService;
            dialogService = messageDialogService;
            Validator = validator;
            showExtensions = localSettings.ReadBool(LocalSettings.Keys.ShowFileExtensions) ?? false;

            // Subscription that notifies view model if it needs to show extensions or not 
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

        /// <summary>
        /// Checks item's name before ending renaming process
        /// If name is valid renames item on UI and physically
        /// When item's name is invalid shows message for user and cancels renaming
        /// </summary>
        /// <param name="item"> Items that is renamed </param>
        [RelayCommand]
        private async Task EndRenamingItemAsync(IRenameableObject item)
        {
            if (Validator.IsInvalid(item.Name))
            {
                await dialogService.ShowMessageAsync(
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
