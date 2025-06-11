using System.Globalization;

namespace ProductApp.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Colors.Green : Colors.Red;
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("BoolToColorConverter nie obsługuje konwersji zwrotnej");
        }
    }
    public class BoolToTextConverter : IValueConverter
    {
        public string TrueText { get; set; } = "Tak";
        public string FalseText { get; set; } = "Nie";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? TrueText : FalseText;
            }
            return FalseText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("BoolToTextConverter nie obsługuje konwersji zwrotnej");
        }
    }
    public class PriceFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal price)
            {
                return price.ToString("C", culture);
            }
            return "0,00 zł";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("PriceFormatConverter nie obsługuje konwersji zwrotnej");
        }
    }
}