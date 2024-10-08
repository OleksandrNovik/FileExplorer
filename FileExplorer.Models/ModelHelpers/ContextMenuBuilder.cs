﻿#nullable enable
using CommunityToolkit.Mvvm.Input;
using FileExplorer.Helpers;
using System.Collections.Generic;

namespace FileExplorer.Models.ModelHelpers
{
    public static class ContextMenuBuilder
    {
        private class CommandData
        {
            public IRelayCommand Command { get; }
            public object? CommandParameter { get; }
            public string Name { get; }
            public string IconGlyph { get; }

            public CommandData(string name, string icon, IRelayCommand command, object? commandParameter)
            {
                Command = command;
                Name = name;
                IconGlyph = icon;
                CommandParameter = commandParameter;
            }

        }

        /// <summary>
        /// Adds open menu item to an existing menu
        /// </summary>
        /// <param name="menu"> Provided menu </param>
        /// <param name="command"> Command to open item </param>
        /// <param name="commandParameter"> Parameter to te command </param>
        public static List<MenuFlyoutItemViewModel> WithOpen(this List<MenuFlyoutItemViewModel> menu,
            IRelayCommand command, object? commandParameter = null)
        {
            return menu.FromCommandData(new CommandData("Open", Constants.FluentIcons.Open, command, commandParameter));
        }

        /// <summary>
        /// Adds open in new tab menu item to an existing menu
        /// </summary>
        /// <param name="menu"> Provided menu </param>
        /// <param name="command"> Command to open item in a new tab </param>
        /// <param name="commandParameter"> Parameter to te command </param>
        public static List<MenuFlyoutItemViewModel> WithOpenInNewTab(this List<MenuFlyoutItemViewModel> menu,
            IRelayCommand command, object? commandParameter = null)
        {
            return menu.FromCommandData(new CommandData("Open in new tab", Constants.FluentIcons.OpenInNewTab, command, commandParameter));
        }

        /// <summary>
        /// Adds create (file or folder) to an existing menu
        /// </summary>
        /// <param name="menu"> Provided menu </param>
        /// <param name="command"> Command to create item (command should have bool parameter for: true - create file, false - create folder) </param>
        public static List<MenuFlyoutItemViewModel> WithCreate(this List<MenuFlyoutItemViewModel> menu,
            IRelayCommand command)
        {
            var menuItem = new MenuFlyoutItemViewModel("Create")
            {
                IconGlyph = Constants.FluentIcons.Create,
                Items =
                [
                    new MenuFlyoutItemViewModel("File").WithCommand(command, false),
                    new MenuFlyoutItemViewModel("Folder").WithCommand(command, true)
                ]
            };

            menu.Add(menuItem);
            return menu;
        }


        /// <summary>
        /// Adds refresh menu item to an existing menu
        /// </summary>
        /// <param name="menu"> Provided menu </param>
        /// <param name="command"> Command to refresh directory </param>
        /// <param name="commandParameter"> Parameter to te command </param>
        public static List<MenuFlyoutItemViewModel> WithRefresh(this List<MenuFlyoutItemViewModel> menu,
            IRelayCommand command, object? commandParameter = null)
        {
            return menu.FromCommandData(new CommandData("Refresh", Constants.FluentIcons.Refresh, command, commandParameter));
        }

        /// <summary>
        /// Adds paste menu item to an existing menu
        /// </summary>
        /// <param name="menu"> Provided menu </param>
        /// <param name="command"> Command to paste items to a directory </param>
        /// <param name="commandParameter"> Parameter to te command </param>
        public static List<MenuFlyoutItemViewModel> WithPaste(this List<MenuFlyoutItemViewModel> menu,
            IRelayCommand command, object? commandParameter = null)
        {
            return menu.FromCommandData(new CommandData("Paste", Constants.FluentIcons.Paste, command, commandParameter));
        }

        /// <summary>
        /// Adds Details menu item to an existing menu
        /// </summary>
        /// <param name="menu"> Provided menu </param>
        /// <param name="command"> Command to show Details </param>
        /// <param name="commandParameter"> Parameter to te command </param>
        public static List<MenuFlyoutItemViewModel> WithDetails(this List<MenuFlyoutItemViewModel> menu,
            IRelayCommand command, object? commandParameter = null)
        {
            return menu.FromCommandData(new CommandData("Details", Constants.FluentIcons.Details, command, commandParameter));
        }

