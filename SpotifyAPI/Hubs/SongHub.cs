using Microsoft.AspNetCore.SignalR;
using Models.Spotify.CurrentlyPlaying;
using Models.Spotify.Playlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyAPI.Hubs
{
    public class SongHub : Hub, ISongHub
    {
        public async Task SongChanged(CurrentlyPlaying song)
        {
            await Clients.All.SendAsync("SongChanged", song);
        }

        public async Task PlaylistChanged(PlaylistItem playlist)
        {
            await Clients.All.SendAsync("PlaylistChanged", playlist);
        }

        public async Task VolumeChanged(int volume)
        {
            await Clients.All.SendAsync("VolumeChanged", volume);
        }

        public async Task Paused()
        {
            await Clients.All.SendAsync("Paused");
        }

        public async Task Resumed()
        {
            await Clients.All.SendAsync("Resumed");
        }
    }
}
