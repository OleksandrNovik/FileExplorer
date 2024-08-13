using CommunityToolkit.WinUI.UI;
using FileExplorer.ViewModels;
using Helpers.General;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Models.StorageWrappers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DirectoryPage : Page
    {
        public DirectoryPageViewModel ViewModel { get; }
        public DirectoryPage()
        {
            this.ViewModel = App.GetService<DirectoryPageViewModel>();
            this.Resources["EndRenamingCommand"] = this.ViewModel.EndRenamingItemCommand;
            this.Resources["EndRenamingIfLostFocusCommand"] = this.ViewModel.FileOperations.EndRenamingIfNeededCommand;
            this.Resources["OpenCommand"] = this.ViewModel.FileOperations.OpenCommand;
            this.InitializeComponent();
        }

        private void SelectAllItems(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            DirectoryItemsGrid.SelectAll();
        }

        private void OnDirectoryItemsGridRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var tappedElement = e.OriginalSource as FrameworkElement;
            ContextMenu.Items.Clear();

            // If grid item was right-clicked
            if (tappedElement?.DataContext is DirectoryItemWrapper wrapper)
            {
                // Item is not selected
                if (!DirectoryItemsGrid.SelectedItems.Contains(wrapper))
                {
                    // Clear previously selected items
                    DirectoryItemsGrid.DeselectAll();
                    // Select item that user right-clicked on
                    DirectoryItemsGrid.SelectedItem = wrapper;
                }

            }
            // If grid itself was right-clicked
            else if (DirectoryItemsGrid.SelectedItems.Count > 0)
            {
                DirectoryItemsGrid.DeselectAll();
            }

            ContextMenu.Items.AddRange(ViewModel.OnContextMenuRequired());
            ContextMenu.ShowAt(tappedElement, e.GetPosition(tappedElement));
        }
    }
}
