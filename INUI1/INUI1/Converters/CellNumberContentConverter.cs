using System;
using System.Globalization;
using System.Windows.Data;

namespace INUI1.Converters
{
    class CellNumberContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int && (int) value == 0)
            {
                return "";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
