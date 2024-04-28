using MorcuTool.Models;
using MorcuTool.Services;
using MorcuTool.ViewModels.Pages;
using MorcuTool.Views.Pages;

namespace MorcuTool;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    static readonly IHost host =
        Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(c =>
            {
                var basePath =
                    Path.GetDirectoryName(AppContext.BaseDirectory)
                    ?? throw new DirectoryNotFoundException(
                        "Unable to find the base directory of the application."
                    );
                _ = c.SetBasePath(basePath);
            })
            .ConfigureServices(
                (context, services) =>
                {
                    // App Host
                    _ = services.AddHostedService<ApplicationHostService>();

                    // Page resolver service
                    _ = services.AddSingleton<IPageService, PageService>();

                    // Theme manipulation
                    _ = services.AddSingleton<IThemeService, ThemeService>();

                    // TaskBar manipulation
                    _ = services.AddSingleton<ITaskBarService, TaskBarService>();

                    // Service containing navigation, same as INavigationWindow... but without window
                    _ = services.AddSingleton<INavigationService, Wpf.Ui.NavigationService>();

                    // Main window with navigation
                    _ = services.AddSingleton<INavigationWindow, MainWindow>();
                    _ = services.AddSingleton<MainWindowViewModel>();

                    // Views and ViewModels
                    _ = services.AddSingleton<HomePage>();
                    _ = services.AddSingleton<HomePageViewModel>();
                    _ = services.AddSingleton<PackageViewPage>();
                    _ = services.AddSingleton<PackageViewPageViewModel>();
                    _ = services.AddSingleton<SettingsPage>();
                    _ = services.AddSingleton<SettingsPageViewModel>();

                    // Configuration
                    services.AddSingleton<AppConfig>(serviceProvider =>
                    {
                        var config = new AppConfig();
                        SettingsContext context = new();
                        SettingsPropertyCollection properties = new();
                        SettingsProviderCollection providers = new();
                        config.Initialize(context, properties, providers);
                        return config;
                    });
                }
            )
            .Build();

    async void App_OnStartup(object sender, StartupEventArgs e)
    {
        await host.StartAsync();
    }

    async void App_OnExit(object sender, ExitEventArgs e)
    {
        await host.StopAsync();
        host.Dispose();
    }

    void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        _ = MessageBox.Show(
            e.Exception.Message,
            "Unhandled Exception",
            MessageBoxButton.OK,
            MessageBoxImage.Error
        );
    }
}