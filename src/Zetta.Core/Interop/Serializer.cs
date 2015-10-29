using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Zetta.Core.Interop {
    public class Serializer {
        private static JsonSerializerSettings _settings = new JsonSerializerSettings();
        private static IList<Type> _deserializedTypes = new List<Type>();

        static Serializer() {
            _settings.ContractResolver = Resolver;
            _settings.NullValueHandling = NullValueHandling.Include;
        }

        public static string Serialize<T>(T device) where T : Device {
            return JsonConvert.SerializeObject(device, Formatting.None, _settings);
        }

        public static IEnumerable<T> DeserializeArray<T>(string json) where T : Device {
            EnsureTypeConverter<T>();

            return JArray.Parse(json)
                .Select((d) => JsonConvert.DeserializeObject<T>(d.ToString(), _settings))
                .ToArray()
                .AsEnumerable();
        }

        public static IEnumerable<Device> DeserializeArray(string json, Type[] types, int length) {
            foreach (var type in types) {
                EnsureTypeConverter(type);
            }

            return JArray.Parse(json)
                .Select((obj, index) => JsonConvert.DeserializeObject(obj.ToString(), types[index], _settings))
                .Cast<Device>()
                .ToArray()
                .AsEnumerable();
        }

        public static void EnsureTypeConverter<T>() where T : Device {
            if (!_deserializedTypes.Contains(typeof(T))) {
                _settings.Converters.Add(new DeviceConverter<T>());
                _deserializedTypes.Add(typeof(T));
            }
        }

        public static void EnsureTypeConverter(Type type) {
            var methodInfo = typeof(Serializer).GetMethods().Where((info) => {
                return info.Name == "EnsureTypeConverter" && info.IsGenericMethod;
            }).First();

            methodInfo.MakeGenericMethod(type).Invoke(null, null);
        }

        public static ZettaInteropContractResolver Resolver =
            new ZettaInteropContractResolver();
    }
}