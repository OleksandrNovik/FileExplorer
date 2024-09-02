#nullable enable
namespace Models.Messages
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
        //TODO: Create new ViewOptions like in windows explorer
        GridView,
        TableView
    }

    public record ViewOptionsChangedMessage(ViewOptions ViewOptions);
}
