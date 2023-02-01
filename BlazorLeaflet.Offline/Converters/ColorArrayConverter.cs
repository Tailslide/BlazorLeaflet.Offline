using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorLeaflet.Offline.Converters
{
    public class ColorArrayConverter : JsonConverter<Color[]>
    {
        public override Color[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Color[] value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();   
            foreach (var v in value)
            {
                writer.WriteStringValue(ColorTranslator.ToHtml(v));
            }
            writer.WriteEndArray();
        }
    }
}
