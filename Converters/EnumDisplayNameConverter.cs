using System.Globalization;
using System.Reflection;

namespace MorcuTool.Converters;

public sealed class EnumDisplayNameConverter : DependencyObject, IValueConverter
{
    readonly Dictionary<object, string> _enumDisplayNames = new();

    public static readonly DependencyProperty NameMethodProperty = DependencyProperty.Register(
        nameof(NameMethod),
        typeof(NameMethod),
        typeof(EnumDisplayNameConverter),
        new PropertyMetadata(NameMethod.Auto)
    );

    public NameMethod NameMethod
    {
        get => (NameMethod)GetValue(NameMethodProperty);
        set
        {
            SetValue(NameMethodProperty, value);
            _enumDisplayNames.Clear();
        }
    }

    string GetDisplayName(MemberInfo memberInfo)
    {
        switch (NameMethod)
        {
            case NameMethod.Auto:
            {
                var displayAttribute = memberInfo.GetCustomAttribute<DisplayNameAttribute>();
                if (displayAttribute != null && !string.IsNullOrEmpty(displayAttribute.DisplayName))
                    return displayAttribute.DisplayName;
                var descriptionAttribute = memberInfo.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttribute != null && !string.IsNullOrEmpty(descriptionAttribute.Description))
                    return descriptionAttribute.Description;
                break;
            }
            case NameMethod.DisplayName:
            {
                var displayAttribute = memberInfo.GetCustomAttribute<DisplayNameAttribute>();
                if (displayAttribute != null && !string.IsNullOrEmpty(displayAttribute.DisplayName))
                    return displayAttribute.DisplayName;
                break;
            }
            case NameMethod.Description:
            {
                var descriptionAttribute = memberInfo.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttribute != null && !string.IsNullOrEmpty(descriptionAttribute.Description))
                    return descriptionAttribute.Description;
                break;
            }
        }
        return memberInfo.Name;
    }

    string GetDisplayName(object value)
    {
        if (_enumDisplayNames.TryGetValue(value, out var displayName))
            return displayName;

        switch (value)
        {
            case ISmartEnum smartEnum:
            {
                Type smartEnumType = smartEnum.GetType();
                if (smartEnumType.TryGetValues(out IEnumerable<object> enums))
                {
                    string returnName = string.Empty;
                    foreach (var enumValue in enums)
                    {
                        var enumName = enumValue.ToString()!;
                        MemberInfo? enumField = enumValue.GetType().GetMember(enumName).FirstOrDefault();
                        if (enumField is not null)
                            enumName = GetDisplayName(enumField);
                        _enumDisplayNames[enumValue] = enumName;
                        if (enumValue.Equals(value))
                            returnName = enumName;
                    }
                    if (!string.IsNullOrEmpty(returnName))
                        return returnName;
                }
                break;
            }
            default:
            {

                Type enumType = value.GetType();
                string enumName = Enum.GetName(enumType, value) ?? value.ToString()!;
                FieldInfo? enumField = enumType.GetField(enumName);
                if (enumField is not null)
                    enumName = GetDisplayName(enumField);
                _enumDisplayNames[value] = enumName;
                return enumName;
            }
        }

        return value.ToString()!;
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
            return DependencyProperty.UnsetValue;
        return GetDisplayName(value);
    }

    object IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[AttributeUsage(AttributeTargets.All)]
public sealed class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; }

    public DisplayNameAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}

public enum NameMethod
{
    Auto,
    DisplayName,
    Description,
}