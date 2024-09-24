using CommunityToolkit.Mvvm.ComponentModel;

namespace FileExplorer.ViewModels.Controls
{
    public sealed partial class FileNamingTextViewModel : ObservableObject
    {
        /// <summary>
        /// Name without extension for view purposes
        /// </summary>
        [ObservableProperty]
        private string name;

        /// <summary>
        /// Extension to show or hide it from the name separately (for view purposes only)
        /// </summary>
        [ObservableProperty]
        private string extension;

        /// <summary>
        /// Editable name + extension part (or name part when extensions are hidden)
        /// this property is for edit purposes
        /// </summary>
        [ObservableProperty]
        private string editableName;
    }
}
