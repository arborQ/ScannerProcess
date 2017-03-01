using System;
using System.Globalization;
using System.Windows.Data;
using WorkflowService;

namespace ScannerReader.Converters
{
    public class MessageTypeToSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((MessageType)value)
            {
                case MessageType.Warning:
                    return 100;
                case MessageType.Error:
                    return 200;
                default:
                    return 35;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MessageTypeToFontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((MessageType)value)
            {
                case MessageType.Warning:
                    return 40;
                case MessageType.Error:
                    return 50;
                default:
                    return 25;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
