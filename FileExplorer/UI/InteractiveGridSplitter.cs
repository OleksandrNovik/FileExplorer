using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Input;
using GridSplitter = CommunityToolkit.WinUI.UI.Controls.GridSplitter;

namespace FileExplorer.UI
{
    /// <summary>
    /// Grid splitter that has intuitive pointer when dragged
    /// </summary>
    public class InteractiveGridSplitter : GridSplitter
    {
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.SizeWestEast);
            base.OnPointerEntered(e);
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
            base.OnPointerExited(e);
        }
    }
}
