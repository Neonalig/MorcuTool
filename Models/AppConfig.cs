namespace MorcuTool.Models;

public class AppConfig : ApplicationSettingsBase
{
    public const int MaxRecentFiles = 10;

    readonly Dictionary<string, object> _defaultValues = new()
    {
        { nameof(RecentFiles), () => new ObservableCollection<string>() },
        { nameof(BNKConversionMethod), BNKConversionMethod.RawBinary },
        { nameof(BNKExtrPath), string.Empty },
        { nameof(VGMStreamPath), string.Empty },
        { nameof(HKXConversionMethod), HKXConversionMethod.RawBinary },
        { nameof(HKXCMDPath), string.Empty },
        { nameof(HKDumpPath), string.Empty },
        { nameof(Havok2FBXPath), string.Empty },
    };

    readonly Dictionary<string, object> _loadedValues = new();
    bool didPopulateLoaded;

    [SuppressMessage("ReSharper", "NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract")]
    public AppConfig()
    {
        foreach ((string key, object value) in _defaultValues)
        {
            this[key] ??= value is Func<object> func ? func() : value;
        }
    }

    public new void Initialize(SettingsContext context, SettingsPropertyCollection properties, SettingsProviderCollection providers)
    {
        base.Initialize(context, properties, providers);
        Reload();
    }

    public new void Reload()
    {
        base.Reload();
        didPopulateLoaded = true;
        foreach (SettingsProperty property in Properties)
        {
            _loadedValues[property.Name] = this[property.Name];
        }
    }

    public new void Save()
    {
        base.Save();
        Reload();
    }

    public bool IsDirty
    {
        get
        {
            Dictionary<string, object> values = didPopulateLoaded ? _loadedValues : _defaultValues;
            foreach ((string key, object value) in values)
            {
                object currentValue = this[key];
                object savedValue = value is Func<object> func ? func() : value;
                if (!Equals(currentValue, savedValue))
                {
                    Console.WriteLine($"Key: {key}, Value: {currentValue}, Last Saved: {savedValue}");
                    return true;
                }
            }

            return false;
        }
    }

    [UserScopedSetting]
    public ObservableCollection<string> RecentFiles
    {
        get => (ObservableCollection<string>)this[nameof(RecentFiles)];
        set => this[nameof(RecentFiles)] = value;
    }

    [UserScopedSetting]
    public BNKConversionMethod BNKConversionMethod
    {
        get => (BNKConversionMethod)this[nameof(BNKConversionMethod)];
        set => this[nameof(BNKConversionMethod)] = value;
    }

    [UserScopedSetting]
    public string BNKExtrPath
    {
        get => (string)this[nameof(BNKExtrPath)];
        set => this[nameof(BNKExtrPath)] = value;
    }

    [UserScopedSetting]
    public string VGMStreamPath
    {
        get => (string)this[nameof(VGMStreamPath)];
        set => this[nameof(VGMStreamPath)] = value;
    }

    [UserScopedSetting]
    public HKXConversionMethod HKXConversionMethod
    {
        get => (HKXConversionMethod)this[nameof(HKXConversionMethod)];
        set => this[nameof(HKXConversionMethod)] = value;
    }

    [UserScopedSetting]
    public string HKXCMDPath
    {
        get => (string)this[nameof(HKXCMDPath)];
        set => this[nameof(HKXCMDPath)] = value;
    }

    [UserScopedSetting]
    public string HKDumpPath
    {
        get => (string)this[nameof(HKDumpPath)];
        set => this[nameof(HKDumpPath)] = value;
    }

    [UserScopedSetting]
    public string Havok2FBXPath
    {
        get => (string)this[nameof(Havok2FBXPath)];
        set => this[nameof(Havok2FBXPath)] = value;
    }
}

public enum BNKConversionMethod
{
    [DisplayName("Raw (.bnk)")]
    [Description("Raw output of Wwise SoundBank files.")]
    RawBinary,
    [DisplayName("bnkextr for Wwise .wem")]
    [Description("Wwise Encoded Media (WEM) output of Wwise SoundBank files.\nOutputs .wem format, Uses bnkextr.")]
    BNKExtr_WwiseWEM,
    [DisplayName("VGMStream for .wav")]
    [Description("Waveform Audio File Format (WAV) output of Wwise SoundBank files.\nOutputs .wav format, Uses vgmstream.")]
    VGMStream_WAV,
}

public enum HKXConversionMethod
{
    [DisplayName("Raw (.hkx)")]
    [Description("Raw output of Havok binary files.")]
    RawBinary,
    [DisplayName("hkxcmd for Havok .xml")]
    [Description("XML output of Havok files.\nOutputs .xml format, Uses hkxcmd.")]
    HKXCMD_HavokXML,
    [DisplayName("hkxcmd for Gamebryo .kf")]
    [Description("Gamebryo KF output of Havok files.\nOutputs .kf format, Uses hkxcmd.")]
    HKXCMD_GamebryoKF,
    [DisplayName("hkdump for Wavefront .obj")]
    [Description("Wavefront OBJ output of Havok files.\nOutputs .obj format, Uses hkdump.")]
    HKDump_WavefrontOBJ,
    [DisplayName("Havok2FBX for Autodesk .fbx")]
    [Description("Autodesk FBX output of Havok files.\nOutputs .fbx format, Uses Havok2FBX.")]
    Havok2FBX,
}