﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Spotify.CurrentlyPlaying
{
    public class Context
    {
        public ExternalUrls external_urls { get; set; }
        public string href { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
}
