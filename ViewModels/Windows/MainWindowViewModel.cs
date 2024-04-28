using CommunityToolkit.Mvvm.Input;
using MorcuTool.ViewModels;
using MorcuTool.Views.Pages;
using MenuItem = Wpf.Ui.Controls.MenuItem;

namespace MorcuTool;

public sealed class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<object> NavigationItems { get; }
    public ObservableCollection<object> NavigationFooter { get; }
    public ObservableCollection<MenuItem> TrayMenuItems { get; }

    public MainWindowViewModel()
    {
        NavigationItems =
        [
            new NavigationViewItem
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(HomePage),
            },
        ];

        NavigationFooter =
        [
            new NavigationViewItem
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(SettingsPage)
            },
        ];

        TrayMenuItems =
        [
            new MenuItem
            {
                Header = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                Tag = "navigate:"+nameof(SettingsPage),
            },
            new MenuItem
            {
                Header = "Exit",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Dismiss24 },
                Command = new RelayCommand(() => Application.Current.Shutdown()),
            },
        ];
    }
}