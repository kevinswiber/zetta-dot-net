using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Zetta {
    public class Interop {
        public Interop() {
            Allowed = new Dictionary<string, string[]>();
            Transitions = new Dictionary<string, TransitionValue>();
        }

        public static Interop Wrap<T>(T device) where T : Device {
            var interop = new Interop();
            interop.Properties = Serialize(device);
            interop.Allowed = device._allowed;
            interop.Transitions = device._transitions;
            interop.OnUpdate = async (dynamic input) => {
                device.update = (Func<object, Task<object>>)input;
                return Wrap(device);
            };

            interop.OnSave = async (dynamic input) => {
                device.save = (Func<object, Task<object>>)input;
                return Wrap(device);
            };

            interop.Fetch = async (dynamic input) => {
                return Wrap(device);
            };

            interop.SetId = async (dynamic input) => {
                device.Id = (string)input;
                return Wrap(device);
            };

            return interop;
        }

        public static string Serialize<T>(T device) where T : Device {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.SerializeObject(device, Formatting.None, settings);
        }

        public string Properties { get; set; }

        public Func<object, Task<object>> OnUpdate { get; set; }

        public Func<object, Task<object>> OnSave { get; set; }

        public Func<object, Task<object>> Fetch { get; set; }

        public Func<object, Task<object>> SetId { get; set; }

        public IDictionary<string, string[]> Allowed { get; private set; }

        public IDictionary<string, TransitionValue> Transitions { get; private set; }
    }
}