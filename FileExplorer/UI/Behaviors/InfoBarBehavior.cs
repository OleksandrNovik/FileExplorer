﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using System;

namespace FileExplorer.UI.Behaviors
{
    public class InfoBarBehavior : Behavior<InfoBar>
    {
        private readonly DispatcherTimer closeTimer;
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool),
                typeof(InfoBarBehavior), new PropertyMetadata(false, OnRenamedPropertyChanged));

        public InfoBarBehavior()
        {
            closeTimer = new DispatcherTimer();
            closeTimer.Interval = TimeSpan.FromSeconds(2);
            closeTimer.Tick += CloseAfterTimeout;
        }

        private void CloseAfterTimeout(object sender, object e)
        {
            AssociatedObject.IsOpen = false;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Closing += OnClosing;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Closing -= OnClosing;
        }

        private void OnClosing(InfoBar sender, InfoBarClosingEventArgs args)
        {
            closeTimer.Stop();
        }

        private static void OnRenamedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is InfoBarBehavior behavior)
            {
                if ((bool)e.NewValue)
                {
                    behavior.closeTimer.Start();
                }
            }
        }
    }
}
