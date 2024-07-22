#nullable enable
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;

namespace FileExplorer.Models
{
    public sealed class MenuFlyoutItemViewModel
    {
        public string Text { get; set; }
        public IRelayCommand? Command { get; set; }
        public object? CommandParameter { get; set; }
        public List<MenuFlyoutItemViewModel>? Items { get; set; }
        public string? IconGlyph { get; set; }

        public MenuFlyoutItemViewModel(string text)
        {
            Text = text;
        }
    }
}
