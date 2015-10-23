using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zetta.Core.Interop {
    public class PayloadFactory {
        private static IDictionary<Type, IList<string>> _monitorsCache = new Dictionary<Type, IList<string>>();

        public static Payload Create<T>(T proxiedDevice) where T : Device {
            var device = DeviceProxy.RemoveProxy(proxiedDevice);

            var payload = new Payload();
            payload.Properties = Serializer.Serialize(device);
            payload.Allowed = device.Allowed;
            payload.Transitions = device.Transitions;

            payload.SetId = async (dynamic input) => {
                return await Task.Run(() => {
                    device.Id = (string)input;
                    return Create(device);
                });
            };

            var type = typeof(T);
            if (!_monitorsCache.ContainsKey(type)) {
                _monitorsCache[type] = type.GetProperties().Where((info) => {
                    return info.GetCustomAttributes(true)
                            .Where((attribute) => attribute is MonitorAttribute).Any();
                }).Select((info) => info.Name)
                  .Select((name) => Serializer.Resolver.GetResolvedPropertyName(name)).ToList();
            }

            payload.Monitors = _monitorsCache[type];

            return payload;
        }
    }
}