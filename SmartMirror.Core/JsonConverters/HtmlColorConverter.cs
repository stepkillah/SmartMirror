using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartMirror.Core.JsonConverters
{
    public class HtmlColorConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return string.IsNullOrEmpty(value)
                ? default
                : ColorTranslator.FromHtml(value);
        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options) => writer.WriteStringValue(ColorTranslator.ToHtml(value));

    }
}
