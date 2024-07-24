using FileExplorer.Contracts;
using FileExplorer.Helpers;
using FileExplorer.Services;
using FileExplorer.ViewModels;
using FileExplorer.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;
using Hosting = Microsoft.Extensions.Hosting.Host;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static WindowExtended MainWindow { get; } = new MainWindow();
        public IHost Host { get; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// 
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Host = Hosting.CreateDefaultBuilder()
                .UseContentRoot(AppContext.BaseDirectory)
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<CommonFileOperationsViewModel>();

                    services.AddPageAndViewModel<DirectoryPage, DirectoryPageViewModel>();
                    services.AddPageAndViewModel<ShellPage, ShellPageViewModel>();

                    services.AddTransient<IHistoryNavigationService, HistoryNavigationService>();
                    services.AddTransient<IDirectoryRouteService, DirectoryRouteService>();
                    services.AddTransient<IMenuFlyoutFactory, MenuFlyoutFactory>();
                    services.AddTransient<IPageService, PageService>();
                    services.AddTransient<IDirectoryManager, DirectoryManager>();

                    services.AddTransient<DirectoriesNavigationViewModel>();
                    services.AddTransient<SearchOperationsViewModel>();
                    services.AddTransient<DirectoryToolBarViewModel>();

                    services.AddSingleton<ITabService, TabsService>();
                })
                .Build();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            m_window = MainWindow;
            WindowHelper.TrackWindow(MainWindow);
            m_window.Content = GetService<ShellPage>();
            m_window.Activate();
        }

        private Window m_window;

        /// <summary>
        /// Method for retrieving a registered service of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of service to retrieve.</typeparam>
        /// <returns>The registered service instance.</returns>
        public static T GetService<T>()
            where T : class
        {
            if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
            {
                throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
            }

            return service;
        }
    }
}
