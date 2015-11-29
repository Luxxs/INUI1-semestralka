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
            int parse = 0;
            if(value is string)
            {
                if((value as string).Equals("") || !int.TryParse(value as string, out parse))
                {
                    return 0;
                }
            }
            return parse;
        }
    }
}
