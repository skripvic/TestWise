using System;
using System.Globalization;
using System.Windows.Data;

namespace cadwiseTest.ViewModel;

public class ParametersConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not FileInformation fileInfo) return string.Empty;
        var parts = new System.Collections.Generic.List<string>();

        if (fileInfo.RemovePunctuation)
        {
            parts.Add("Удалить пунктуацию");
        }

        if (fileInfo.MinWordLength > 1)
        {
            parts.Add($"Удалить слова короче {fileInfo.MinWordLength}");
        }

        return string.Join("\n", parts);

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}