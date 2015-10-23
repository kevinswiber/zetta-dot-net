using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
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
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        public static IEnumerable<T> DeserializeArray<T>(string json) where T : Device {
            //return JsonConvert.DeserializeObject<T>(json, _settings);
            var propertyMap = typeof(T).GetProperties()
                .Select((prop) => prop.Name)
                .ToDictionary(Serializer.Resolver.GetResolvedPropertyName);

            var objects = JArray.Parse(json).Children<JObject>();
            var deserialized = objects.Select((obj) => {
                var device = DeviceProxy.Create<T>();
                obj.Properties().ToList().ForEach((prop) => {
                    var name = prop.Name;
                    if (propertyMap.ContainsKey(name)) {
                        var clrPropertyName = propertyMap[name];
                        var clrProperty = typeof(T).GetProperty(clrPropertyName);

                        var savedValue = Convert.ChangeType(prop.Value, clrProperty.PropertyType);
                        if (prop.Value.Type == JTokenType.Null) {
                            savedValue = null;
                        }

                        clrProperty.SetValue(device, savedValue);
                    }
                });

                return device;
            }).ToArray();

            return deserialized.AsEnumerable();
        }

        //public static CamelCasePropertyNamesContractResolver Resolver = new CamelCasePropertyNamesContractResolver();
        public static ZettaInteropContractResolver Resolver = new ZettaInteropContractResolver();
    }
}
