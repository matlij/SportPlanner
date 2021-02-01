using System;
using System.Globalization;
using Xamarin.Forms;

namespace SportPlanner.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value
                ? Color.FromHex("#E5FFE0")
                : Color.FromHex("#FFE0E0");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
