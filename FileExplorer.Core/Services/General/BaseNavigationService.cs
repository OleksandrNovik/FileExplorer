#nullable enable
using FileExplorer.Core.Contracts;
using Helpers.Application;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace FileExplorer.Core.Services.General;

/// <summary>
/// Basic navigation service that contains all necessary methods and members for a navigation service
/// </summary>
public abstract class BaseNavigationService
{
    /// <summary>
    /// Decides if navigation services caches navigated pages 
    /// </summary>
    protected bool enableCache = true;

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

    protected virtual void OnNavigated(object sender, NavigationEventArgs e)
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
        bool navigated;

        if (enableCache)
        {
            navigated = Frame.Navigate(pageType, parameter);
        }
        else
        {
            navigated = Frame.NavigateToType(pageType, parameter, new FrameNavigationOptions
            {
                IsNavigationStackEnabled = false
            });
        }

        if (navigated)
        {
            if (previousViewModel is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedFrom();
            }
        }
    }
}