﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Esfa.Vacancy.Api.Core
{
    /// <summary>
    /// from http://stackoverflow.com/a/21298811/1882637
    /// </summary>
    public class StrictEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);

            try
            {
                // We're only interested in integers or strings;
                // all other token types should fall through
                if (token.Type == JTokenType.Integer || token.Type == JTokenType.String)
                {
                    // Get the string representation of the token
                    // and check if it is numeric
                    var s = token.ToString();
                    int i;
                    if (int.TryParse(s, out i))
                    {
                        // If the token is numeric, try to find a matching
                        // name from the enum. If it works, convert it into
                        // the actual enum value; otherwise punt.
                        var name = Enum.GetName(objectType, i);
                        if (name != null)
                            return Enum.Parse(objectType, name);
                    }
                    else
                    {
                        // We've got a non-numeric value, so try to parse it
                        // as is (case insensitive). If this doesn't work,
                        // it will throw an ArgumentException.
                        return Enum.Parse(objectType, s, true);
                    }
                }
            }
            catch (ArgumentException)
            {
                // Eat the exception and fall through
            }

            // We got a bad value, so return the first value from the enum as
            // a default. Alternatively, you could throw an exception here to
            // halt the deserialization.
            var en = Enum.GetValues(objectType).GetEnumerator();
            en.MoveNext();

            return en.Current;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
