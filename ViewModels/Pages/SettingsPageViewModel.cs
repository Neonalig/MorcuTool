using CommunityToolkit.Mvvm.Input;
using MorcuTool.Models;

namespace MorcuTool.ViewModels.Pages;

public sealed class SettingsPageViewModel : ViewModelBase, INavigationAware
{
    readonly AppConfig _appConfig;

    public SettingsPageViewModel(AppConfig appConfig)
    {
        _appConfig = appConfig;

        SaveCommand = new RelayCommand(SaveChanges);
        DiscardCommand = new RelayCommand(DiscardChanges);
        ResetCommand = new RelayCommand(ResetSettings);

        AboutBNKExtrCommand = new RelayCommand(() => Utility.OpenInBrowser("https://github.com/eXpl0it3r/bnkextr"));
        AboutVGMStreamCommand = new RelayCommand(() => Utility.OpenInBrowser("https://vgmstream.org/"));
        AboutHKXCMDCommand = new RelayCommand(() => Utility.OpenInBrowser("https://www.nexusmods.com/skyrim/mods/1797"));
        AboutHKDumpCommand = new RelayCommand(() => Utility.OpenInBrowser("https://github.com/opparco/hkdump"));
        AboutHavok2FBXCommand = new RelayCommand(() => Utility.OpenInBrowser("https://github.com/Highflex/havok2fbx"));
    }

    public void PromptSaveChanges()
    {
        if (!_appConfig.IsDirty)
        {
            Console.WriteLine("Settings are not dirty, returning...");
            return;
        }

        Console.WriteLine("Settings are dirty, prompting user...");
        MessageBoxResult result = MessageBox.Show(
            "Do you want to save the changes you made to the settings?",
            "MorcuTool",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question
        );

        switch (result)
        {
            case MessageBoxResult.Yes:
                SaveChanges();
                break;
            case MessageBoxResult.No:
                DiscardChanges();
                break;
        }
    }

    public void ResetSettings()
    {
        _appConfig.Reset();
        NotifyAllProperties();
    }

    public void SaveChanges()
    {
        _appConfig.Save();
        NotifyAllProperties();
    }

    public void DiscardChanges()
    {
        _appConfig.Reload();
        NotifyAllProperties();
    }

    void NotifyAllProperties()
    {
        OnPropertyChanged(nameof(BNKConversionMethod));
        OnPropertyChanged(nameof(BNKExtrPathVisibility));
        OnPropertyChanged(nameof(BNKExtrPath));
        OnPropertyChanged(nameof(VGMStreamPathVisibility));
        OnPropertyChanged(nameof(VGMStreamPath));
        OnPropertyChanged(nameof(HKXConversionMethod));
        OnPropertyChanged(nameof(HKXCMDPathVisibility));
        OnPropertyChanged(nameof(HKXCMDPath));
        OnPropertyChanged(nameof(HKDumpPathVisibility));
        OnPropertyChanged(nameof(HKDumpPath));
        OnPropertyChanged(nameof(Havok2FBXPathVisibility));
        OnPropertyChanged(nameof(Havok2FBXPath));
    }

    void INavigationAware.OnNavigatedTo()
    {
    }

    void INavigationAware.OnNavigatedFrom()
    {
        PromptSaveChanges();
    }

    public ObservableCollection<BNKConversionMethod> BNKConversionMethods { get; } = new(Enum.GetValues<BNKConversionMethod>());

    public BNKConversionMethod BNKConversionMethod
    {
        get => _appConfig.BNKConversionMethod;
        set
        {
            _appConfig.BNKConversionMethod = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(BNKExtrPathVisibility));
            OnPropertyChanged(nameof(VGMStreamPathVisibility));
        }
    }

    public Visibility BNKExtrPathVisibility => BNKConversionMethod is BNKConversionMethod.BNKExtr_WwiseWEM ? Visibility.Visible : Visibility.Collapsed;

    public string BNKExtrPath
    {
        get => _appConfig.BNKExtrPath;
        set
        {
            _appConfig.BNKExtrPath = value;
            OnPropertyChanged();
        }
    }

    public Visibility VGMStreamPathVisibility => BNKConversionMethod is BNKConversionMethod.VGMStream_WAV ? Visibility.Visible : Visibility.Collapsed;

    public string VGMStreamPath
    {
        get => _appConfig.VGMStreamPath;
        set
        {
            _appConfig.VGMStreamPath = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<HKXConversionMethod> HKXConversionMethods { get; } = new(Enum.GetValues<HKXConversionMethod>());

    public HKXConversionMethod HKXConversionMethod
    {
        get => _appConfig.HKXConversionMethod;
        set
        {
            _appConfig.HKXConversionMethod = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HKXCMDPathVisibility));
            OnPropertyChanged(nameof(HKDumpPathVisibility));
            OnPropertyChanged(nameof(Havok2FBXPathVisibility));
        }
    }

    public Visibility HKXCMDPathVisibility => HKXConversionMethod is HKXConversionMethod.HKXCMD_HavokXML or HKXConversionMethod.HKXCMD_GamebryoKF ? Visibility.Visible : Visibility.Collapsed;

    public string HKXCMDPath
    {
        get => _appConfig.HKXCMDPath;
        set
        {
            _appConfig.HKXCMDPath = value;
            OnPropertyChanged();
        }
    }

    public Visibility HKDumpPathVisibility => HKXConversionMethod is HKXConversionMethod.HKDump_WavefrontOBJ ? Visibility.Visible : Visibility.Collapsed;

    public string HKDumpPath
    {
        get => _appConfig.HKDumpPath;
        set
        {
            _appConfig.HKDumpPath = value;
            OnPropertyChanged();
        }
    }

    public Visibility Havok2FBXPathVisibility => HKXConversionMethod is HKXConversionMethod.Havok2FBX ? Visibility.Visible : Visibility.Collapsed;

    public string Havok2FBXPath
    {
        get => _appConfig.Havok2FBXPath;
        set
        {
            _appConfig.Havok2FBXPath = value;
            OnPropertyChanged();
        }
    }

    public ICommand SaveCommand { get; }
    public ICommand DiscardCommand { get; }
    public ICommand ResetCommand { get; }

    public ICommand AboutBNKExtrCommand { get; }
    public ICommand AboutVGMStreamCommand { get; }
    public ICommand AboutHKXCMDCommand { get; }
    public ICommand AboutHKDumpCommand { get; }
    public ICommand AboutHavok2FBXCommand { get; }
}