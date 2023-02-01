using BlazorLeaflet.Offline.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorLeaflet.Offline
{
    public class Settings
    {
        [JsonConverter(typeof(EnumArrayConverter<LeafletType>))]
        [JsonPropertyName("availableLeafletTypes")]
        public LeafletType[] AvailableLeafletTypes { get; set; } = new LeafletType[] {
            LeafletType.First, 
            LeafletType.Second, 
        };

        [JsonPropertyName("minZoom")]
        public int MinZoom { get; set; } = 12;

        [JsonPropertyName("defaultZoom")]
        public int DefaultZoom { get; set; } = 12;

        [JsonPropertyName("subdomains")]
        public string Subdomains { get; set; } = "abc";

        [JsonPropertyName("attribution")]
        public string Attribution { get; set; } = "Map data {attribution.OpenStreetMap}";

        [JsonPropertyName("defaultLat")]
        public float? DefaultLat { get; set; } = 51.10F;

        [JsonPropertyName("defaultLong")]
        public float? DefaultLong { get; set; } = -114.07F;

        [JsonPropertyName("saveOnLoad")]
        public bool SaveOnLoad { get; set; } = true;

        [JsonPropertyName("downsample")]
        public bool Downsample { get; set; } = true;

        [JsonPropertyName("layerSwitcherVisible")]
        public bool LayerSwitcherVisible { get; set; } = true;

        [JsonPropertyName("offlineButtonsVisible")]
        public bool OfflineButtonsVisible { get; set; } = true;

        [JsonPropertyName("offlineLayerAvailable")]
        public bool OfflineLayerAvailable { get; set; } = true;

        /// <summary>
        /// Fire our own context menu event and hide system one. make sure to add css for disabling ios context long press:
        /// .leaflet-container { -webkit-user-select: none; -webkit-touch-callout: none; }
        /// </summary>
        [JsonPropertyName("replaceContextMenu")]        
        public bool ReplaceContextMenu { get; set; } = false;

        //[JsonPropertyName("defaultSettings")]
        //public DefaultSettings DefaultSettings { get; set; } = new DefaultSettings();
    }
}
