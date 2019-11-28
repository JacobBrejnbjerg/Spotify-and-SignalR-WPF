using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Settings
{
    public class HubSettings
    {
        public string HubUrl { get; set; }
        public string HubName { get; set; }

        public HubSettings(string hubUrl, string hubName)
        {
            HubUrl = hubUrl;
            HubName = hubName;
        }
    }
}
