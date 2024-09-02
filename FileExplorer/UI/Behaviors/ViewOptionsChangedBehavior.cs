using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using Models.Messages;

namespace FileExplorer.UI.Behaviors
{
    public class ViewOptionsChangedBehavior : Behavior<ContentControl>
    {
        public DataTemplateSelector TemplateSelector { get; set; }

        public static readonly DependencyProperty ViewOptionsProperty =
            DependencyProperty.Register(nameof(ViewOptions), typeof(ViewOptions),
                typeof(ViewOptionsChangedBehavior), new PropertyMetadata(ViewOptions.GridView, OnViewOptionsChanged));
        public ViewOptions ViewOptions
        {
            get => (ViewOptions)GetValue(ViewOptionsProperty);
            set => SetValue(ViewOptionsProperty, value);
        }

        private static void OnViewOptionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ViewOptionsChangedBehavior { AssociatedObject: not null } behavior)
            {
                behavior.AssociatedObject.ContentTemplate = behavior.TemplateSelector.SelectTemplate(e.NewValue);
            }
        }

    }
}
