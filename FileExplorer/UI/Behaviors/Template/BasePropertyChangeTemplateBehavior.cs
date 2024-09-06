using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace FileExplorer.UI.Behaviors.Template
{
    public abstract class BasePropertyChangeTemplateBehavior : Behavior<ContentControl>
    {
        /// <summary>
        /// Template selector that contains logic to select template depending on property 
        /// </summary>
        public DataTemplateSelector TemplateSelector { get; set; }

        /// <summary>
        /// Method that is invoked when some property, which defines template is changed 
        /// </summary>
        protected static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BasePropertyChangeTemplateBehavior { AssociatedObject: not null } behavior)
            {
                behavior.AssociatedObject.ContentTemplate = behavior.TemplateSelector.SelectTemplate(e.NewValue);
            }
        }
    }
}