        /// <summary>
        /// Adds cut & rename menu item to an existing menu
        /// </summary>
        /// <param name="menu"> Provided menu </param>
        /// <param name="command"> Command to rename item </param>
        /// <param name="commandParameter"> Parameter to te command </param>
        public static List<MenuFlyoutItemViewModel> WithRename(this List<MenuFlyoutItemViewModel> menu,
            IRelayCommand command, object? commandParameter = null)
        {
            return menu.FromCommandData(new CommandData("Rename", Constants.FluentIcons.Rename, command, commandParameter));
        }


        /// <summary>
        /// Adds cut & rename menu item to an existing menu
        /// </summary>
        /// <param name="menu"> Provided menu </param>
        /// <param name="command"> Command to cut item or items </param>
        /// <param name="commandParameter"> Parameter to te command </param>
        public static List<MenuFlyoutItemViewModel> WithCut(this List<MenuFlyoutItemViewModel> menu,
            IRelayCommand command, object? commandParameter = null)
        {
            return menu.FromCommandData(new CommandData("Cut", Constants.FluentIcons.Cut, command, commandParameter));
        }

        /// <summary>
        /// Adds copy menu item to an existing menu
        /// </summary>
        /// <param name="menu"> Provided menu </param>
        /// <param name="command"> Command to copy item or items </param>
        /// <param name="commandParameter"> Parameter to te command </param>
        public static List<MenuFlyoutItemViewModel> WithCopy(this List<MenuFlyoutItemViewModel> menu,
            IRelayCommand command, object? commandParameter = null)
        {
            return menu.FromCommandData(new CommandData("Copy", Constants.FluentIcons.Copy, command, commandParameter));
        }

        /// <summary>
        /// Adds delete menu item to an existing menu
        /// </summary>
        /// <param name="menu"> Provided menu </param>
        /// <param name="command"> Command to delete item or items  </param>
        /// <param name="commandParameter"> Parameter to te command </param>
        public static List<MenuFlyoutItemViewModel> WithDelete(this List<MenuFlyoutItemViewModel> menu,
            IRelayCommand command, object? commandParameter = null)
        {
            return menu.FromCommandData(new CommandData("Delete", Constants.FluentIcons.Delete, command, commandParameter));
        }


        /// <summary>
        /// Adds pin menu item to an existing menu
        /// </summary>
        /// <param name="menu"> Provided menu </param>
        /// <param name="command"> Command to pin item </param>
        /// <param name="commandParameter"> Parameter to te command </param>
        public static List<MenuFlyoutItemViewModel> WithPin(this List<MenuFlyoutItemViewModel> menu,
            IRelayCommand command, object? commandParameter = null)
        {
            return menu.FromCommandData(new CommandData("Pin", Constants.FluentIcons.Pin, command, commandParameter));
        }

        /// <summary>
        /// Adds unpin menu item to an existing menu
        /// </summary>
        /// <param name="menu"> Provided menu </param>
        /// <param name="command"> Command to unpin item </param>
        /// <param name="commandParameter"> Parameter to te command </param>
        public static List<MenuFlyoutItemViewModel> WithUnpin(this List<MenuFlyoutItemViewModel> menu,
            IRelayCommand command, object? commandParameter = null)
        {
            return menu.FromCommandData(new CommandData("Unpin", Constants.FluentIcons.Unpin, command, commandParameter));
        }

        /// <summary>
        /// Creates new menu item from <see cref="CommandData"/> provided
        /// </summary>
        /// <param name="menu"> Menu that we are adding item into </param>
        /// <param name="commandDataData"> All needed data to create menu item</param>
        private static List<MenuFlyoutItemViewModel> FromCommandData(this List<MenuFlyoutItemViewModel> menu, CommandData commandDataData)
        {
            var menuItem = new MenuFlyoutItemViewModel(commandDataData.Name)
            {
                IconGlyph = commandDataData.IconGlyph,

            }.WithCommand(commandDataData.Command, commandDataData.CommandParameter);

            menu.Add(menuItem);
            return menu;
        }

        private static MenuFlyoutItemViewModel WithCommand(this MenuFlyoutItemViewModel item, IRelayCommand command, object? commandParameter)
        {
            item.Command = command;
            item.CommandParameter = commandParameter;
            return item;
        }
    }
}
