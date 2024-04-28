using MorcuTool.Views.Pages;
using Wpf.Ui;

namespace MorcuTool.Services;

public sealed class ApplicationHostService : IHostedService
{
    readonly IServiceProvider serviceProvider;
    INavigationWindow? navigationWindow;

    public ApplicationHostService(IServiceProvider serviceProvider)
        => this.serviceProvider = serviceProvider;

    public Task StartAsync(CancellationToken cancellationToken) => HandleActivationAsync();

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    async Task HandleActivationAsync()
    {
        await Task.CompletedTask;

        if (!Application.Current.Windows.OfType<MainWindow>().Any())
        {
            object service = serviceProvider.GetService(typeof(INavigationWindow)) ?? throw new InvalidOperationException("The navigation window is not available.");
            navigationWindow = (INavigationWindow)service;
            navigationWindow.ShowWindow();

            _ = navigationWindow.Navigate(typeof(HomePage));
        }

        await Task.CompletedTask;
    }
}