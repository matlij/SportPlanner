using SportPlanner.Models;
using SportPlanner.Models.Constants;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace SportPlanner.Converters
{
    public class UserReplyToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var userReplyInt = (int)value;
            var userReply = (EventReply)userReplyInt;

            switch (userReply)
            {
                case EventReply.Attending:
                    return IconConstants.OkSymbol;

                case EventReply.NotAttending:
                    return IconConstants.NotOkSymbol;

                default:
                case EventReply.NotReplied:
                    return IconConstants.NoReplySymbol;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
