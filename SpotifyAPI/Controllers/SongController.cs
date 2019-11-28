using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Models.Spotify.CurrentlyPlaying;
using Newtonsoft.Json;
using SpotifyAPI.Lib;
using SpotifyAPI.Models;
using SpotifyAPI.Settings;

namespace SpotifyAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly SpotifyAPISettings _spotifyAPISettings;
        private readonly HttpCaller _httpCaller;

        public SongController(IOptions<SpotifyAPISettings> spotifyApiSettings, HttpCaller httpCaller)
        {
            _spotifyAPISettings = spotifyApiSettings.Value;
            _httpCaller = httpCaller;
        }


        /// <summary>
        /// Gets data from the Spotify API
        /// </summary>
        /// <returns>SpotifyReply object of current playing song</returns>
        [HttpGet]
        public async Task<ActionResult<CurrentlyPlaying>> Get()
        {
            CurrentlyPlaying currentlyPlaying = await _httpCaller.Get<CurrentlyPlaying>("me/player", _spotifyAPISettings.AccessToken);

            return Ok(currentlyPlaying);
        }


        /// <summary>
        /// Skips to next song in Spotify API
        /// </summary>
        /// <returns>SpotifyReply object with data of new song</returns>
        [HttpPost("Next")]
        public async Task<ActionResult<CurrentlyPlaying>> Next()
        {
            HttpStatusCode status = await _httpCaller.Post("me/player/next?device_id=" + _spotifyAPISettings.DeviceID,
                                                        _spotifyAPISettings.AccessToken);

            CurrentlyPlaying currentlyPlaying = await _httpCaller.Get<CurrentlyPlaying>("me/player", _spotifyAPISettings.AccessToken);

            return Ok(currentlyPlaying);
        }


        /// <summary>
        /// Goes to previous song in Spotify API
        /// </summary>
        /// <returns>SpotifyReply object with data of new song</returns>
        [HttpPost("Previous")]
        public async Task<ActionResult<CurrentlyPlaying>> Previous()
        {
            HttpStatusCode status = await _httpCaller.Post("me/player/previous?device_id=" + _spotifyAPISettings.DeviceID,
                                                            _spotifyAPISettings.AccessToken);

            CurrentlyPlaying currentlyPlaying = await _httpCaller.Get<CurrentlyPlaying>("me/player", _spotifyAPISettings.AccessToken);

            return Ok(currentlyPlaying);
        }

        /// <summary>
        /// Pauses currently playing song
        /// </summary>
        /// <returns>HTTP Statuscode of whether it went well or not</returns>
        [HttpPut("Pause")]
        public async Task<IActionResult> Pause()
        {
            HttpStatusCode status = await _httpCaller.Put("me/player/pause?device_id=" + _spotifyAPISettings.DeviceID,
                                                        _spotifyAPISettings.AccessToken);

            return StatusCode((int)status);
        }

        /// <summary>
        /// Resumes the paused song song
        /// </summary>
        /// <returns>HTTP Statuscode of whether it went well or not</returns>
        [HttpPut("Play")]
        public async Task<IActionResult> Play()
        {
            HttpStatusCode status = await _httpCaller.Put("me/player/play?device_id=" + _spotifyAPISettings.DeviceID,
                                                        _spotifyAPISettings.AccessToken);

            return StatusCode((int)status);
        }


        /// <summary>
        /// Plays song with given track URI
        /// </summary>
        /// <param name="contextUri">URI of the album or playlist from which the song is in</param>
        /// <param name="offset">The index of the song in the playlist</param>
        /// <returns>HTTP Statuscode of whether it went well or not</returns>
        [HttpPut("Play/{contextUri}/{offset}")]
        public async Task<ActionResult<CurrentlyPlaying>> Play(string contextUri, int offset)
        {
            var playContent = new
            {
                context_uri = contextUri,
                offset = new
                {
                    position = offset
                }
            };

            // Serialize anonymous object into HttpContent
            HttpContent content = new StringContent(JsonConvert.SerializeObject(playContent));

            HttpStatusCode status = await _httpCaller.Put("me/player/play?device_id=" + _spotifyAPISettings.DeviceID,
                                                        _spotifyAPISettings.AccessToken,
                                                        content);

            CurrentlyPlaying currentlyPlaying = await _httpCaller.Get<CurrentlyPlaying>("me/player/currently-playing", _spotifyAPISettings.AccessToken);

            return Ok(currentlyPlaying);
        }
    }
}