using Models;
using Models.Spotify.CurrentlyPlaying;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SpotifyWPF.Converters
{
    public class ProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Progress progress = value as Progress;

            if (progress != null && progress.Progress_Ms != 0 && progress.Duration_Ms != 0)
                return ((double)progress.Progress_Ms / (double)progress.Duration_Ms) * 100;
            else
                return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
