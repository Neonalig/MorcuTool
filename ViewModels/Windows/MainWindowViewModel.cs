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

    NavigationViewItem packagesItem;

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
            packagesItem = new NavigationViewItem
            {
                Content = "Packages",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Apps24 },
                TargetPageType = typeof(PackageViewPage),
                Visibility = Visibility.Collapsed,
            },

            // DEBUG:
            new NavigationViewItem
            {
                Content = "Toggle Packages",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Bug24 },
                Command = new RelayCommand(() => packagesItem.Visibility = packagesItem.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible),
            }
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