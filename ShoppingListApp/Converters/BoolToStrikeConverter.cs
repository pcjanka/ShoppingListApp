using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ShoppingListApp.Converters
{
    public class BoolToStrikeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && b) return TextDecorations.Strikethrough;
            return TextDecorations.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
