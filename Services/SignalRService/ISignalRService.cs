using Models.Spotify.CurrentlyPlaying;
using Models.Spotify.Playlist;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.SignalRService
{
    public interface ISignalRService
    {
        /// <summary>
        /// Triggeres whenever a song changes
        /// </summary>
        event Action<CurrentlyPlaying> SongChanged;

        /// <summary>
        /// Triggers whenever a playlist changes
        /// </summary>
        event Action<PlaylistItem> PlaylistChanged;

        /// <summary>
        /// Triggers whenever the volume has changed
        /// </summary>
        event Action<int> VolumeChanged;

        /// <summary>
        /// Triggeres whenever the song pauses
        /// </summary>
        public event Action Paused;

        /// <summary>
        /// Triggers whenever the song resumes
        /// </summary>
        public event Action Resumed;

        /// <summary>
        /// Connects to SignalR
        /// </summary>
        Task ConnectAsync();

        /// <summary>
        /// Triggers the SongChanged event
        /// </summary>
        /// <param name="song">The song which should be sent to all subscribers</param>
        Task SendSongChangedAsync(CurrentlyPlaying song);

        /// <summary>
        /// Triggers the PlaylistChanged event
        /// </summary>
        /// <param name="playlist">The playlist which should be sent to all subscribers</param>
        Task SendPlaylistChangedAsync(PlaylistItem playlist);

        /// <summary>
        /// Triggers the VolumeChanged event
        /// </summary>
        /// <param name="volume">The volume which should be sent to all subscribers</param>
        Task SendVolumeChangedAsync(int volume);

        /// <summary>
        /// Triggers the Paused event
        /// </summary>
        Task SendPausedAsync();

        /// <summary>
        /// Triggers the Resumed event
        /// </summary>
        Task SendResumedAsync();
    }
}
