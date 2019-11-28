using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Spotify.Playlist
{
    public class Playlists
    {
        public string href { get; set; }
        public List<PlaylistItem> items { get; set; }
        public int limit { get; set; }
        public object next { get; set; }
        public int offset { get; set; }
        public string previous { get; set; }
        public int total { get; set; }
    }
}
