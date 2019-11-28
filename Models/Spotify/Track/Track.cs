using Models.Spotify.CurrentlyPlaying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Spotify.Track
{
    public class Track
    {
        public List<Artist> artists { get; set; }
        public string uri { get; set; }
        public string name { get; set; }
        public int duration_ms { get; set; }
    }
}
