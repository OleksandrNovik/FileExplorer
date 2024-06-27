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

        /// <summary>
        /// Focuses <see cref="TextBox"/> when folder's or file's name is edited
        /// Selects text inside of <see cref="TextBox"/>
        /// </summary>
        /// <param name="d"> <see cref="IsRenamed"/> new value </param>
        /// <param name="e"> Event arguments </param>
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
        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            // TODO: End editing item in here
            IsRenamed = false;
        }

    }
}
