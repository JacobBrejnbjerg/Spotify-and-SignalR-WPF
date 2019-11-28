using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace SpotifyWPF.Converters
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Value is in miliseconds
            TimeSpan ts = TimeSpan.FromMilliseconds((int)value);

            // Formats the string using two digits minuts and seconds
            return String.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Parses string to datetime object and gets milliseconds
            return DateTime.ParseExact(value.ToString(), "hh:mm:ss", null).Millisecond;
        }
    }
}
