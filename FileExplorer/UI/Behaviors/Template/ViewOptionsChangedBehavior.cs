using Microsoft.UI.Xaml;
using Models.Messages;

namespace FileExplorer.UI.Behaviors.Template
{
    /// <summary>
    /// Defines view option property, which when changed selects new template
    /// </summary>
    public sealed class ViewOptionsChangedBehavior : BasePropertyChangeTemplateBehavior
    {
        /// <summary>
        /// Dependency property to register to property changed event 
        /// </summary>
        public static readonly DependencyProperty ViewOptionsProperty =
            DependencyProperty.Register(nameof(ViewOptions), typeof(ViewOptions),
                typeof(ViewOptionsChangedBehavior), new PropertyMetadata(ViewOptions.GridView, OnPropertyChanged));
        public ViewOptions ViewOptions
        {
            get => (ViewOptions)GetValue(ViewOptionsProperty);
            set => SetValue(ViewOptionsProperty, value);
        }
    }
}
