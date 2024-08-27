using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml.Controls;
using Models.Contracts.Storage;

namespace FileExplorer.UI.Behaviors.Collections
{
    /// <summary>
    /// Behavior that runs command only with <see cref="IDirectoryItem"/> command parameter 
    /// </summary>
    public sealed class DirectoryItemDoubleClickBehavior : BaseDoubleTappedTypeSafeBehavior<ListViewBase, IDirectoryItem>;
}
