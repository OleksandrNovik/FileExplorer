using FileExplorer.Models.Storage.Abstractions;
using FileExplorer.UI.Behaviors.BaseBehaviors;
using Microsoft.UI.Xaml.Controls;

namespace FileExplorer.UI.Behaviors.Collections
{
    /// <summary>
    /// Behavior that runs command only with <see cref="InteractiveStorageItem"/> command parameter 
    /// </summary>
    public sealed class DirectoryItemDoubleClickBehavior : BaseDoubleTappedTypeSafeBehavior<Control, InteractiveStorageItem>;
}
