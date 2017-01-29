using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace ScannerReader.Converters
{
    public class MessageToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}