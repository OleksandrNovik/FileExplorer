using FileExplorer.Models.StorageWrappers;
using FileExplorer.ViewModels;
using Models;
using Models.StorageWrappers;
using System.Collections.Generic;

namespace FileExplorer.Services
{
    public sealed class ContextMenuMetadataBuilder
    {
        private readonly DirectoryPageViewModel viewModel;

        public ContextMenuMetadataBuilder(DirectoryPageViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        /// <summary>
        /// Builds menu for a certain item that provided as parameter
        /// </summary>
        /// <param name="item"> Item that will be used in interactions with menu </param>
        /// <returns> Menu options for a provided item </returns>
        public List<MenuFlyoutItemViewModel> BuildMenuForItem(DirectoryItemWrapper item)
        {
            var folder = item as DirectoryWrapper;

            List<MenuFlyoutItemViewModel> menuCommands = [
                new MenuFlyoutItemViewModel("Open")
                {
                    IconGlyph = "\uED25",
                    Command = viewModel.OpenCommand,
                    CommandParameter = item
                },

                new MenuFlyoutItemViewModel("Copy")
                {
                    IconGlyph = "\uE8C8",
                    Command = viewModel.CopyItemCommand,
                    CommandParameter = item
                },

                new MenuFlyoutItemViewModel("Cut")
                {
                    IconGlyph = "\uE8C6",
                    Command = viewModel.CutItemCommand,
                    CommandParameter = item
                },

                new MenuFlyoutItemViewModel("Delete")
                {
                    IconGlyph = "\uE74D",
                    Command = viewModel.RecycleItemCommand,
                    CommandParameter = item
                },

                new MenuFlyoutItemViewModel("Rename")
                {
                    IconGlyph = "\uE8AC",
                    Command = viewModel.BeginRenamingItemCommand,
                    CommandParameter = item
                },

                new MenuFlyoutItemViewModel("Details")
                {
                    IconGlyph = "\uE946",
                    Command = viewModel.ShowDetailsCommand,
                    CommandParameter = item
                }
            ];

            if (folder is not null)
            {
                menuCommands.Insert(1, new MenuFlyoutItemViewModel("Open in new tab")
                {
                    IconGlyph = "\uE8B4",
                    Command = viewModel.OpenInNewTabCommand,
                    CommandParameter = folder
                });
            }

            return menuCommands;
        }

        /// <summary>
        /// Builds default menu (when no items are selected)
        /// This menu manipulates current directory itself (sorting, changing view, refreshing etc.)
        /// </summary>
        /// <returns> Default menu for current directory </returns>
        public List<MenuFlyoutItemViewModel> BuildDefaultMenu()
        {
            return
            [
                new MenuFlyoutItemViewModel("Refresh")
                {
                    IconGlyph = "\uE72C",
                    Command = viewModel.RefreshCommand
                },

                new MenuFlyoutItemViewModel("Create")
                {
                    IconGlyph = "\uE710",
                    Items = new List<MenuFlyoutItemViewModel>
                    {
                        new MenuFlyoutItemViewModel("File")
                        {
                            Command = viewModel.CreateFileCommand
                        },
                        new MenuFlyoutItemViewModel("Folder")
                        {
                            Command = viewModel.CreateDirectoryCommand
                        },
                    }
                },

                new MenuFlyoutItemViewModel("Paste")
                {
                    IconGlyph = "\uE77F",
                    Command = viewModel.PasteItemsCommand
                },

                new MenuFlyoutItemViewModel("Details")
                {
                    IconGlyph = "\uE946",
                    Command = viewModel.ShowDetailsCommand,
                    CommandParameter = viewModel.CurrentDirectory
                }
            ];
        }
    }
}
