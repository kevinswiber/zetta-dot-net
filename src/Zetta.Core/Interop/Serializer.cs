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
            return JsonConvert.DeserializeObject<IEnumerable<T>>(json, _settings)
                .Select((device) => DeviceProxy.InterceptDevice(device))
                .ToArray().AsEnumerable();

            // TODO: Remove old code.  Don't think it's needed anymore.
            //       The below code deserializes directly into a proxy
            //       instead of the above, which a) deserializes into a
            //       normal device type and then b) wraps that device
            //       in a proxy.

            /*var propertyMap = typeof(T).GetProperties()
                .Select((prop) => prop.Name)
                .ToDictionary(Resolver.GetResolvedPropertyName);

            var objects = JArray.Parse(json).Children<JObject>();
            var deserialized = objects.Select((obj) => {
                var device = DeviceProxy.Create<T>();
                obj.Properties().ToList().ForEach((prop) => {
                    var name = prop.Name;
                    if (propertyMap.ContainsKey(name)) {
                        var clrPropertyName = propertyMap[name];
                        var clrProperty = typeof(T).GetProperty(clrPropertyName);

                        // TODO: Make this recursive through sub-objects, arrays.
                        var savedValue = Convert.ChangeType(prop.Value, clrProperty.PropertyType);
                        if (prop.Value.Type == JTokenType.Null) {
                            savedValue = null;
                        }

                        clrProperty.SetValue(device, savedValue);
                    }
                });

                return device;
            }).ToArray();

            return deserialized.AsEnumerable();*/
        }

        public static IEnumerable<Device> DeserializeArray(string json, Type[] types, int length) {
            var objects = JArray.Parse(json).Children<JObject>();
            return objects.Select((obj, index) => Deserialize(types[index], obj.ToString())).Cast<Device>().ToArray().AsEnumerable();
        }

        public static ZettaInteropContractResolver Resolver =
            new ZettaInteropContractResolver();
    }
}