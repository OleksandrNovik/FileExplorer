using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;

namespace FileExplorer.Helpers
{
    public static class WindowHelper
    {
        public static List<Window> ActiveWindows { get; } = new();

        public static Window CreateWindow()
        {
            Window newWindow = new Window
            {
                SystemBackdrop = new MicaBackdrop()
            };
            TrackWindow(newWindow);
            return newWindow;
        }

        public static void TrackWindow(Window window)
        {
            window.Closed += (sender, args) =>
            {
                ActiveWindows.Remove(window);
            };
            ActiveWindows.Add(window);
        }

        public static Window GetWindowForElement(UIElement element)
        {
            if (element.XamlRoot != null)
            {
                foreach (Window window in ActiveWindows)
                {
                    if (element.XamlRoot == window.Content.XamlRoot)
                    {
                        return window;
                    }
                }
            }
            return null;
        }
    }
}
