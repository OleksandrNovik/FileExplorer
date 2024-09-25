using CommunityToolkit.Mvvm.Input;
using FileExplorer.Helpers.StorageHelpers;
using FileExplorer.Models;
using FileExplorer.Models.Ranges;
using System.Collections.Generic;

namespace FileExplorer.Core.Services.Factories.SearchProperties
{
    /// <summary>
    /// Factory for file type property of search options
    /// </summary>`
    public sealed class FileTypeMenuBuilder : BasePropertyBuilder<string>
    {
        protected override IEnumerable<MenuFlyoutItemViewModel> CompleteMenu(IRelayCommand command)
        {
            // TODO: Complete types list
            return
            [
                new MenuFlyoutItemViewModel("Images")
                {
                    Command = command,
                    CommandParameter = new PredicateChecker<string>(FileExtensionsHelper.IsImage)
                },
                new MenuFlyoutItemViewModel("Media")
                {
                    Command = command,
                    CommandParameter = new PredicateChecker<string>(FileExtensionsHelper.IsMedia)
                },
                new MenuFlyoutItemViewModel("Video")
                {
                    Command = command,
                    CommandParameter = new PredicateChecker<string>(FileExtensionsHelper.IsVideo)
                }
            ];
        }
    }
}
