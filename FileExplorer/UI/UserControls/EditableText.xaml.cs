#nullable enable
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls
{
    public sealed partial class EditableText : UserControl
    {
        public static readonly DependencyProperty IsEditedProperty =
            DependencyProperty.Register(nameof(IsEdited), typeof(bool),
                typeof(EditableText), new PropertyMetadata(false));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string),
                typeof(EditableText), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty TextBlockBehaviorsProperty =
            DependencyProperty.Register(nameof(TextBlockBehaviors), typeof(BehaviorCollection),
                typeof(EditableText), new PropertyMetadata(new BehaviorCollection(), OnTextBlockBehaviorsChanged));

        public static readonly DependencyProperty TextBoxBehaviorsProperty =
            DependencyProperty.Register(nameof(TextBoxBehaviors), typeof(BehaviorCollection),
                typeof(EditableText), new PropertyMetadata(new BehaviorCollection(), OnTextBoxBehaviorsChanged));

        private static void OnTextBoxBehaviorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (EditableText)d;

            if (e.NewValue is BehaviorCollection collection)
            {
                control.SetBehaviors(control.TextBox, collection);
            }
        }

        private static void OnTextBlockBehaviorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (EditableText)d;

            if (e.NewValue is BehaviorCollection collection)
            {
                control.SetBehaviors(control.TextBlock, collection);
            }
        }

        public bool IsEdited
        {
            get => (bool)GetValue(IsEditedProperty);
            set => SetValue(IsEditedProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public BehaviorCollection TextBlockBehaviors
        {
            get => (BehaviorCollection)GetValue(TextBlockBehaviorsProperty);
            set => SetValue(TextBlockBehaviorsProperty, value);
        }

        public BehaviorCollection TextBoxBehaviors
        {
            get => (BehaviorCollection)GetValue(TextBoxBehaviorsProperty);
            set => SetValue(TextBoxBehaviorsProperty, value);
        }

        public EditableText()
        {
            this.InitializeComponent();
        }

        private void SetBehaviors(DependencyObject destination, BehaviorCollection source)
        {
            var behaviors = Interaction.GetBehaviors(destination);

            if (behaviors is not null)
            {
                foreach (var dependencyObj in source)
                {
                    if (dependencyObj is Behavior behavior)
                    {
                        behavior.Attach(destination);
                        behaviors.Add(dependencyObj);
                    }
                }
            }
        }
    }
}
