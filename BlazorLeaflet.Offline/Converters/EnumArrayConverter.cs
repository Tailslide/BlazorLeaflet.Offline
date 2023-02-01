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
    public class EnumArrayConverter<T> : JsonConverter<T[]> where T : Enum
    {
        public override T[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, T[] value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var v in value)
            {
                writer.WriteStringValue(v.ToString());
            }
            writer.WriteEndArray();

        }
    }
}
