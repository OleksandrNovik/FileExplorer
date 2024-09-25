using CommunityToolkit.Mvvm.Input;
using FileExplorer.Models;
using FileExplorer.Models.Enums;
using FileExplorer.Models.Ranges;
using FileExplorer.Models.Storage.Additional;
using System.Collections.Generic;

namespace FileExplorer.Core.Services.Factories.SearchProperties
{
    /// <summary>
    /// Factory for <see cref="ByteSize" /> search property
    /// </summary>
    public sealed class SizeMenuBuilder : BasePropertyBuilder<ByteSize>
    {
        /// <inheritdoc />
        protected override IEnumerable<MenuFlyoutItemViewModel> CompleteMenu(IRelayCommand command)
        {
            return
            [

                new MenuFlyoutItemViewModel("Empty ",
                    new RangeChecker<ByteSize>(ByteSizeRange.Empty, ExcludingOptions.Less))
                {
                    Command = command,
                },
                new MenuFlyoutItemViewModel("Tiny ",
                    new RangeChecker<ByteSize>(ByteSizeRange.Tiny, ExcludingOptions.Within))
                {
                    Command = command,
                },

                new MenuFlyoutItemViewModel("Tiny ",
                    new RangeChecker<ByteSize>(ByteSizeRange.Small, ExcludingOptions.Within))
                {
                    Command = command,
                },

                new MenuFlyoutItemViewModel("Medium ",
                    new RangeChecker<ByteSize>(ByteSizeRange.Medium, ExcludingOptions.Within))
                {
                    Command = command,
                },

                new MenuFlyoutItemViewModel("Large ",
                    new RangeChecker<ByteSize>(ByteSizeRange.Large, ExcludingOptions.Within))
                {
                    Command = command,
                },

                new MenuFlyoutItemViewModel("Huge ",
                    new RangeChecker<ByteSize>(ByteSizeRange.Huge, ExcludingOptions.Within))
                {
                    Command = command,
                },
                new MenuFlyoutItemViewModel("Giant ",
                    new RangeChecker<ByteSize>(ByteSizeRange.Giant, ExcludingOptions.More))
                {
                    Command = command,
                },

            ];
        }
    }
}
