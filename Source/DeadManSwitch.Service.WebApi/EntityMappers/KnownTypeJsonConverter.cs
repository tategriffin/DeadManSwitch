using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeadManSwitch.Service.WebApi
{
    /// <summary>
    /// Use KnownType Attribute to match a derived class based on the class given to the serilaizer
    /// Selected class will be the first class to match all properties in the json object.
    /// </summary>
    internal class KnownTypeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            Type t = typeof (List<>);
            bool b1 = objectType.IsGenericType && (objectType.GetGenericTypeDefinition() == t);

            return System.Attribute.GetCustomAttributes(objectType).Any(v => v is KnownTypeAttribute);
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject originalJsonObject = JObject.Load(reader);

            KnownTypeAttribute[] allKnownTypeAttributes = (KnownTypeAttribute[]) System.Attribute.GetCustomAttributes(objectType).OfType<KnownTypeAttribute>();

            // check known types for a match. 
            foreach (var knownTypeAttribute in allKnownTypeAttributes)
            {
                object target = Activator.CreateInstance(knownTypeAttribute.Type);
                var knownTypeCandidate = SerializeToKnownType(serializer, target);

                if (IsKnownTypeMatch(originalJsonObject, knownTypeCandidate))
                {
                    serializer.Populate(originalJsonObject.CreateReader(), target);
                    return target;
                }
            }

            throw new SerializationException($"Could not convert base class {objectType}");
        }

        private bool IsKnownTypeMatch(IDictionary<string, JToken> originalJsonObject, IDictionary<string, JToken> knownTypeCandidate)
        {
            var originalKeys = originalJsonObject.Keys.ToList();
            var candidateKeys = knownTypeCandidate.Keys.ToList();

            return (originalKeys.Count == candidateKeys.Count && originalKeys.Intersect(candidateKeys).Count() == originalKeys.Count);
        }

        private JObject SerializeToKnownType(JsonSerializer serializer, object target)
        {
            JObject serializedObject;
            using (var writer = new StringWriter())
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    serializer.Serialize(jsonWriter, target);
                    string json = writer.ToString();
                    serializedObject = JObject.Parse(json);
                }
            }
            return serializedObject;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

    }
}
