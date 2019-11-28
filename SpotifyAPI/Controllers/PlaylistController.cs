using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.Spotify.Playlist;
using Models.Spotify.Track;
using SpotifyAPI.Lib;
using SpotifyAPI.Settings;

namespace SpotifyAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly SpotifyAPISettings _spotifyAPISettings;
        private readonly HttpCaller _httpCaller;

        public PlaylistController(IOptions<SpotifyAPISettings> spotifyApiSettings, HttpCaller httpCaller)
        {
            _spotifyAPISettings = spotifyApiSettings.Value;
            _httpCaller = httpCaller;
        }

        /// <summary>
        /// Get a list of all playlists
        /// </summary>
        /// <returns>Object containing information about the playlists and amount</returns>
        [HttpGet]
        public async Task<ActionResult<Playlists>> Get()
        {
            Playlists playlists = await _httpCaller.Get<Playlists>("me/playlists", _spotifyAPISettings.AccessToken);

            return Ok(playlists);
        }

        /// <summary>
        /// Returns all tracks within a Spotify Playlist
        /// </summary>
        /// <param name="playlistId">ID of the playlist to get the tracks from</param>
        /// <returns>List of tracks</returns>
        [HttpGet("{playlistId}/Tracks")]
        public async Task<ActionResult<Tracks>> GetTracks(string playlistId)
        {
            Tracks tracks = await _httpCaller.Get<Tracks>("playlists/" + playlistId + "/tracks?fields=items(track(uri, name, duration_ms, artists))",
                                                                    _spotifyAPISettings.AccessToken);

            return Ok(tracks);
        }
    }
}