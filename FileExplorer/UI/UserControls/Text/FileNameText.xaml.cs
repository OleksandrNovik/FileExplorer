using FileExplorer.ViewModels.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.Xaml.Interactivity;
using System.IO;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls.Text
{
    public sealed partial class FileNameText : BehaviorSettingUserControl
    {
        public static readonly DependencyProperty IsEditProperty =
            DependencyProperty.Register(nameof(IsEdit), typeof(bool),
                typeof(FileNameText), new PropertyMetadata(false));

        public static readonly DependencyProperty ShowExtensionProperty =
            DependencyProperty.Register(nameof(ShowExtension), typeof(bool),
                typeof(FileNameText), new PropertyMetadata(false, OnTextChangeRequired));


        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register(nameof(FileName), typeof(string),
                typeof(FileNameText), new PropertyMetadata(string.Empty, OnTextChangeRequired));

        public static readonly DependencyProperty TextBoxBehaviorsProperty =
            DependencyProperty.Register(nameof(TextBoxBehaviors), typeof(BehaviorCollection),
                typeof(FileNameText), new PropertyMetadata(new BehaviorCollection(), OnTextBoxBehaviorsChanged));

        private static void OnTextBoxBehaviorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (FileNameText)d;

            if (e.NewValue is BehaviorCollection collection)
            {
                control.SetBehaviors(control.TextBox, collection);
            }
        }

        private static void OnTextChangeRequired(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FileNameText text)
            {
                text.GetFileName();
            }
        }

        /// <summary>
        /// Model that contains both presentation (name + extension) and editable part (editable name)
        /// This is useful to maintain ability to turn of show extension and still get good result
        /// Or edit extension if show extensions is turned on
        /// </summary>
        private FileNamingTextViewModel ViewModel { get; }

        /// <summary>
        /// True if item should be in edit mode or false if view mode
        /// </summary>
        public bool IsEdit
        {
            get => (bool)GetValue(IsEditProperty);
            set => SetValue(IsEditProperty, value);
        }

        /// <summary>
        /// Decides if this file name text shows file extension or not
        /// </summary>
        public bool ShowExtension
        {
            get => (bool)GetValue(ShowExtensionProperty);
            set => SetValue(ShowExtensionProperty, value);
        }

        /// <summary>
        /// Name of file to edit and show
        /// </summary>
        public string FileName
        {
            get => (string)GetValue(FileNameProperty);
            set => SetValue(FileNameProperty, value);
        }

        public BehaviorCollection TextBoxBehaviors
        {
            get => (BehaviorCollection)GetValue(TextBoxBehaviorsProperty);
            set => SetValue(TextBoxBehaviorsProperty, value);
        }

        public FileNameText()
        {
            ViewModel = App.GetService<FileNamingTextViewModel>();
            this.InitializeComponent();
        }

        private void GetFileName()
        {
            if (FileName is not null)
            {
                // Separate file's name and extension
                ViewModel.Name = Path.GetFileNameWithoutExtension(FileName);
                ViewModel.Extension = Path.GetExtension(FileName);

                ViewModel.EditableName = ShowExtension
                    ? ViewModel.Name + ViewModel.Extension
                    : ViewModel.Name;
            }
        }

        private void SetFileName()
        {
            if (FileName is not null)
            {
                var fileExtension = Path.GetExtension(FileName);

                if (ShowExtension)
                {
                    var newExtension = Path.GetExtension(ViewModel.EditableName);

                    // Extenstion has changed
                    if (newExtension != fileExtension)
                    {
                        // File should get new name and extension
                        FileName = ViewModel.EditableName;
                    }
                    else
                    {
                        FileName = Path.GetFileNameWithoutExtension(ViewModel.EditableName) + fileExtension;
                    }
                }
                // If there are no extension maybe it is turned off for now
                else
                {
                    // So file should get its new name + old extension (that has no changed)
                    FileName = ViewModel.EditableName + fileExtension;
                }

            }
        }
        private void OnTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            // On enter save changes in file name
            if (e.Key is VirtualKey.Enter)
            {
                SetFileName();
            }
        }
    }
}
