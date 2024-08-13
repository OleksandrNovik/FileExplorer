﻿#nullable enable
using FileExplorer.Core.Contracts;
using Helpers.Application;
using Microsoft.UI.Xaml.Controls;
using System;

namespace FileExplorer.Core.Services.General
{
    public abstract class BaseNavigationService
    {
        private Frame? frame;
        public Frame? Frame
        {
            get => frame;
            set
            {
                UnregisterFrameEvents();
                frame = value;
                RegisterFrameEvents();
            }
        }

        private void RegisterFrameEvents()
        {
            if (frame != null)
            {
                frame.Navigated += OnNavigated;
            }
        }

        private void UnregisterFrameEvents()
        {
            if (frame != null)
            {
                frame.Navigated -= OnNavigated;
            }
        }

        protected virtual void OnNavigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (sender is Frame frame)
            {
                if (frame.GetPageViewModel() is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedTo(e.Parameter);
                }
            }
        }

        protected void UseNavigationFrame(Type pageType, object? parameter = null)
        {
            ArgumentNullException.ThrowIfNull(Frame);

            var previousViewModel = Frame.GetPageViewModel();

            bool navigated = Frame.Navigate(pageType, parameter);

            if (navigated)
            {
                if (previousViewModel is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedFrom();
                }
            }
        }
    }
}
