using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zetta.Core.Interop {
    public class DevicePayloadFactory {
        private static IDictionary<Type, IList<string>> _monitorsCache = new Dictionary<Type, IList<string>>();

        public static DevicePayload Create<T>(T device) where T : Device {

            var payload = new DevicePayload();
            payload.Properties = Serializer.Serialize(device);
            payload.Allowed = device.Allowed;
            payload.Transitions = device.Transitions;

            payload.SetId = async (dynamic input) => {
                return await Task.Run(() => {
                    device.Id = (string)input;
                    return Create(device);
                });
            };

            payload.SetCreateReadStream = async (dynamic input) => {
                var func = (Func<object, Task<object>>)input.fn;
                device.SetCreateReadStreamFunction(async (name) => {
                    var stream = new ObjectStream();
                    Func<object, Task<object>> onDataFn = async (data) => {
                        stream.Publish((string)data);
                        return await Task.FromResult(0);
                    };
                    var fnInput = new { name = name, onData = onDataFn };
                    await func.Invoke(fnInput);

                    return stream;
                });
                return await Task.FromResult(0);
            };

            payload.SetAvailableFunction = async (dynamic input) => {
                var func = (Func<object, Task<object>>)input.fn;
                device.SetAvailableFunction(async (transition) => {
                    return (bool)await func.Invoke(new { transition = transition });
                });
                return await Task.FromResult(0);
            };

            payload.SetCallFunction = async (dynamic input) => {
                var func = (Func<object, Task<object>>)input.fn;
                device.SetCallFunction(async (transition) => {
                    try {
                        //Console.WriteLine("Invoking transition: {0}", transition);
                        await func.Invoke(new { transition = transition });
                    } catch (Exception ex) {
                        if (ex.Message.StartsWith("Error: Machine cannot use transition")) {
                            throw new InvalidOperationException(ex.Message);
                        }
                    }
                });
                return await Task.FromResult(0);
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