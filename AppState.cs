using MorcuTool.ViewModels.Pages;

namespace MorcuTool;

public static class AppState
{
    public static Package activePackage = new();

    public static Vault activeVault = new();

    public static PackageViewPageViewModel activePackageViewPageViewModel = new();
}