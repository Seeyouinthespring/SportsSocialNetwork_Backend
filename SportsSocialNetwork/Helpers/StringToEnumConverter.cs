using Newtonsoft.Json;
using System;

namespace SportsSocialNetwork.Helpers
{
    public class StringToEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var type = Nullable.GetUnderlyingType(objectType);
            if (type != null) objectType = type;
            if (reader.Value == null && type != null) return reader.Value;
            if (reader.ValueType != typeof(string))
                throw new JsonSerializationException();
            var value = (string)reader.Value;
            object result = null;
            if (Enum.TryParse(objectType, value, true, out result) && Enum.IsDefined(objectType, result))
                return result;
            throw new JsonSerializationException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string _value = value.ToString();
            _value = Char.ToLowerInvariant(_value[0]) + _value.Substring(1);
            writer.WriteValue(_value);
        }
    }
}
