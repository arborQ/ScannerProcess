using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace AdminPanel.Converters
{
    public class PasswordBoxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string[] result = Array.ConvertAll<object, string>(values, obj =>
            {
                return (obj == null) ? string.Empty : obj.ToString();
            });
            // RoomID + IsNeedPassword + PasswordText
            return result[0] + '\n' + result[1] + '\n' + result[2];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
