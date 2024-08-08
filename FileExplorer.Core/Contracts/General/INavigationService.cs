#nullable enable
using Microsoft.UI.Xaml.Controls;

namespace FileExplorer.Core.Contracts.General
{
    public interface INavigationService<in TParam>
    {
        public Frame? Frame { get; set; }

        public void NavigateTo(TParam value);
    }
}
