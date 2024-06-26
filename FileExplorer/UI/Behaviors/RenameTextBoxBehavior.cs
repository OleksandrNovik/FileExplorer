using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace FileExplorer.UI.Behaviors
{
    public class RenameTextBoxBehavior : Behavior<TextBox>
    {
        public bool IsRenamed
        {
            get => (bool)GetValue(IsRenamedProperty);
            set => SetValue(IsRenamedProperty, value);
        }

        public static readonly DependencyProperty IsRenamedProperty =
            DependencyProperty.Register(nameof(IsRenamed), typeof(bool),
                typeof(RenameTextBoxBehavior), new PropertyMetadata(false, OnRenamedPropertyChanged));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.LostFocus += OnLostFocus;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.LostFocus -= OnLostFocus;
        }

        public static void OnRenamedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RenameTextBoxBehavior behaviour)
            {
                if ((bool)e.NewValue)
                {
                    behaviour.AssociatedObject.Focus(FocusState.Programmatic);
                    behaviour.AssociatedObject.SelectAll();
                }
            }
        }

        /// <summary>
        /// If user loses focus on <see cref="TextBox"/> file/folder name is no longer edited
        /// </summary>
        /// <param name="sender"> Event sender (TextBox) </param>
        /// <param name="e"> Event args </param>
        public void OnLostFocus(object sender, RoutedEventArgs e)
        {
            IsRenamed = false;
        }

    }
}
