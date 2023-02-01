using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorLeaflet.Offline
{
    public class Marker
    {
        [JsonPropertyName("lat")]
        public float Lat { get; set; }

        [JsonPropertyName("long")]
        public float Long { get; set; }

        [JsonPropertyName("popupHtml")]
        public string PopupHtml { get; set; }

    }
}
