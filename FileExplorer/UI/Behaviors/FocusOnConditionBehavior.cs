using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace FileExplorer.UI.Behaviors
{
    public class FocusOnConditionBehavior : Behavior<TextBox>
    {

        public bool Condition
        {
            get => (bool)GetValue(ConditionProperty);
            set => SetValue(ConditionProperty, value);

        }

        public static readonly DependencyProperty ConditionProperty =
            DependencyProperty.Register(nameof(Condition), typeof(bool),
                typeof(FocusOnConditionBehavior), new PropertyMetadata(false, OnConditionChanged));

        private static void OnConditionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FocusOnConditionBehavior behavior)
            {
                if ((bool)e.NewValue)
                {
                    behavior.AssociatedObject.Focus(FocusState.Programmatic);
                    behavior.AssociatedObject.SelectAll();
                }
            }
        }
    }
}
