using System;
using System.Security;
using Newtonsoft.Json;

namespace PayOn.Infrastructure.JsonConverters
{
    public class ParseSecureStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteRawValue(((SecureString) value).ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader == null)
                return null;

            SecureString secureString = new SecureString();

            foreach(char c in reader.Value.ToString())
                secureString.AppendChar(c);

            return secureString;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SecureString);
        }
    }
}
