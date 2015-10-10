using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Zetta.Core {
    public class Interop {
        private static JsonSerializerSettings _settings = new JsonSerializerSettings();

        private static IDictionary<Type, IList<string>> _monitorsCache = new Dictionary<Type, IList<string>>();

        public Interop() {
            Allowed = new Dictionary<string, string[]>();
            Transitions = new Dictionary<string, TransitionValue>();
            Monitors = new List<string>();
           
            _settings.ContractResolver = Resolver;
        }

        public static Interop Wrap<T>(T proxiedDevice) where T : Device {
            var device = Device.RemoveProxy(proxiedDevice);

            var interop = new Interop();
            interop.Properties = Serialize(device);
            interop.Allowed = device._allowed;
            interop.Transitions = device._transitions;

            interop.OnSync = async (dynamic input) => {
                device.SetSyncFunction((Func<object, Task<object>>)input);
                return Wrap(device);
            };

            interop.OnSave = async (dynamic input) => {
                device.SetSaveFunction((Func<object, Task<object>>)input);
                return Wrap(device);
            };

            interop.Fetch = async (dynamic input) => {
                return Wrap(device);
            };

            interop.SetId = async (dynamic input) => {
                device.Id = (string)input;
                return Wrap(device);
            };

            var type = typeof(T);
            if (!_monitorsCache.ContainsKey(type)) {
                _monitorsCache[type] = type.GetProperties().Where((info) => {
                    return info.GetCustomAttributes(true)
                            .Where((attribute) => attribute is MonitorAttribute).Count() > 0;
                }).Select((info) => info.Name)
                  .Select((name) => Resolver.GetResolvedPropertyName(name)).ToList();
            }

            interop.Monitors = _monitorsCache[type];

            return interop;
        }

        public static string Serialize<T>(T device) where T : Device {
            return JsonConvert.SerializeObject(device, Formatting.None, _settings);
        }

        public static T DeserializeArray<T>(string json) where T : IEnumerable<Device> {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        public string Properties { get; set; }

        public Func<object, Task<object>> OnSync { get; set; }

        public Func<object, Task<object>> OnSave { get; set; }

        public Func<object, Task<object>> Fetch { get; set; }

        public Func<object, Task<object>> SetId { get; set; }

        public IDictionary<string, string[]> Allowed { get; private set; }

        public IDictionary<string, TransitionValue> Transitions { get; private set; }

        public IList<string> Monitors { get; set; }

        public static CamelCasePropertyNamesContractResolver Resolver = new CamelCasePropertyNamesContractResolver();
    }
}