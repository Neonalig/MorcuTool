namespace MorcuTool.Views.Pages;

public partial class PackageViewPage
{
    public PackageViewPage()
    {
        DataContext = AppState.activePackageViewPageViewModel;
        InitializeComponent();
    }
}