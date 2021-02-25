using System;
using System.Globalization;
using Xamarin.Forms;

namespace SportPlanner.Converters
{
    public class BoolToRedOrGreenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value
                ? Color.Red
                : Color.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
