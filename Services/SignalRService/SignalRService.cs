using Models.Spotify.CurrentlyPlaying;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Services.Settings;
using Microsoft.AspNetCore.SignalR.Client;
using Models.Spotify.Playlist;

namespace Services.SignalRService
{
    public class SignalRService : ISignalRService
    {
        HubConnection connection;
        private readonly string _hubUrl;

        /// <summary>
        /// Triggeres whenever a song changes
        /// </summary>
        public event Action<CurrentlyPlaying> SongChanged;

        /// <summary>
        /// Triggers whenever a playlist changes
        /// </summary>
        public event Action<PlaylistItem> PlaylistChanged;

        /// <summary>
        /// Triggers whenever the volume has changed
        /// </summary>
        public event Action<int> VolumeChanged;

        /// <summary>
        /// Triggeres whenever the song pauses
        /// </summary>
        public event Action Paused;

        /// <summary>
        /// Triggers whenever the song resumes
        /// </summary>
        public event Action Resumed;


        public SignalRService(HubSettings hubSettings)
        {
            _hubUrl = hubSettings.HubUrl;
        }

        /// <summary>
        /// Connects to SignalR
        /// </summary>
        public async Task ConnectAsync()
        {
            connection = new HubConnectionBuilder()
                .WithUrl(_hubUrl)
                .Build();

            connection.On<CurrentlyPlaying>("SongChanged", (song) => SongChanged?.Invoke(song));
            connection.On<PlaylistItem>("PlaylistChanged", (playlist) => PlaylistChanged?.Invoke(playlist));
            connection.On<int>("VolumeChanged", (volume) => VolumeChanged?.Invoke(volume));
            connection.On("Paused", () => Paused?.Invoke());
            connection.On("Resumed", () => Resumed?.Invoke());

            await connection.StartAsync();
        }

        /// <summary>
        /// Triggers the SongChanged event
        /// </summary>
        /// <param name="song">The song which should be sent to all subscribers</param>
        public async Task SendSongChangedAsync(CurrentlyPlaying song)
        {
            await connection.InvokeAsync("SongChanged", song);
        }

        /// <summary>
        /// Triggers the PlaylistChanged event
        /// </summary>
        /// <param name="playlist">The playlist which should be sent to all subscribers</param>
        public async Task SendPlaylistChangedAsync(PlaylistItem playlist)
        {
            await connection.InvokeAsync("PlaylistChanged", playlist);
        }

        /// <summary>
        /// Triggers the VolumeChanged event
        /// </summary>
        /// <param name="volume">The volume which should be sent to all subscribers</param>
        public async Task SendVolumeChangedAsync(int volume)
        {
            await connection.InvokeAsync("VolumeChanged", volume);
        }

        /// <summary>
        /// Triggers the Paused event
        /// </summary>
        public async Task SendPausedAsync()
        {
            await connection.InvokeAsync("Paused");
        }

        /// <summary>
        /// Triggers the Resumed event
        /// </summary>
        public async Task SendResumedAsync()
        {
            await connection.InvokeAsync("Resumed");
        }
    }
}
