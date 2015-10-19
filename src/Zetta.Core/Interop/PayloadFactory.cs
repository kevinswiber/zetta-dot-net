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
            payload.Allowed = device._allowed;
            payload.Transitions = device._transitions;

            payload.OnSync = async (dynamic input) => {
                await Task.Run(() => device.SetSyncFunction((Func<object, Task<object>>)input));
                return Create(device);
            };

            payload.OnSave = async (dynamic input) => {
                return await Task.Run(() => {
                    device.SetSaveFunction((Func<object, Task<object>>)input);
                    return Create(device);
                });
            };

            payload.Fetch = async (dynamic input) => {
                return await Task.FromResult(Create(device));
            };

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
                            .Where((attribute) => attribute is MonitorAttribute).Count() > 0;
                }).Select((info) => info.Name)
                  .Select((name) => Serializer.Resolver.GetResolvedPropertyName(name)).ToList();
            }

            payload.Monitors = _monitorsCache[type];

            return payload;
        }
    }
}