using FileExplorer.Models;
using FileExplorer.Models.StorageWrappers;
using FileExplorer.ViewModels;
using System.Collections.Generic;

namespace FileExplorer.Services
{
    public class ContextMenuMetadataBuilder
    {
        private readonly DirectoryPageViewModel viewModel;

        public ContextMenuMetadataBuilder(DirectoryPageViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public List<MenuFlyoutItemViewModel> BuildMenuForItem(DirectoryItemWrapper item)
        {
            var folder = item as DirectoryWrapper;

            List<MenuFlyoutItemViewModel> menuCommands = [
                new MenuFlyoutItemViewModel("Open")
                {
                    Command = viewModel.OpenCommand,
                    CommandParameter = item
                },

                new MenuFlyoutItemViewModel("Copy")
                {
                    Command = viewModel.CopyItemCommand,
                    CommandParameter = item
                },

                new MenuFlyoutItemViewModel("Cut")
                {
                    Command = viewModel.CutItemCommand,
                    CommandParameter = item
                },

                new MenuFlyoutItemViewModel("Delete")
                {
                    Command = viewModel.RecycleItemCommand,
                    CommandParameter = item
                },

                new MenuFlyoutItemViewModel("Rename")
                {
                    Command = viewModel.BeginRenamingItemCommand,
                    CommandParameter = item
                },

                new MenuFlyoutItemViewModel("Details")
                {
                    Command = viewModel.ShowDetailsCommand,
                    CommandParameter = item
                }
            ];

            if (folder is not null)
            {
                menuCommands.Insert(1, new MenuFlyoutItemViewModel("Open in new tab")
                {
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
                    Command = viewModel.RefreshCommand
                },

                new MenuFlyoutItemViewModel("Create")
                {
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
                    Command = viewModel.PasteItemsCommand
                },

                new MenuFlyoutItemViewModel("Details")
                {
                    Command = viewModel.ShowDetailsCommand,
                    CommandParameter = viewModel.CurrentDirectory
                }
            ];
        }
    }
}
