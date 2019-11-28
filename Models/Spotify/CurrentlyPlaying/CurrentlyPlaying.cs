using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Spotify.CurrentlyPlaying
{
    public class CurrentlyPlaying
    {
        public Device device { get; set; }
        public bool shuffle_state { get; set; }
        public string repeat_state { get; set; }
        public long timestamp { get; set; }
        public Context context { get; set; }
        public int progress_ms { get; set; }
        public Item item { get; set; }
        public string currently_playing_type { get; set; }
        public Actions actions { get; set; }
        public bool is_playing { get; set; }
    }
}