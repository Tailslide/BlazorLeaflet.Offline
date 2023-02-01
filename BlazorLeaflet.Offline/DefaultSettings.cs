//using BlazorLeaflet.Offline.Converters;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;

//namespace BlazorLeaflet.Offline
//{
//    public class DefaultSettings
//    {
//        [JsonConverter(typeof(ColorArrayConverter))]
//        [JsonPropertyName("defaultColorSet")]
//        public Color[] DefaultColorSet { get; set; } = new Color[] { 
//            ColorTranslator.FromHtml("#EF4444"),
//            ColorTranslator.FromHtml("#10B981"),
//            ColorTranslator.FromHtml("#2563EB"),
//            ColorTranslator.FromHtml("#FFFF00"),
//            ColorTranslator.FromHtml("#7C3AED"),
//            ColorTranslator.FromHtml("#F472B6"),
//            ColorTranslator.FromHtml("#000000"),
//            ColorTranslator.FromHtml("#FFFFFF")
//        };

//        [JsonConverter(typeof(ColorConverter))]
//        [JsonPropertyName("defaultColor")]
//        public Color DefaultColor { get; set; } = ColorTranslator.FromHtml("#EF4444");

//        [JsonPropertyName("defaultColorsFollowCurrentColors")]
//        public bool DefaultColorsFollowCurrentColors { get; set; } = false;

//        [JsonConverter(typeof(ColorConverter))]
//        [JsonPropertyName("defaultFillColor")]
//        public Color DefaultFillColor { get; set; } = ColorTranslator.FromHtml("#EF4444");

//        [JsonPropertyName("defaultFontFamilies")]
//        public string[] DefaultFontFamilies { get; set; } = new string[] { "Times, \"Times New Roman\", serif", "Helvetica, Arial, sans-serif", "Courier, \"Courier New\", monospace", "cursive", "fantasy" };

//        [JsonPropertyName("defaultFontFamily")]
//        public string DefaultFontFamily { get; set; } = "Helvetica, Arial, sans-serif";

//        [JsonConverter(typeof(ColorConverter))]
//        [JsonPropertyName("defaultHighlightColor")]
//        public Color DefaultHighlightColor { get; set; } = ColorTranslator.FromHtml("#FFFF00");

//        [JsonPropertyName("defaultHighlightOpacity")]
//        public double DefaultHighlightOpacity = 0.5;

//        [JsonPropertyName("defaultOpacitySteps")]
//        public double[] DefaultOpacitySteps { get; set; } = new double[] { 0.1, 0.25, 0.5, 0.75, 1 };

//        [JsonConverter(typeof(ColorConverter))]
//        [JsonPropertyName("defaultStrokeColor")]
//        public Color DefaultStrokeColor { get; set; } = ColorTranslator.FromHtml("#FFFFFF");

//        [JsonPropertyName("defaultStrokeDasharray")]
//        public string DefaultStrokeDasharray { get; set; } = "";

//        [JsonPropertyName("defaultStrokeDasharrays")]
//        public string[] DefaultStrokeDasharrays { get; set; } = new string[] { "", "3", "12 3", "9 6 3 6" };

//        [JsonPropertyName("defaultStrokeWidth")]
//        public int DefaultStrokeWidth { get; set; } = 3;

//        [JsonPropertyName("defaultStrokeWidths")]
//        public int[] DefaultStrokeWidths { get; set; } = new int[] { 1, 2, 3, 5, 10 };

//        [JsonConverter(typeof(CustomEnumDescriptionConverter<DisplayMode>))]
//        [JsonPropertyName("displayMode")]
//        public DisplayMode DisplayMode { get; set; } = DisplayMode.Inline;


//        [JsonPropertyName("popupMargin")]
//        public int PopupMargin { get; set; } = 30;
//    }
//}
