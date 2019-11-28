using Models.Spotify.CurrentlyPlaying;
using Models.Spotify.Playlist;
using Models.Spotify.Track;
using SpotifyAPI.Lib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.SpotifiyService
{
    public class SpotifyApi : ISpotifyApi
    {
        private readonly HttpCaller _httpCaller;

        public SpotifyApi(HttpCaller httpCaller)
        {
            _httpCaller = httpCaller;
        }

        /// <summary>
        /// Get the currently playing Spotify song
        /// </summary>
        /// <returns>CurrentlyPlaying object containing information about the song</returns>
        public async Task<CurrentlyPlaying> GetCurrentlyPlayingSongAsync()
        {
            return await _httpCaller.Get<CurrentlyPlaying>("song");
        }

        /// <summary>
        /// Resumes the current song
        /// </summary>
        public async Task ResumeSongAsync()
        {
            await _httpCaller.Put("song/play");
        }

        /// <summary>
        /// Pauses the current song
        /// </summary>
        public async Task PauseSongAsync()
        {
            await _httpCaller.Put("song/pause");
        }

        /// <summary>
        /// Skips to the next song
        /// </summary>
        /// <returns>CurrentlyPlaying object contaning the new song</returns>
        public async Task<CurrentlyPlaying> NextSongAsync()
        {
            return await _httpCaller.Post<CurrentlyPlaying>("song/next");
        }

        /// <summary>
        /// Goes to the previous song
        /// </summary>
        /// <returns>CurrentlyPlaying object which contains the new song</returns>
        public async Task<CurrentlyPlaying> PreviousSongAsync()
        {
            return await _httpCaller.Post<CurrentlyPlaying>("song/previous");
        }

        /// <summary>
        /// Sets the volume of Spotify
        /// </summary>
        /// <param name="volume">Volume between 0 and 100</param>
        public async Task SetVolumeAsync(int volume)
        {
            await _httpCaller.Put("volume/" + volume);
        }

        /// <summary>
        /// Get all the users playlists
        /// </summary>
        /// <returns>Playlists object which contains a list of all the playlists</returns>
        public async Task<Playlists> GetPlaylistsAsync()
        {
            return await _httpCaller.Get<Playlists>("playlist");
        }

        /// <summary>
        /// Get all the tracks in a playlist
        /// </summary>
        /// <param name="playlistId">The id of the playlist</param>
        /// <returns>Tracks object containing a list of all the tracks in the playlist</returns>
        public async Task<Tracks> GetTracksAsync(string playlistId)
        {
            return await _httpCaller.Get<Tracks>("playlist/" + playlistId + "/tracks");
        }

        /// <summary>
        /// Plays a specific song
        /// </summary>
        /// <param name="contextUri">Uri of Album og Playlist</param>
        /// <param name="offset">The index of the song in the Album or Playlist</param>
        /// <returns>CurrentlyPlaying object containing the new song</returns>
        public async Task<CurrentlyPlaying> PlaySongAsync(string contextUri, int offset)
        {
            return await _httpCaller.Put<CurrentlyPlaying>($"song/play/{contextUri}/{offset}");
        }
    }
}
