using System.Globalization;

namespace ChurchMS.MAUI.Converters;

/// <summary>Returns true if the value is not null (for IsVisible bindings).</summary>
public class NotNullConverter : IValueConverter
{
    public static readonly NotNullConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is not null && value is not string s || (value is string str && !string.IsNullOrWhiteSpace(str));

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
