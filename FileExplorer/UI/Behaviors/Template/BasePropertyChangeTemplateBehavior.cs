using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace FileExplorer.UI.Behaviors.Template
{
    public abstract class BasePropertyChangeTemplateBehavior<TProperty> : Behavior<ContentControl>
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(TProperty),
                typeof(BasicFilePropertiesChangedBehavior), new PropertyMetadata(null, OnPropertyChanged));

        public TProperty Value
        {
            get => (TProperty)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Template selector that contains logic to select template depending on property 
        /// </summary>
        public DataTemplateSelector TemplateSelector { get; set; }

        /// <summary>
        /// Method that is invoked when some property, which defines template is changed 
        /// </summary>
        protected static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BasePropertyChangeTemplateBehavior<TProperty> { AssociatedObject: not null } behavior)
            {
                behavior.AssociatedObject.ContentTemplate = behavior.TemplateSelector.SelectTemplate(e.NewValue);
            }
        }
    }
}
