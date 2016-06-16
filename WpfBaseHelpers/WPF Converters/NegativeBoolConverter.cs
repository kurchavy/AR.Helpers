using System;
using System.Globalization;
using System.Windows.Data;

namespace AR.WPF.Helpers
{
    /// <summary>
    /// Converts bool to negative value
    /// </summary>
    public class NegativeBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool res = (bool)value;
            return !res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
