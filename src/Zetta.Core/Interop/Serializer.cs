using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Zetta.Core.Interop {
    public class Serializer {
        private static JsonSerializerSettings _settings = new JsonSerializerSettings();

        static Serializer() {
            _settings.ContractResolver = Resolver;
        }

        public static string Serialize<T>(T device) where T : Device {
            return JsonConvert.SerializeObject(device, Formatting.None, _settings);
        }

        public static T Deserialize<T>(string json) where T : Device {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        public static T DeserializeArray<T>(string json) where T : IEnumerable<Device> {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        public static CamelCasePropertyNamesContractResolver Resolver = new CamelCasePropertyNamesContractResolver();
    }
}
