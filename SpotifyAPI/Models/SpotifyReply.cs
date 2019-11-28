using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyAPI.Models
{
    public class SpotifyReply
    {
        public string SongName { get; set; }
        public string Artist { get; set; }
        public int Volume { get; set; }
        public int Seconds { get; set; }
        public int PlayedSeconds { get; set; }
    }
}
