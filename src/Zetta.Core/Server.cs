using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zetta.Core.Interop;

namespace Zetta.Core {
    public class Server {
        private Func<object, Task<object>> _find;
        private Func<object, Task<object>> _observe;
        private Func<object, Task<object>> _prepare;

        public void SetFindFunction(Func<object, Task<object>> find) {
            _find = find;
        }

        public void SetObserveFunction(Func<object, Task<object>> observe) {
            _observe = observe;
        }

        public void SetPrepareFunction(Func<object, Task<object>> prepare) {
            _prepare = prepare;
        }

        public async Task<IEnumerable<T>> Find<T>(string query) where T : Device {
            var results = (string)await _find(query);

            return Serializer.DeserializeArray<T>(results);
        }

        public void Observe<T>(dynamic query, Action<T> callback) where T : Device {
            var queryPayload = new QueryPayload();
            queryPayload.Query = query;
            queryPayload.Callback = (input) => {
                var device = Serializer.DeserializeArray<T>((string)input).First();
                Prepare(device);
                callback.Invoke(device);
                return Task.FromResult<object>(null);
            };

            _observe.Invoke(queryPayload).Wait();
        }

        public void Observe<T1, T2>(dynamic query, Action<T1, T2> callback)
            where T1 : Device
            where T2 : Device {
            var queryPayload = new QueryPayload();
            queryPayload.Query = query;
            queryPayload.Callback = async (input) => {
                var devices = Serializer.DeserializeArray((string)input, new[] { typeof(T1), typeof(T2) }, 2).ToArray();
                foreach (Device d in devices) {
                    await Prepare(d);
                }
                callback.Invoke((T1)devices[0], (T2)devices[1]);
                return Task.FromResult<object>(null);
            };

            _observe.Invoke(queryPayload).Wait();
        }

        private async Task Prepare<T>(T device) where T : Device {
            await _prepare.Invoke(DevicePayloadFactory.Create(device));
        }
    }
}