using System;
using System.Globalization;
using System.Windows.Data;

namespace INUI1.Converters
{
    class IntToStringArrayContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int[,])
            {
                int[,] val = (int[,])value;
                string[,] retVal = new string[val.GetLength(0), val.GetLength(1)];
                for (int i = 0; i < val.GetLength(0); i++)
                {
                    for (int j = 0; j < val.GetLength(1); j++)
                    {
                        if (val[i, j] == 0)
                            retVal[i, j] = "";
                        else
                            retVal[i, j] = val[i, j].ToString();
                    }
                }
                return retVal;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
