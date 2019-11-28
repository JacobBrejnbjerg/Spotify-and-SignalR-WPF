using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using SpotifyAPI.Hubs;
using SpotifyAPI.Lib;
using SpotifyAPI.Models;
using SpotifyAPI.Settings;

namespace SpotifyAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class VolumeController : ControllerBase
    {
        private readonly SpotifyAPISettings _spotifyAPISettings;
        private readonly HttpCaller _httpCaller;

        public VolumeController(IOptions<SpotifyAPISettings> spotifyApiSettings, HttpCaller httpCaller)
        {
            _spotifyAPISettings = spotifyApiSettings.Value;
            _httpCaller = httpCaller;
        }

        /// <summary>
        /// Sets the volume of Spotify
        /// </summary>
        /// <param name="volume">Volume between 0 and 100%</param>
        /// <returns>HTTP Statuscode of whether it went well or not</returns>
        [HttpPut("{volume}")]
        public async Task<IActionResult> Put(int volume)
        {
            HttpStatusCode status = await _httpCaller.Put("me/player/volume" +
                                                            "?volume_percent=" + volume +
                                                            "&device_id=" + _spotifyAPISettings.DeviceID,
                                                        _spotifyAPISettings.AccessToken,
                                                        null);
            return StatusCode((int)status);
        }
    }
}