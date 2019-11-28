using Models.Spotify.CurrentlyPlaying;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SpotifyWPF.Converters
{
    public class ArtistsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<Artist> artists = value as List<Artist>;
            return String.Join(", ", artists.Select(artist => artist.name));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
