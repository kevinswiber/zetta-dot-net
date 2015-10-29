using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Zetta.Core.Interop {
    public class Serializer {
        private static JsonSerializerSettings _settings = new JsonSerializerSettings();

        static Serializer() {
            _settings.ContractResolver = Resolver;
            _settings.NullValueHandling = NullValueHandling.Include;
        }

        public static string Serialize<T>(T device) where T : Device {
            return JsonConvert.SerializeObject(device, Formatting.None, _settings);
        }

        public static T Deserialize<T>(string json) where T : Device {
            var obj = JsonConvert.DeserializeObject<T>(json, _settings);
            return DeviceProxy.InterceptDevice(obj);
        }

        public static object Deserialize(Type type, string json) {
            var obj = JsonConvert.DeserializeObject(json, type, _settings);
            return DeviceProxy.InterceptDevice(type, Convert.ChangeType(obj, type));
        }

        public static IEnumerable<T> DeserializeArray<T>(string json) where T : Device {
            var t = DeviceProxy.GetInterceptedType<T>();
            var ien = typeof(IEnumerable<>);
            var conversionType = ien.MakeGenericType(t);
            var deserialized = JsonConvert.DeserializeObject(json, conversionType);
            var enumerable = (IEnumerable<T>)deserialized;
            return enumerable.Select((d) => {
                t.GetField("__interceptors").SetValue(d, new[] { new SetterInterceptor() });
                return d;
            }).ToArray().AsEnumerable();
        }

        public static IEnumerable<Device> DeserializeArray(string json, Type[] types, int length) {
            var objects = JArray.Parse(json).Children<JObject>();
            return objects.Select((obj, index) => Deserialize(types[index], obj.ToString())).Cast<Device>().ToArray().AsEnumerable();
        }

        public static ZettaInteropContractResolver Resolver =
            new ZettaInteropContractResolver();
    }
}