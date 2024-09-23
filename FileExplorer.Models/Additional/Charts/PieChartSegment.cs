using Microsoft.UI.Xaml.Media;

namespace FileExplorer.Models.Additional.Charts
{
    /// <summary>
    /// Models that represents pie chart segment
    /// </summary>
    public sealed class PieChartSegment
    {
        /// <summary>
        /// Value of segment
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Color for a segment on the chart
        /// </summary>
        public Brush Color { get; set; }
    }
}
