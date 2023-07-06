﻿
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyClassLibrary.Methods
{
    public class NoQuotesStringConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteRawValue(value);
        }


    }
}
