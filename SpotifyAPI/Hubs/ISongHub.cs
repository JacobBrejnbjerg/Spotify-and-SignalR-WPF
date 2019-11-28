using Models.Spotify.CurrentlyPlaying;
using Models.Spotify.Playlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyAPI.Hubs
{
    public interface ISongHub
    {
        Task SongChanged(CurrentlyPlaying song);
        Task PlaylistChanged(PlaylistItem playlist);
        Task VolumeChanged(int volume);
        Task Paused();
        Task Resumed();
    }
}
