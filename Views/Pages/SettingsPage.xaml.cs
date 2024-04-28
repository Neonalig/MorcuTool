using MorcuTool.ViewModels.Pages;

namespace MorcuTool.Views.Pages;

public partial class SettingsPage
{
    public SettingsPageViewModel ViewModel { get; }

    public SettingsPage(SettingsPageViewModel viewModel)
    {
        DataContext = ViewModel = viewModel;
        InitializeComponent();
    }

    void SettingsPage_OnUnloaded(object sender, RoutedEventArgs e)
    {
        ViewModel.PromptSaveChanges();
    }
}