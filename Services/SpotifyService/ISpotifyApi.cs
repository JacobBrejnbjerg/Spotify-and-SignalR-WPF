using Models.Spotify.CurrentlyPlaying;
using Models.Spotify.Playlist;
using Models.Spotify.Track;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.SpotifiyService
{
    public interface ISpotifyApi
    {
        /// <summary>
        /// Get the currently playing Spotify song
        /// </summary>
        /// <returns>CurrentlyPlaying object containing information about the song</returns>
        Task<CurrentlyPlaying> GetCurrentlyPlayingSongAsync();

        /// <summary>
        /// Resumes the current song
        /// </summary>
        Task ResumeSongAsync();

        /// <summary>
        /// Pauses the current song
        /// </summary>
        Task PauseSongAsync();

        /// <summary>
        /// Skips to the next song
        /// </summary>
        /// <returns>CurrentlyPlaying object contaning the new song</returns>
        Task<CurrentlyPlaying> NextSongAsync();

        /// <summary>
        /// Goes to the previous song
        /// </summary>
        /// <returns>CurrentlyPlaying object which contains the new song</returns>
        Task<CurrentlyPlaying> PreviousSongAsync();

        /// <summary>
        /// Sets the volume of Spotify
        /// </summary>
        /// <param name="volume">Volume between 0 and 100</param>
        Task SetVolumeAsync(int volume);

        /// <summary>
        /// Get all the users playlists
        /// </summary>
        /// <returns>Playlists object which contains a list of all the playlists</returns>
        Task<Playlists> GetPlaylistsAsync();

        /// <summary>
        /// Get all the tracks in a playlist
        /// </summary>
        /// <param name="playlistId">The id of the playlist</param>
        /// <returns>Tracks object containing a list of all the tracks in the playlist</returns>
        Task<Tracks> GetTracksAsync(string playlistId);

        /// <summary>
        /// Plays a specific song
        /// </summary>
        /// <param name="contextUri">Uri of Album og Playlist</param>
        /// <param name="offset">The index of the song in the Album or Playlist</param>
        /// <returns>CurrentlyPlaying object containing the new song</returns>
        Task<CurrentlyPlaying> PlaySongAsync(string contextUri, int offset);
    }
}
