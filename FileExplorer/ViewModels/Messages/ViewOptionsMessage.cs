#nullable enable
namespace FileExplorer.ViewModels.Messages
{
    public enum SortOptions
    {
        /// <summary>
        /// Use option that is already picked
        /// </summary>
        Default,
        Name,
        Date,
        Size
    }
    public enum ViewOptions
    {
        /// <summary>
        /// Option that is already picked
        /// </summary>
        Default,
        //TODO: Create new ViewOptions like in windows explorer
        GridView,
        ListView
    }
    public class ViewOptionsMessage
    {
        public ViewOptions ViewOption { get; }
        public SortOptions SortBy { get; }

        public ViewOptionsMessage(ViewOptions viewOptions = ViewOptions.Default, SortOptions sortBy = SortOptions.Default)
        {
            ViewOption = viewOptions;
            SortBy = sortBy;
        }
    }
}
