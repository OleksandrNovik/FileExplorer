using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.DirectoriesNavigation;
using FileExplorer.Core.Contracts.Factories;
using FileExplorer.Core.Contracts.General;
using FileExplorer.Core.Contracts.Settings;
using FileExplorer.Core.Services;
using FileExplorer.Core.Services.DirectoriesNavigation;
using FileExplorer.Core.Services.Factories;
using FileExplorer.Core.Services.Settings;
using FileExplorer.Services;
using FileExplorer.ViewModels;
using FileExplorer.ViewModels.General;
using FileExplorer.ViewModels.Informational;
using FileExplorer.ViewModels.Settings;
using FileExplorer.Views;
using Helpers.Application;
using Helpers.General;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Models.Contracts.Storage;
using System;
using Hosting = Microsoft.Extensions.Hosting.Host;
using SearchOptionsViewModel = FileExplorer.ViewModels.Search.SearchOptionsViewModel;
using SettingsViewModel = FileExplorer.ViewModels.Settings.SettingsViewModel;


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
            ThreadingHelper.InitializeForMainThread();

            this.Host = Hosting.CreateDefaultBuilder()
                .UseContentRoot(AppContext.BaseDirectory)
                .ConfigureServices((context, services) =>
                {
                    services.AddPageAndViewModel<DirectoryPage, DirectoryPageViewModel>();
                    services.AddPageAndViewModel<ShellPage, ShellPageViewModel>();

                    // History and directory navigation services
                    services.AddTransient<IHistoryNavigationService, HistoryNavigationService>();
                    services.AddTransient<IDirectoryRouteService, DirectoryRouteService>();

                    // Tab services
                    services.AddTransient<ITabService, TabsService>();

                    //Navigation
                    services.AddTransient<IPageTypesService, PageTypesService>();
                    services.AddSingleton<INavigationService, NavigationService>();

                    //Settings services
                    services.AddTransient<IBasicPageService<string>, BasicPageService>();
                    services.AddSingleton<ISettingsNavigationService, SettingsNavigationService>();

                    // Factories
                    services.AddTransient<IMenuFlyoutFactory, MenuFlyoutFactory>();
                    services.AddTransient<IStorageFactory<IDirectoryItem>, StorageFactory>();

                    services.AddTransient<HomePageViewModel>();
                    services.AddTransient<TabNavigationViewModel>();

                    //Settings
                    services.AddTransient<SettingsViewModel>();
                    services.AddTransient<SettingsPreferencesViewModel>();
                    services.AddTransient<SettingsExplorerViewModel>();

                    // Search
                    services.AddTransient<SearchOptionsViewModel>();

                    // Informational
                    services.AddTransient<DirectoryItemInfoViewModel>();
                    services.AddTransient<InfoBarViewModel>();

                    //File operations
                    services.AddTransient(_ => GetStaticResource<FileOperationsViewModel>("FileOperations"));

                    //ViewOptions
                    services.AddTransient<ViewOptionsViewModel>();

                })
                .Build();

            this.InitializeComponent();
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

        public static T GetStaticResource<T>(object key)
        {
            if (App.Current.Resources.TryGetValue(key, out object value) && value is T resource)
            {
                return resource;
            }

            throw new ArgumentException($"No resource with type {typeof(T)} that has provided key", nameof(key));
        }
    }
}
